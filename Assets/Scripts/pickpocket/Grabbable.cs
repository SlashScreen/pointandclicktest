using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool goal; //is the thing to grab
    Vector2 inititalPos;
    Rigidbody2D rb;
    void Start()
    {
        inititalPos = GetComponent<Rigidbody2D>().position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void reset(){
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.position = inititalPos;
    }
    
}
