using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerSpriteController : MonoBehaviour
{
    float horizon;
    public float baseScale = 1;
    private void Start()
    {
        horizon = GameObject.Find("Player").GetComponent<PlayerControl>().horizon;
    }
    void Update()
    {
        float scale =  baseScale-((1/((Mathf.Abs(transform.position.y)-horizon))*3));
        transform.localScale = new Vector3(scale, scale, scale);
        //transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y, ((transform.position.y)*-1));
    }
}
