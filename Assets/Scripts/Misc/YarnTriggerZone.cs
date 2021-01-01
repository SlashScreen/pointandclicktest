using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnTriggerZone : MonoBehaviour
{
    public YarnProgram yarnscript;
    public string node;

    private void Start()
    {
        GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>().dia.Add(yarnscript);
    }
    private void OnCollisionEnter2D(Collision2D other) //if collides with player, play the node
    {
        if(other.gameObject.tag == "Player"){
            GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>().dia.StartDialogue(node);
        }
    }
}
