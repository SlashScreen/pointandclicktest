using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    
    public string useNode; //items used on the object
    public YarnProgram yarnDialog;
    public DialogControllerComponent dialog;
    //used for interacting
    public option[] options;

    [System.Serializable]
    public struct option{
        public string tooltip;
        public string node;
    }
    

    //TODO: handle interacting with items

    private void Start()
    {
        dialog.dia.Add(yarnDialog);
        dialog.dia.AddCommandHandler("Show",Show);
        dialog.dia.AddCommandHandler("Hide",Hide);
    }
    public void beginDialog(string node){
        dialog.dia.StartDialogue(node);
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
