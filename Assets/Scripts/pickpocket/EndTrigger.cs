using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public string finishnode;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Grabbable" && other.gameObject.GetComponent<Grabbable>().goal == true){ //if grabbable and the correct thing
            Debug.Log("Won!"); //TODO: Actually win
            GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.StartDialogue(finishnode);
        }
    }
}
