using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AccessoryAnimatiorController : MonoBehaviour
{
    public Animator playerAnimator;
    public bool hidden;
    Animator accessoryAnimator;
    SpriteRenderer r;
    Vector3 offset;

    private void Start()
    {
        accessoryAnimator = GetComponent<Animator>();
        r = GetComponent<SpriteRenderer>();
        offset = transform.localPosition;
    }

    void Update()
    {
        /*accessoryAnimator.SetInteger("direction", playerAnimator.GetInteger("direction"));
        accessoryAnimator.SetBool("facing_left", playerAnimator.GetBool("facing_left"));
        accessoryAnimator.SetBool("walking", playerAnimator.GetBool("walking"));
        accessoryAnimator.SetInteger("animation", playerAnimator.GetInteger("animation"));
        transform.localScale = playerAnimator.gameObject.transform.localScale;
        transform.position = playerAnimator.gameObject.transform.position + new Vector3(0,0,-0.1f);// + offset;*/
        r.enabled = !hidden; //dont like calling this every frame but its not a huge issue
    }

    [YarnCommand("Hide")]

    public void hide(){
        hidden = true;
    }

    [YarnCommand("Show")]
    public void show(){
        hidden = false;
    }
}
