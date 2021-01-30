using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;
using Yarn.Unity;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//OK yes I know this is a disASter, orginizationally. at least im not yandev ok

public class PlayerControl : MonoBehaviour{
    //Variables
    //Public vars
    [Header("Movement variables")]
    public Vector3 targetPosition; //where the player will go to next
    public float speed = 650f; //speed of player
    [Tooltip("Distance from path waypoint to be considered 'arrived'")]
    public float nextWaypointDistance = 1f; //distance from path waypoint to be considered "arrived"
    public Vector2 direction = new Vector2(); //movement direction
    [Tooltip("Horizon line of scene")]
    public float horizon;  //horizon line y 
    public bool reachedEndOfPath = false; //if reached end of path
    public Vector2 mouseP = new Vector2();
    public bool goingToTalkPoint = false;
    public bool goingToObject = false; //if the player is walking to an object rahter than a position
    public int ind;
    
    //Private vars
    
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    Seeker seeker; //seeker component
    Rigidbody2D rb; //rigidbody component
    PlayerMain main;

    //Path Functions
    public void GenPath(Vector3 t){ //Generates path from current position to point t
        seeker.StartPath(rb.position, t, OnPathComplete); //calls OnPathComplete once complete
    }

    public void StopPlayer(string[] args){
        currentWaypoint = path.vectorPath.Count; //by doing this i force the player to stop in its tracks
    }

    void OnPathComplete(Path p){ //initiates path follow
        if(!p.error){ //if valid path
            path = p; //set path
            currentWaypoint = 0; //reset current waypoint
        }
    }
    
    public IEnumerator MovePlayer(string[] coords, System.Action onComplete, bool toTalkPoint = false){ //moving the player via code. 2,d argument important for blocking;
        Vector3 target = new Vector3(); //init target
        //convert from string to float
        target.x = float.Parse(coords[0]); //set target x
        target.y = float.Parse(coords[1]); //set target y
        //generate path
        goingToTalkPoint = toTalkPoint;
        GenPath(target);
        //stop script until reachedEndOfPath is true
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        onComplete(); //call this once done, important for blocking
    }

    public IEnumerator MovePlayerToObject(string[] obj, System.Action onComplete, bool toTalkPoint = false){
        Vector3 target = new Vector3(); //init target
        GameObject o = GameObject.Find(obj[0]);
        //convert from string to float
        target.x = o.transform.position.x; //set target x
        target.y = o.transform.position.y; //set target y
        //generate path
        goingToTalkPoint = toTalkPoint;
        GenPath(target);
        //stop script until reachedEndOfPath is true
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        onComplete(); //call this once done, important for blocking
    }

    //Unity loops
    void Start(){
        //get components
        main = GetComponent<PlayerMain>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        
        SceneManager.sceneLoaded += OnLoad;
        //set up yarn commands unique to this player
        
        main.d.dia.AddCommandHandler("StopPlayer",StopPlayer);
        main.d.dia.AddCommandHandler("MovePlayerTo", (parameters, onComplete) => StartCoroutine(MovePlayer(parameters, onComplete))); //Remember, this is used for blocking
        main.d.dia.AddCommandHandler("GoToObject", (parameters, onComplete) => StartCoroutine(MovePlayerToObject(parameters, onComplete))); //Remember, this is used for blocking
    }

    public void OnMousePos(InputValue input){
        mouseP = input.Get<Vector2>();
    }

     void OnLoad(Scene scene, LoadSceneMode sceneMode){
        StopPlayer(new string[] {""});
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
            goingToTalkPoint = false;
            direction = Vector2.zero; //direction is nothing (used for sprite direction control)
            return; //exit
        }else{
            reachedEndOfPath = false; //not reached end of path (set here every frame i believe to stop some problems)
            direction = rb.velocity;//Vector2.MoveTowards (direction,rb.velocity,5f * Time.deltaTime); //direction is vague movement (used for sprite direction control)
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
