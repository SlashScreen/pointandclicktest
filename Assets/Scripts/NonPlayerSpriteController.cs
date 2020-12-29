﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerSpriteController : MonoBehaviour
{
    float horizon;
    public float baseScale = 1;
    public float sortingOffet = -.1f; //minus means forward in layer
    private void Start()
    {
        horizon = GameObject.Find("Player").GetComponent<PlayerControl>().horizon;
    }
    void Update()
    {
        float scale =  baseScale-((1/((Mathf.Abs(transform.position.y)-horizon))*3));
        transform.localScale = new Vector3(scale, scale, scale);
        transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y,  (horizon - (transform.position.y+sortingOffet)) * -1);
    }
}
