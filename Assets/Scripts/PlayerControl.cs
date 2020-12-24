using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;
using Yarn.Unity;

public class PlayerControl : MonoBehaviour
{
    //Variables
    //Public vars
    public Vector3 targetPosition; //where the player will go to next
    public Vector3 UIPosition; //options wheel position
    public float speed = 650f; //speed of player
    public List<drawerItem> inventory = new List<drawerItem>(); //list of item IDs for inventory
    public float nextWaypoitDistance = 1f; //distance from path waypoint to be considered "arrived"
    public DialogControllerComponent d;
    public optionspanel opt;
    public DrawerScript drawer;
    public List<string> activatedNodes = new List<string>();
    public string room;
    
    public Vector2 direction = new Vector2();
    public float perspective = .7f;
    public float horizon;  //horizon line y 
    JSONItemParser JSON;
    //Private vars
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    public bool reachedEndOfPath = false; //if reached end of path
    Seeker seeker; //seeker component
    Rigidbody2D rb; //rigidbody component
    bool inConversation = false;
    bool goingToObject = false; //if the player is walking to an object rahter than a position
    GameObject clickedObject = null; //an interactive object that is clicked
    //Explanation for clickedObject:
    //if an object that is clickable is clicked
    //store a reference to the object
    //set goingToObject to be true
    //then generate a path and walk to the object, and ensure the player cannot click away
    //if at the object, only then start dialog and reset everything

    //Path Functions
    public void GenPath(Vector3 t){ //Generates path from current position to point t
        seeker.StartPath(rb.position, t, OnPathComplete); //calls OnPathComplete once complete
        //Debug.Log("generated path");
    }

    void OnPathComplete(Path p){ //initiates path follow
        if(!p.error){ //if valid path
            path = p; //set path
            currentWaypoint = 0; //reset current waypoint
        }
    }
    //Inventory management fucntions
    drawerItem findItemWithID(int id){
        if (inventory.Exists (x => x.id == id)){
            return inventory[inventory.FindIndex(x => x.id == id)];
        }
        return new drawerItem(); //if not found, return the default one
    }

    drawerItem newItem(int id){
        foreach(var di in JSON.drawer.items){ //loop through item config JSON, find object in there with same ID as the inventory
            if(di.id == id){
                return di;
            }
        }
        return new drawerItem();
    }
    

    public void addItem(string[] item){
        //Adds item of ID item into inventory
        inventory.Add(newItem(int.Parse(item[0])));
        drawer.updateInventory(inventory);
    }

    public void removeItem(string[] item){ //POTENTIAL ISSUE: if there is no item of ID in inventory it errors out
        //removes item of ID item
        inventory.Remove(newItem(int.Parse(item[0])));
        drawer.updateInventory(inventory);
    }

    public object itemInInventory(Yarn.Value[] item){ 
        //Yarn wrapper for findItemWithID. sets variable based on whether it exists
        return inventory.Exists (x => x.id == item[0].AsNumber);
    }

    public void addCompletedNode(string node){
        activatedNodes.Add(node);
    }

    public void combineItems(string[] items){ 
        //Combines first 2 strings into item 3 and adds to inventory
        Debug.Log("combine items");
        removeItem(new string[] {items[0]});
        removeItem(new string[] {items[1]});
        addItem(new string[] {items[2]});
    }
    //yarn conversation stuff
    void addNode(String node){
        activatedNodes.Add(node);
    }
    public void OnDialogStart(){
        inConversation = true;
    }

    public void OnDialogEnd(){
        inConversation = false;
    }

    public IEnumerator movePlayer(string[] coords, System.Action onComplete){ //moving the player via code. 2,d argument important for blocking
        //Debug.Log("begin yield");
        Vector3 target = new Vector3();
        //convert from string to float
        target.x = float.Parse(coords[0]);
        target.y = float.Parse(coords[1]);
        //generate path
        Debug.Log("going to point "+target);
        GenPath(target);
        //stop script until reachedEndOfPath is true
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        //Debug.Log("yield");
        onComplete(); //call this once done, important for blocking
    }

    public void startUseDialog(){
        if(clickedObject.GetComponent<InteractiveObject>()){ //for objects
            clickedObject.GetComponent<InteractiveObject>().beginDialog(clickedObject.GetComponent<InteractiveObject>().useNode); //trigger Use node
        }else{ //for NPCs
            clickedObject.GetComponent<NPCscript>().beginDialog(clickedObject.GetComponent<NPCscript>().useNode); //trigger Use node
        }
    }

    //Unity loops
    void Start()
    {
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        JSON = GetComponent<JSONItemParser>();
        //set up yarn commands
        d.dia.AddCommandHandler("AddItem",addItem);
        d.dia.AddCommandHandler("RemoveItem",removeItem);
        d.dia.AddCommandHandler("CombineItems",combineItems);
        d.dia.AddFunction("seeIfHasItem", 1 , itemInInventory);
        d.dia.AddCommandHandler("MovePlayerTo", (parameters, onComplete) => StartCoroutine(movePlayer(parameters, onComplete))); //Remember, this is used for blocking
    }

    private void Update() //per frame updates
    {
        if (inConversation){
            //nothing should happen if youre in a conversation
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !goingToObject) //when clicked and not actively going to an object
        {
            UIPosition = Input.mousePosition; //set point where mouse clicked in world space
            Vector2 mousePos2D = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity ,LayerMask.GetMask("Clickable")); //raycast to find clicked object. Only sees Clickable layer

            if (EventSystem.current.IsPointerOverGameObject()){
                //if UI clicked, exit out of script
                return;
            }

            if (hit.collider != null) { //if something is clicked
                
                if(hit.collider.gameObject.tag == "Clickable" || hit.collider.gameObject.tag == "Wall"){ //if clicked object has tag "Clickable" or "Wall"
                    
                    if (hit.collider.gameObject.GetComponent<InteractiveObject>() || hit.collider.gameObject.GetComponent<NPCscript>()){
                        //If it is an interactive object or NPC
                        clickedObject = hit.collider.gameObject; //set clicked object

                        if(clickedObject.GetComponent<InteractiveObject>()){ //gets talk points
                            targetPosition = clickedObject.GetComponent<InteractiveObject>().talkPoint.position;
                        }else{
                            targetPosition = clickedObject.GetComponent<NPCscript>().talkPoint.position;
                        }

                        if (d.gameObject.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber != 0){ //if item in hand from inventory
                            //Start use script once movement complete
                            StartCoroutine(movePlayer(new string[] {targetPosition.x.ToString(),targetPosition.y.ToString(),targetPosition.z.ToString()},startUseDialog));
                    
                        }else{//if no item in hand, set up wheel
                            opt.setPosition(UIPosition); //teleport wheel
                            if(clickedObject.GetComponent<InteractiveObject>()){
                                opt.setButtons(clickedObject.GetComponent<InteractiveObject>()); //init wheel for Object. NTS: Could make as one function using a generic?...
                            }else{
                                opt.setButtons_NPC(clickedObject.GetComponent<NPCscript>()); //init wheel for NPC
                            }
                            opt.Show(); //show wheel
                        }
                    }                

                }else{
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //set point where mouse clicked in world space
                    GenPath(targetPosition); //generate path to mouse point if some other prop or whatever is clicked
                }
            }else{
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //set point where mouse clicked in world space
                GenPath(targetPosition); //generate path to mouse point if ground is clicked
            }
        }

    }
    void FixedUpdate() //physics update
    {
        //Determining if player needs to move

        if(path == null){ //if invalid path, ignore 
            reachedEndOfPath = false;
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count){ //if current waypoint to follow is beyond the end of the path, reach end of path and stop
            reachedEndOfPath = true;
            path = null;
            //Debug.Log("end of path");
            direction = new Vector2();
            return;
        }else{
            reachedEndOfPath = false;
            direction = rb.velocity;
        }

        //Moving player rigidbody
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //find direction between next waypoint and current position
        Vector2 force = dir * speed * Time.deltaTime * new Vector2(1, 1-((1/((Mathf.Abs(transform.position.y)-horizon))*2)) );//Mathf.Exp(Mathf.Abs( horizon - transform.position.y )-2f)*.3f ); //calculate force vector. changes y my e^x for perspective
        
        rb.AddForce(force); //apply force vector
        

        //seeing if waypoint reached
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); //calculate distance between player and next waypoint
        if(distance < nextWaypoitDistance){ //if distance is less than the minimum distance from the player to the point,
            currentWaypoint++; //start going to next waypoint
        }

        //TODO: Sprite direction calculation
    }
}
