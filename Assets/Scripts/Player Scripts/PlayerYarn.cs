using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerYarn : MonoBehaviour
{
    [Header("Yarn stuff")]
    public List<string> activatedNodes = new List<string>();
    public bool inConversation;
    public bool pizzaflag = false;
    public bool jesusflag = false;
    PlayerMain main;

    private void Start()
    {
        main = GetComponent<PlayerMain>();
        main.d.dia.AddFunction("Visited",1,IsNodeVisited);
        main.d.dia.AddFunction("GetPizzaFlag",0,GetPizzaFlag);
        main.d.dia.AddFunction("GetJesusFlag",0,GetJesusFlag);
        main.d.dia.AddCommandHandler("FlipPizzaFlag",FlipPizzaFlag);
        main.d.dia.AddCommandHandler("FlipPizzaFlag",FlipJesusFlag);
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

    public void AddCompletedNode(){ //might not be necessary
        //adds completed node string name to the list. 
        if (!activatedNodes.Contains(main.d.dia.CurrentNodeName)){
            //Debug.Log("added node " + main.d.dia.CurrentNodeName);
            activatedNodes.Add(main.d.dia.CurrentNodeName);
        }
    }

    public void FlipPizzaFlag(string[] s){
        pizzaflag = true;
    }

    public void FlipJesusFlag(string[] s){
        jesusflag = true;
    }

    public object GetPizzaFlag(Yarn.Value[] s){
        return pizzaflag;
    }

    public object GetJesusFlag(Yarn.Value[] s){
        return jesusflag;
    }

    public object IsNodeVisited(Yarn.Value[] name){ //returns if node already visited
        Debug.Log("Visited:" + activatedNodes.Contains(name[0].AsString));
        return activatedNodes.Contains(name[0].AsString);
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
