﻿using System.Collections;
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
    public DialogControllerComponent d;
    //Private vars
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    bool reachedEndOfPath = false; //if reached end of path
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
        Debug.Log(inventory);
    }

    public void removeItem(string[] item){
        inventory.Remove(int.Parse(item[0]));
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
    void Start()
    {
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        d.dia.AddCommandHandler("AddItem",addItem);
        d.dia.AddCommandHandler("RemoveItem",removeItem);
        d.dia.AddCommandHandler("CombineItem",combineItems);
    }

    private void Update() //per frame updates
    {
        if(inConversation){
            return; 
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && !goingToObject) //when clicked and not actively going to an object
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //set point where mouse clicked in world space
            Vector2 mousePos2D = new Vector2(targetPosition.x, targetPosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero); //raycast to find clicked object
            if (hit.collider != null) {
                if(hit.collider.gameObject.tag == "Clickable" || hit.collider.gameObject.tag == "Wall"){ //if clicked object has tag "Clickable" or "Wall"
                    Debug.Log(hit.collider.gameObject.name);
                    clickedObject = hit.collider.gameObject;
                    goingToObject = true;
                    GenPath(targetPosition);
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
            if (goingToObject){
                goingToObject = false;
                clickedObject.GetComponent<InteractiveObject>().beginDialog();
                clickedObject = null;
            }
            return;
        }else{
            reachedEndOfPath = false;
        }
        if(inConversation){
            return; 
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
