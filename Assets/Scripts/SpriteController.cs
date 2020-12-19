using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    PlayerControl player;
    public Animator animator;
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
    }
}
