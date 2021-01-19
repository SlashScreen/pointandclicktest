using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class PlayerSpriteController : MonoBehaviour //NTS could absolutely extend a general sprite controller
{
    public Animator animator;
    public AnimControllerData animationData;
    public float dropshadowSize = 2f;
    public float minsize = .2f; 
    public Transform dropshadow;
    public Vector2 dir = new Vector2();
    public float baseSize = 3f;
    public PlayerControl player;
    
    public float horizon;

    public virtual void Start()
    {
        //sert runtime stuff
        player = gameObject.GetComponent<PlayerControl>();
        horizon = player.horizon;
    }

    public void Update()
    {
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

        //perspective
        float scale =  minsize-((1/((Mathf.Abs(transform.position.y)-horizon))*baseSize)); //set scale to 1/(y-horizon)*3
        float shadowScale = dropshadowSize -((1/((Mathf.Abs(transform.position.y)-horizon))*baseSize)); //set scale for shadow
        //sorting order
        animator.gameObject.transform.position = new Vector3 (animator.gameObject.transform.position.x,animator.gameObject.transform.position.y, (horizon - animator.gameObject.transform.position.y) * -1);
        
        animator.gameObject.transform.localScale = new Vector3(scale, scale, scale); //set scale
        dropshadow.localScale = new Vector3(shadowScale, shadowScale, shadowScale); //set scale dropshadow

    }

    [YarnCommand("SetSprite")]
    public void setSprite(string[] args){
        animator.SetInteger("animation",int.Parse(args[0])); //sets animator to correspoding animation in animationdata table
    }

    public virtual void getDir(){
        dir = player.direction;
    }
}
