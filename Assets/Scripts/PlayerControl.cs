using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerControl : MonoBehaviour
{
    //Variables
    //Public vars
    public Vector3 targetPosition; //where the player will go to next
    public Vector3 UIPosition;
    public float speed = 400f; //speed of player
    public List<int> inventory = new List<int>(); //list of item IDs for inventory
    public float nextWaypoitDistance = 1f; //distance from path waypoint to be considered "arrived"
    public DialogControllerComponent d;
    public optionspanel opt;
    public DrawerScript drawer;
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

    //Functions
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
    public void addItem(string[] item){
        inventory.Add(int.Parse(item[0]));
        drawer.updateInventory(inventory);
    }

    public void removeItem(string[] item){
        inventory.Remove(int.Parse(item[0]));
        drawer.updateInventory(inventory);
    }

    public bool itemInInventory(string[] item){
        return inventory.Contains(int.Parse(item[0]));
    }
    public void combineItems(string[] items){ 
        //Combines first 2 strings into item 3 and adds to inventory
        removeItem(new string[] {items[0]});
        removeItem(new string[] {items[1]});
        addItem(new string[] {items[2]});
    }
    //yarn conversation stuff
    public void OnDialogStart(){
        inConversation = true;
    }

    public void OnDialogEnd(){
        inConversation = false;
    }

    public IEnumerator movePlayer(string[] coords, System.Action onComplete){ //moving the player via code. 2,d argument important for blocking
        //TODO: Wait until moved to continue conversation
        //Debug.Log("begin yield");
        Vector3 target = new Vector3();
        target.x = float.Parse(coords[0]);
        target.y = float.Parse(coords[1]);
        GenPath(target);
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        //Debug.Log("yield");
        onComplete(); //important for blocking
    }

    void Start()
    {
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        d.dia.AddCommandHandler("AddItem",addItem);
        d.dia.AddCommandHandler("RemoveItem",removeItem);
        d.dia.AddCommandHandler("CombineItem",combineItems);
        d.dia.AddCommandHandler("MovePlayerTo", (parameters, onComplete) => StartCoroutine(movePlayer(parameters, onComplete))); //Remember, this is used for blocking
    }

    private void Update() //per frame updates
    {
        if (inConversation){
            return;
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !goingToObject && ! inConversation) //when clicked and not actively going to an object
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //set point where mouse clicked in world space
            UIPosition = Input.mousePosition; //set point where mouse clicked in world space
            Vector2 mousePos2D = new Vector2(targetPosition.x, targetPosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); //raycast to find clicked object
            if (hit.collider != null) {
                if(hit.collider.gameObject.tag == "Clickable" || hit.collider.gameObject.tag == "Wall" || hit.collider.tag == "UI"){ //if clicked object has tag "Clickable" or "Wall"

                    if (hit.collider.gameObject.GetComponent<InteractiveObject>()){
                        clickedObject = hit.collider.gameObject;

                        goingToObject = true;

                        GenPath(targetPosition);
                    }

                }else{
                    GenPath(targetPosition); //generate path to mouse point
                }
            }else{
                GenPath(targetPosition); //generate path to mouse point
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
        if(currentWaypoint >= path.vectorPath.Count){ //if current waypoint to follow is beyond the end of the path,
        
            reachedEndOfPath = true;
            //Debug.Log("reached end of path");

            if (goingToObject){
                //this is for the interact wheel
                goingToObject = false;
                opt.setPosition(UIPosition);
                opt.setButtons(clickedObject.GetComponent<InteractiveObject>());
                opt.Show();
                clickedObject = null;
            }
            
            path = null;
            return;
        }else{
            reachedEndOfPath = false;
            
        }

        //Moving player rigidbody
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; //find direction between next waypoint and current position
        Vector2 force = dir * speed * Time.deltaTime; //calculate force vector
        rb.AddForce(force); //apply force vector

        //seeing if waypoint reached
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]); //calculate distance between player and next waypoint
        if(distance < nextWaypoitDistance){ //if distance is less than the minimum distance from the player to the point,
            currentWaypoint++; //start going to next waypoint
        }

        //TODO: Sprite direction calculation
    }
}
