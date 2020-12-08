using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public string yarnStartNode;
    public YarnProgram yarnDialog;
    public DialogControllerComponent dialog;
    //used for interacting
    public bool Lookable = true;
    public bool Usable = false;

    //TODO: handle interacting with items

    private void Start()
    {
        dialog.dia.Add(yarnDialog);
        dialog.dia.AddCommandHandler("Show",Show);
        dialog.dia.AddCommandHandler("Hide",Hide);
    }
    public void beginDialog(){
        dialog.dia.StartDialogue(yarnStartNode);
    }

    public void Show(string[] n){
        if(n[0] == name){
            gameObject.SetActive(true);
        }
    }

    public void Hide(string[] n){
        if(n[0] == name){
            gameObject.SetActive(false);
        }
    }
}
