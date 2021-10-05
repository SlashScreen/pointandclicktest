using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class mst3k_quips : MonoBehaviour
{
    public quipbubble CBubble; //Crow
    public quipbubble JBubble; //Joel
    public quipbubble SBubble; //Servo 
    public quipbubble ZBubble; //Zero 
    public quipbubble GBubble; //Guest
    public DialogueRunner runner;
    string current;

    public void HandleLine(string[] lineData){
        //switch on line name
        switch (lineData[0].Trim())
        {
            case "C": //Crow
                StartCoroutine(CBubble.ShowText(lineData[2]));
                break;
            case "J": //Joel
                StartCoroutine(JBubble.ShowText(lineData[2]));
                break;
            case "S": //Servo
                StartCoroutine(SBubble.ShowText(lineData[2]));
                break;
            case "Z": //Zero
                StartCoroutine(ZBubble.ShowText(lineData[2]));
                break;
            case "G": //Guest
                StartCoroutine(GBubble.ShowText(lineData[2]));
                break;
            default:
                Debug.Log("No bubble found");
                break;
        }
    }

    public void PlayQuip(string quip){
        runner.StartDialogue("quip."+quip);
    }

    public void PlayQuip(bool canBeSilent,float chanceForSilence,string[] quips){
        if (canBeSilent && (Random.Range(0f,1f)>=chanceForSilence)){ //if can be silent (no quip will play) and random greater than chanceForSilence, play nothing
            return;
        }

        int selectedQuip = Random.Range(0,quips.Length); //choose random quip
        PlayQuip(quips[selectedQuip]); //play quip
    }
}
