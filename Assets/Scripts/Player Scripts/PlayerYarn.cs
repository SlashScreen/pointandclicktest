using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerYarn : MonoBehaviour
{
    [Header("Yarn stuff")]
    public List<string> activatedNodes = new List<string>();
    public bool inConversation;
    PlayerMain main;

    private void Start()
    {
        main = GetComponent<PlayerMain>();
    }


    //yarn conversation stuff
    void AddNode(String node){
        activatedNodes.Add(node);
    }
    public void OnDialogStart(){
        inConversation = true;
    }

    public void OnDialogEnd(){
        inConversation = false;
    }

    public void AddCompletedNode(string node){ //might not be necessary
        //adds completed node string name to the list. 
        activatedNodes.Add(node);
    }

    public void StartUseDialog(){
        //start the use dialog of clickedObject
        if(main.ui.clickedObject.GetComponent<InteractiveObject>()){ //for objects
            main.ui.clickedObject.GetComponent<InteractiveObject>().beginDialog(main.ui.clickedObject.GetComponent<InteractiveObject>().useNode); //trigger Use node (NPC)
        }else{ //for NPCs
            main.ui.clickedObject.GetComponent<NPCscript>().beginDialog(main.ui.clickedObject.GetComponent<NPCscript>().useNode); //trigger Use node (NPC)
        }
    }
}
