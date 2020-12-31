using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipText : MonoBehaviour
{
    //Just a script to skip text when clicked, to be applied to the dialogue UI gameobject
    public void OnMove(InputValue input){
        Debug.Log("Skipped");
        GameObject.Find("Dialogue").GetComponent<Yarn.Unity.DialogueUI>().MarkLineComplete();
    }
}
