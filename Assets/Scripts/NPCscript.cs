using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;
using Yarn.Unity;

public class NPCscript : MonoBehaviour
{
    //Variables
    //Public vars
    public Vector3 targetPosition; //where the player will go to next
    public float speed = 400f; //speed of player
    public float nextWaypoitDistance = 1f; //distance from path waypoint to be considered "arrived"
    DialogControllerComponent d;
    public YarnProgram yarnDialog;
    public string useNode; //items used on the object
    public bool customFlag;
    public InteractiveObject.option[] options; //options

    //Private vars
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    public bool reachedEndOfPath = false; //if reached end of path
    Seeker seeker; //seeker component
    Rigidbody2D rb; //rigidbody component
    
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

    //yarn stuff
     public void beginDialog(string node){
        d.dia.StartDialogue(node);
    }

    [YarnCommand("Show")]
    public void Show(){
        gameObject.SetActive(true);
    }

    [YarnCommand("Hide")]
    public void Hide(){
        gameObject.SetActive(false);
    }

    [YarnCommand("setCustomFlag")]
    void setFlag(string[] b){
        customFlag = bool.Parse(b[0]);
    }

    public IEnumerator moveNPC(string[] coords, System.Action onComplete){ //moving the player via code. 2,d argument important for blocking
        //TODO: Wait until moved to continue conversation
        //Debug.Log("begin yield");
        Debug.Log(coords[0]);
        if(coords[0] == gameObject.name){
            Vector3 target = new Vector3();
            target.x = float.Parse(coords[1]);
            target.y = float.Parse(coords[2]);
            GenPath(target);
            yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        //Debug.Log("yield");
            onComplete(); //important for blocking
        }  
    }

    void Start()
    {
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        d = GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>();
        d.dia.Add(yarnDialog);
        //d.dia.AddCommandHandler("Show",Show);
        //d.dia.AddCommandHandler("Hide",Hide);
        d.dia.AddCommandHandler("MoveNPCTo", (parameters, onComplete) => StartCoroutine(moveNPC(parameters, onComplete))); //Remember, this is used for blocking
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
