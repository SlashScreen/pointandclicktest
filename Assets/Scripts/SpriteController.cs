using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    PlayerControl player;
    public Animator animator;
    public float horizon;  //horizon line y 
    public float closePlane = -5f; // closest it can get to the computer
    public float minsize = .2f; 
    public Transform dropshadow;
    float factor = .15f;
    Vector2 dir = new Vector2();

    void Start()
    {
        player = gameObject.GetComponent<PlayerControl>();
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
        float scale = minsize+Mathf.Abs( horizon - transform.position.y ) * factor; //min size + the difference between sizes * lerp between horizon and close plane
        //Debug.Log(Mathf.Abs( horizon - transform.position.y ) * factor);
        animator.gameObject.transform.localScale = new Vector3(scale, scale, scale); //set scale
        dropshadow.localScale = new Vector3(.5f+scale*1.7f, .5f+scale*1.7f, .5f+scale*1.7f); //set scale dropshadow
    }
}
