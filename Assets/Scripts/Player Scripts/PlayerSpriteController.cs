using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Yarn.Unity;


public class PlayerSpriteController : MonoBehaviour //NTS could absolutely extend a general sprite controller
{
    public Animator animator;
    public AnimControllerData animationData;
    public float dropshadowSize = 2f;
    public float minsize = .2f; 
    public float vOffset = 0f;
    public Transform dropshadow;
    public Vector2 dir = new Vector2();
    public float baseSize = 3f;
    public PlayerControl player;
    
    public float horizon;

    GameObject horizonLine;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad;
    }

    public virtual void Start()
    {
        //sert runtime stuff
        player = gameObject.GetComponent<PlayerControl>();
        horizon = player.horizon;
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        horizonLine = GameObject.Find("HorizonLine");
    }

    public void Update()
    {
        if (horizonLine == null){
            horizonLine = GameObject.Find("HorizonLine");
        }


        getDir();
        //mirroring

        //this is probably able to be more elegant but dont change anything if it's 0
        if (dir.x < 0){
            animator.SetBool("facing_left", true);
        }else if (dir.x > 0){
            animator.SetBool("facing_left", false);
        }

        //Sprite direction
        
        animator.SetBool("walking", dir.magnitude > 0f);

        if(dir.magnitude > 0f){
            int spritedir;
            float ang = Vector2.SignedAngle(Vector2.up,dir); //get angle
            if (ang > 0){ //if angle is less than 0, convert it to be 0-360
                ang = 360 - ang;
            }
            ang = Mathf.Abs(ang);
            spritedir = Mathf.RoundToInt((ang-22.5f)/45);

            animator.SetInteger("direction", spritedir);
        }

        //sorting order
        //Debug.Log(horizonLine);
        animator.gameObject.transform.position = new Vector3 (animator.gameObject.transform.position.x, animator.gameObject.transform.position.y, (horizonLine.transform.position.y - animator.gameObject.transform.position.y) * -1);

        //perspective
        float scale = Mathf.Pow(Mathf.Abs(animator.gameObject.transform.position.y-horizonLine.transform.position.y/2),.8f) * baseSize;//1/Mathf.Abs(transform.position.z + animator.gameObject.transform.position.z);//1/( 15 - Mathf.Abs(animator.gameObject.transform.position.z))*baseSize; //minsize-((1/((Mathf.Abs(transform.position.y)-horizon))*baseSize)); //set scale to 1/(y-horizon)*3
        float shadowScale = scale; //dropshadowSize -((1/((Mathf.Abs(transform.position.y)-horizon))*baseSize)); //set scale for shadow

        transform.localScale = new Vector3(scale, scale, scale); //set scale
        //dropshadow.localScale = new Vector3(shadowScale, shadowScale, shadowScale); //set scale dropshadow
        dropshadow.position = animator.gameObject.transform.position + new Vector3(0,vOffset,0);

    }

    [YarnCommand("SetSprite")]
    public void setSprite(string[] args){
        animator.SetInteger("animation",int.Parse(args[0])); //sets animator to correspoding animation in animationdata table
    }

    public virtual void getDir(){
        dir = player.direction;
    }
}
