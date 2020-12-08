using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public string yarnStartNode;
    public YarnProgram yarnDialog;
    public DialogControllerComponent dialog;

    private void Start()
    {
        dialog.dia.Add(yarnDialog);
    }
    public void beginDialog(){
        dialog.dia.StartDialogue(yarnStartNode);
    }
}
