using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Pathfinding;
using Yarn.Unity;
using UnityEngine.SceneManagement;

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

    public bool hidden;
    public bool customFlag;
    public Vector2 direction;
    int state;
    public List<InteractiveObject.option> options = new List<InteractiveObject.option>(); //options

    //Private vars
    Path path; //path player needs to take
    int currentWaypoint = 0; //current target waypoint
    public bool reachedEndOfPath = false; //if reached end of path
    public Transform talkPoint;
    public UnityEvent customScript;
    public NPCSpriteController spriteController;
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
        //Debug.Log(t);
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
        hidden = false;
        updateTheFlags();
        gameObject.SetActive(true);
    }

    [YarnCommand("Hide")]
    public void Hide(){
        hidden = true;
        updateTheFlags();
        gameObject.SetActive(false);
    }

    [YarnCommand("SetState")]
    public void setState(string[] s){
        state = int.Parse(s[0]);
        updateTheFlags();
    }

    [YarnCommand("setCustomFlag")]
    void setFlag(string[] b){
        customFlag = bool.Parse(b[0]);
    }
    [YarnCommand("MoveNPC")]
    public IEnumerator moveNPC(string[] coords){//, System.Action onComplete){ //moving the player via code. 2,d argument important for blocking
        //Debug.Log("args " + coords);
        //TODO: Wait until moved to continue conversation
        //Debug.Log("begin yield");
        Vector3 target = new Vector3();
        target.x = float.Parse(coords[0]);
        target.y = float.Parse(coords[1]);
        GenPath(target);
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        //Debug.Log("yield");
        //onComplete(); //important for blocking
          
    }

    [YarnCommand("MoveNPCToObject")]
    public IEnumerator moveNPCToObject(string[] obj){//, System.Action onComplete){ //moving the player via code. 2,d argument important for blocking
        //Debug.Log("args " + coords);
        //TODO: Wait until moved to continue conversation
        //Debug.Log("begin yield");
        Vector3 target = new Vector3();
        GameObject o = GameObject.Find(obj[0]);
        target = o.transform.position;
        GenPath(target);
        yield return new WaitUntil(() => reachedEndOfPath); //important for blocking
        //Debug.Log("yield");
        //onComplete(); //important for blocking
          
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad;
    }

    void Start()
    {
        //get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>(); 
        d = GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>(); //get d (lmao)
        //Debug.Log(d);
        //d.dia.AddCommandHandler("MoveNPC", (parameters, onComplete) => StartCoroutine(moveNPC(parameters, onComplete)));
        //if (!d.dia.NodeExists(useNode)){ //If the script isn't already loaded
        //    d.dia.Add(yarnDialog); //load the script
        //}
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
            direction = Vector2.zero;
            return;
        }else{
            reachedEndOfPath = false;
            direction = rb.velocity;
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

    void OnLoad(Scene scene, LoadSceneMode sceneMode){

        if (this == null){
            return;
        }

        SaveManager.itemFlags flags = GameObject.Find("Menu").GetComponent<SaveManager>().pullFlags(gameObject.name);
        //set flags from save file
        hidden = flags.hidden;
        customFlag = flags.custom;
        state = flags.state;
        if(flags.opts != null && flags.opts.Length != 0){
            options = new List<InteractiveObject.option>(flags.opts);
        }
        //Debug.Log("NPC is hidden: " + hidden);
        if (hidden) {
            gameObject.SetActive(false); //if hidden at start, hide
        }

    }

    public void updateTheFlags(){
        GameObject.Find("Menu").GetComponent<SaveManager>().updateFlags(gameObject.name, hidden, customFlag, options.ToArray(), state);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoad;
        //d.dia.RemoveCommandHandler("MoveNPCTo");
    }

    [YarnCommand("Invoke")]
    public void InvokeScript(string[] s){
        customScript.Invoke();
    }

    [YarnCommand("RemoveOption")]
    public void RemoveOption(string[] s){
        options.Remove(options.Find(x => x.tooltip == s[0]));
    }

    [YarnCommand("AddOption")]
    public void AddOption(string[] s){
        //Debug.Log(s);
        InteractiveObject.option opt;
        opt.tooltip = s[0];
        opt.node = s[1];
        options.Add(opt);
    }

    [YarnCommand("ClearOptions")]
    public void ClearOptions(string[] s){
        options.Clear();
    }
}
