using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    public bool dFlag = false;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(dFlag){
            Destroy(gameObject);
        }
    }
}
