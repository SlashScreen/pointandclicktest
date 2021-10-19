using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerYarn : MonoBehaviour
{
    [Header("Yarn stuff")]
    public List<string> activatedNodes = new List<string>();
    public List<string> persistentNodes = new List<string>();
    public bool inConversation;
    public bool pizzaflag = false;
    public bool jesusflag = false;
    PlayerMain main;

    private void Start()
    {
        main = GetComponent<PlayerMain>();
        main.d.dia.AddFunction("Visited",1,IsNodeVisited);
        main.d.dia.AddCommandHandler("SelectRandom",SelectRandom);
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
        if (main.d.dia.Dialogue.GetTagsForNode(main.d.dia.CurrentNodeName).ToString().Contains("persist") && !persistentNodes.Contains(main.d.dia.CurrentNodeName)){
            persistentNodes.Add(main.d.dia.CurrentNodeName);
        }
        if (!activatedNodes.Contains(main.d.dia.CurrentNodeName)){
            //Debug.Log("added node " + main.d.dia.CurrentNodeName);
            activatedNodes.Add(main.d.dia.CurrentNodeName);
        }
    }

    public object IsNodeVisited(Yarn.Value[] name){ //returns if node already visited
        //Debug.Log("Visited:" + activatedNodes.Contains(name[0].AsString));
        return activatedNodes.Contains(name[0].AsString) || persistentNodes.Contains(name[0].AsString);
    }

    public void StartUseDialog(){
        //start the use dialog of clickedObject
        if(main.ui.clickedObject.GetComponent<InteractiveObject>()){ //for objects
            main.ui.clickedObject.GetComponent<InteractiveObject>().beginDialog(main.ui.clickedObject.GetComponent<InteractiveObject>().useNode); //trigger Use node (NPC)
        }else{ //for NPCs
            main.ui.clickedObject.GetComponent<NPCscript>().beginDialog(main.ui.clickedObject.GetComponent<NPCscript>().useNode); //trigger Use node (NPC)
        }
    }

    public void SelectRandom(string[] max){
        int m = int.Parse(max[0]);
        main.d.GetComponent<Yarn.VariableStorage>().SetValue("Random",UnityEngine.Random.Range(0,m));
    }
}
