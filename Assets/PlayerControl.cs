using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerControl : MonoBehaviour
{
    //Variables
    //Public vars
    public Vector3 targetPosition; //where the player will go to next
    public float speed = 400f; //speed of player
    public List<int> inventory = new List<int>(); //list of item IDs for inventory
    public float nextWaypoitDistance = 1f; //distance from path waypoint to be considered "arrived"
    //Private vars
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    bool reachedEndOfPath = false; //if reached end of path
    Seeker seeker; //seeker component
    Rigidbody2D rb; //rigidbody component

    //Functions
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

    public void addItem(int item){
        inventory.Add(item);
    }

    public void removeItem(int item){
        inventory.Remove(item);
    }

    public bool itemInInventory(int item){
        return inventory.Contains(item);
    }

    public void combineItems(int i1, int i2, int r){ //"combines" item i1 and i2 to create r and adds r to inventory
        removeItem(i1);
        removeItem(i2);
        addItem(r);
    }

    void Start()
    {
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() //per frame updates
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) //when clicked
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //set point where mouse clicked in world space
            Vector2 mousePos2D = new Vector2(targetPosition.x, targetPosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); //raycast to find clicked object
            if (hit.collider != null) {
                if(hit.collider.gameObject.tag == "Clickable" || hit.collider.gameObject.tag == "Wall"){ //if clicked object has tag "Clickable" or "Wall"
                    Debug.Log(hit.collider.gameObject.name);
                    //TODO: Interact with object
                    //NTS: have the gen path function be called by the interactible thing
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
            return;
        }
        if(currentWaypoint >= path.vectorPath.Count){ //if current waypoint to follow is beyond the end of the path,
            reachedEndOfPath = true;
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
