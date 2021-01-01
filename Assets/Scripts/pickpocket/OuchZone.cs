using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class OuchZone : MonoBehaviour
{
    public string node;
    DialogControllerComponent d;
    void Start()
    {
        d = GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Player"){
            d.dia.StartDialogue(node); // Do dialogue
        }
    }
}
