using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class PlayerSpriteController : MonoBehaviour //NTS could absolutley extend a general sprite controller
{
    PlayerControl player;
    public Animator animator;
    public AnimControllerData animationData;
    float horizon;
    public float closePlane = -6.5f; // cole it can get to the 4th wall to be max size
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
        //this is probably able to be more elegant but dont change anything if it's 0
        if (dir.x < 0){
            animator.SetBool("mirrored", true);
            //Debug.Log("mirrored");
        }else if (dir.x > 0){
            animator.SetBool("mirrored", false);
        }
        
        animator.SetBool("walking", dir.magnitude > 0f);
        //perspective
        float scale =  1.5f-((1/((Mathf.Abs(transform.position.y)-horizon))*3));
        //animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder =  (int)scale*-100;
        animator.gameObject.transform.localScale = new Vector3(scale, scale, scale); //set scale
        dropshadow.localScale = new Vector3(scale*2, scale*2, scale*2); //set scale dropshadow
    }

    [YarnCommand("SetSprite")]
    void setSprite(string[] args){
        animator.SetInteger("animation",animationData.getNumberOfState(args[0])); //sets animator to correspoding animation in animationdata table
    }
}
