using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipText : MonoBehaviour
{
    //Just a script to skip text when clicked, to be applied to the Player
    public void OnMove(InputValue input){
        skip();
    }

    public void OnSkip(InputValue input){
        skip();
    }

    void skip(){
        GameObject.Find("Dialogue").GetComponent<Yarn.Unity.DialogueUI>().MarkLineComplete();
    }
}
