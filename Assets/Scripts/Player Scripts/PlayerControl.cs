using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;
using Yarn.Unity;
using UnityEngine.InputSystem;

//OK yes I know this is a disASter, orginizationally. at least im not yandev ok

public class PlayerControl : MonoBehaviour{
    //Variables
    //Public vars
    [Header("Movement variables")]
    public Vector3 targetPosition; //where the player will go to next
    public Vector3 UIPosition; //options wheel position
    public float speed = 650f; //speed of player
    [Tooltip("Distance from path waypoint to be considered 'arrived'")]
    public float nextWaypointDistance = 1f; //distance from path waypoint to be considered "arrived"
    public Vector2 direction = new Vector2(); //movement direction
    [Tooltip("Horizon line of scene")]
    public float horizon;  //horizon line y 
    public bool reachedEndOfPath = false; //if reached end of path
    [Header("Inventory UI")]
    [Tooltip("Options wheel")]
    public optionspanel opt; //options wheel
    [Tooltip("Inventory drawer")]
    public DrawerScript drawer; //UI drawer
    public List<drawerItem> inventory = new List<drawerItem>(); //list of item IDs for inventory
    [Header("Yarn stuff")]
    [Tooltip("DialogueControllerComponent")]
    public DialogControllerComponent d;
    public List<string> activatedNodes = new List<string>();
    //Private vars
    JSONItemParser JSON; //json for 
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    Seeker seeker; //seeker component
    Rigidbody2D rb; //rigidbody component
    Vector2 mouseP = new Vector2();
    bool inConversation = false; //is in a conversation
    bool goingToObject = false; //if the player is walking to an object rahter than a position
    InGameDropdown dropdown;
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
    }

    void OnPathComplete(Path p){ //initiates path follow
        if(!p.error){ //if valid path
            path = p; //set path
            currentWaypoint = 0; //reset current waypoint
        }
    }
    //Inventory management fucntions
    drawerItem FindItemWithID(int id){
        //find and returns item in inventory with item id of id
        if (inventory.Exists (x => x.id == id)){ //if there is an item in the list with an id that matches id
            return inventory[inventory.FindIndex(x => x.id == id)]; //return the item
        }
        return new drawerItem(); //if not found, return the default one
    }

    drawerItem NewItem(int id){
        //creates item with id id
        foreach(var di in JSON.drawer.items){ //loop through item config JSON, find object in there with same ID as the inventory
            //nts could be a lambda predicante
            if(di.id == id){//if id matches
                return di; // return
            }
        }
        return new drawerItem(); //if nothing found, returns item id 0; 0 is nothing
    }
    

    public void AddItem(string[] item){
        //Adds item of ID item into inventory
        inventory.Add(NewItem(int.Parse(item[0]))); //calls function with int argument from string 
        drawer.updateInventory(inventory); //updates
    }

    public void RemoveItem(string[] item){ //POTENTIAL ISSUE: if there is no item of ID in inventory it errors out
        //removes item of ID item
        inventory.Remove(NewItem(int.Parse(item[0]))); //calls function with int argument from string 
        drawer.updateInventory(inventory); //updates
    }

    public object ItemInInventory(Yarn.Value[] item){ 
        //Yarn wrapper for findItemWithID. sets variable based on whether it exists
        return inventory.Exists (x => x.id == item[0].AsNumber);
    }

    public void AddCompletedNode(string node){ //might not be necessary
        //adds completed node string name to the list. 
        activatedNodes.Add(node);
    }

    public void CombineItems(string[] items){ 
        //Combines first 2 strings into item 3 and adds to inventory
        RemoveItem(new string[] {items[0]});
        RemoveItem(new string[] {items[1]});
        AddItem(new string[] {items[2]});
    }
    //yarn conversation stuff
    void AddNode(String node){
        activatedNodes.Add(node);
    }
    public void OnDialogStart(){
        inConversation = true;
    }

    public void OnDialogEnd(){
        inConversation = false;
    }

    public IEnumerator MovePlayer(string[] coords, System.Action onComplete){ //moving the player via code. 2,d argument important for blocking;
        Vector3 target = new Vector3(); //init target
        //convert from string to float
        target.x = float.Parse(coords[0]); //set target x
        target.y = float.Parse(coords[1]); //set target y
        //generate path
        Debug.Log("going to point "+target);
        GenPath(target);
        //stop script until reachedEndOfPath is true
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        onComplete(); //call this once done, important for blocking
    }

    public void StartUseDialog(){
        //start the use dialog of clickedObject
        if(clickedObject.GetComponent<InteractiveObject>()){ //for objects
            clickedObject.GetComponent<InteractiveObject>().beginDialog(clickedObject.GetComponent<InteractiveObject>().useNode); //trigger Use node (NPC)
        }else{ //for NPCs
            clickedObject.GetComponent<NPCscript>().beginDialog(clickedObject.GetComponent<NPCscript>().useNode); //trigger Use node (NPC)
        }
    }

    //Unity loops
    void Start(){
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        JSON = GetComponent<JSONItemParser>();
        dropdown = GameObject.Find("Menu").GetComponent<InGameDropdown>();
        //set up yarn commands unique to this player
        d.dia.AddCommandHandler("AddItem",AddItem);
        d.dia.AddCommandHandler("RemoveItem",RemoveItem);
        d.dia.AddCommandHandler("CombineItems",CombineItems);
        d.dia.AddFunction("seeIfHasItem", 1 , ItemInInventory);
        d.dia.AddCommandHandler("MovePlayerTo", (parameters, onComplete) => StartCoroutine(MovePlayer(parameters, onComplete))); //Remember, this is used for blocking
    }

    public void OnMousePos(InputValue input){
        mouseP = input.Get<Vector2>();
    }

    public void OnMove(InputValue input){
        if (inConversation){
            //nothing should happen if youre in a conversation
            return;
        }

        if(!goingToObject) //when clicked and not actively going to an object
        {
            UIPosition = mouseP; //set point where mouse clicked in world space
            Vector2 mousePos2D = Camera.main.ScreenToWorldPoint(mouseP); //gets mouse point in world space

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity ,LayerMask.GetMask("Clickable")); //raycast to find clicked object. Only sees Clickable layer

            if (EventSystem.current.IsPointerOverGameObject()){
                //if any UI clicked, exit out of script
                return;
            }

            if (hit.collider != null) { //if something is clicked on layer clickable
                
                if(hit.collider.gameObject.tag == "Clickable" || hit.collider.gameObject.tag == "Wall"){ //if clicked object has tag "Clickable" or "Wall"
                    //"Clickable" *tag* is interactive objects, thats not totally clear
                    
                    if (hit.collider.gameObject.GetComponent<InteractiveObject>() || hit.collider.gameObject.GetComponent<NPCscript>()){ 
                        //If it is an interactive object or NPC
                        clickedObject = hit.collider.gameObject; //set clicked object

                        if(clickedObject.GetComponent<InteractiveObject>()){ //gets talk points (points in space that the player goes to to interact with object)
                            targetPosition = clickedObject.GetComponent<InteractiveObject>().talkPoint.position;
                        }else{
                            targetPosition = clickedObject.GetComponent<NPCscript>().talkPoint.position;
                        }

                        if (d.gameObject.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber != 0){ //if item in hand from inventory
                            //Start use script once movement complete
                            StartCoroutine(MovePlayer(new string[] {targetPosition.x.ToString(),targetPosition.y.ToString(),targetPosition.z.ToString()},StartUseDialog));
                    
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

                }else if (hit.collider.gameObject.tag == "Player" && d.gameObject.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber != 0){  //if clicked self and something in hand
                    d.dia.StartDialogue("player.use"); //start use dialogue. yes it's hardcoded but i guess it doesnt matter         
                }else{
                    WrapUpMouseCLick();
                }
            }else{
                //mmm yes I know its the same code
                WrapUpMouseCLick();
            }
        }
    }

    void WrapUpMouseCLick(){
        d.gameObject.GetComponent<Yarn.VariableStorage>().SetValue("$selectedInventory",0); //reset hand
        if (opt.hidden && dropdown.hidden){
            targetPosition = Camera.main.ScreenToWorldPoint(mouseP); //set point where mouse clicked in world space
            GenPath(targetPosition); //generate path to mouse point if ground is clicked
        }else{
            opt.Hide();
            dropdown.Cancel();
        }
    }

    void FixedUpdate(){ //physics update
        //Determining if player needs to move
        if(path == null){ //if invalid path, exit loop 
            reachedEndOfPath = false;
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count){ //if current waypoint to follow is beyond the end of the path, reach end of path and stop
            reachedEndOfPath = true; //reached end of path
            path = null; //clear path
            direction = Vector2.zero; //direction is nothing (used for sprite direction control)
            return; //exit
        }else{
            reachedEndOfPath = false; //not reached end of path (set here every frame i believe to stop some problems)
            direction = rb.velocity; //direction is vague movement (used for sprite direction control)
        }

        //Moving player rigidbody
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //find direction between next waypoint and current position
        Vector2 force = dir * speed * Time.deltaTime * new Vector2(1, 1-((1/((Mathf.Abs(transform.position.y)-horizon))*2)) ); //moves speed in a direction, changes y component of direction by a perspective formula thats complicated and I forgot how it works but its based off of 1/x
        
        rb.AddForce(force); //apply force vector
        

        //seeing if waypoint reached
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); //calculate distance between player and next waypoint
        if(distance < nextWaypointDistance){ //if distance is less than the minimum distance from the player to the point,
            currentWaypoint++; //start going to next waypoint
        }
    }
}
