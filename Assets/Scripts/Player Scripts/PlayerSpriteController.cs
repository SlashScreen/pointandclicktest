using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class PlayerSpriteController : MonoBehaviour //NTS could absolutely extend a general sprite controller
{
    PlayerControl player;
    public Animator animator;
    public AnimControllerData animationData;
    float horizon;
    public float dropshadowSize = 2f;
    public float minsize = .2f; 
    public Transform dropshadow;
    Vector2 dir = new Vector2();

    void Start()
    {
        player = gameObject.GetComponent<PlayerControl>();
        horizon = player.horizon;
    }

    private void Update()
    {
        dir = player.direction;
        //mirroring

        //this is probably able to be more elegant but dont change anything if it's 0
        if (dir.x < 0){
            animator.SetBool("mirrored", true);
        }else if (dir.x > 0){
            animator.SetBool("mirrored", false);
        }

        //Sprite direction
        
        animator.SetBool("walking", dir.magnitude > 0f);

        if(dir.magnitude > 0f){
            int spritedir;
            float ang = Vector2.SignedAngle(Vector2.up,dir);
            if (ang > 0){ //if angle is less than 0, convert it to be 0-360
                ang = 360 - ang;
            }
            ang = Mathf.Abs(ang);
            spritedir = Mathf.RoundToInt((ang-22.5f)/45);

            animator.SetInteger("direction", spritedir);
        }

        //perspective
        float scale =  minsize-((1/((Mathf.Abs(transform.position.y)-horizon))*3));
        float shadowScale = dropshadowSize -((1/((Mathf.Abs(transform.position.y)-horizon))*3));
        animator.gameObject.transform.position = new Vector3 (animator.gameObject.transform.position.x,animator.gameObject.transform.position.y, (horizon - animator.gameObject.transform.position.y) * -1);
        animator.gameObject.transform.localScale = new Vector3(scale, scale, scale); //set scale
        dropshadow.localScale = new Vector3(shadowScale, shadowScale, shadowScale); //set scale dropshadow

    }

    [YarnCommand("SetSprite")]
    void setSprite(string[] args){
        animator.SetInteger("animation",animationData.getNumberOfState(args[0])); //sets animator to correspoding animation in animationdata table
    }
}
