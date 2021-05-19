using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DialogueLineParser : MonoBehaviour
{
    public UnityEvent<string> setText;
    public UnityEvent<string[]> setSpeaker;
    public void parseLine(string line){
        //line = GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.dialogueUI;
        string boxLine;
        string[] outArray;
        if(line.Contains(":")){ //if there's a speaker to set
            char[] delimiterchars = {':'};
            string[] parsed = line.Split(delimiterchars,count:2); //split at colon
            boxLine = parsed[1];

            if(parsed[0].Contains("(")){ //if there's a specific nameplate to use
                string internalName = parsed[0].Split('(')[0]; //extract internal speaker name
                int pFrom = parsed[0].IndexOf("(");
                int pTo = parsed[0].IndexOf(")");
                string visibleName = parsed[0].Substring(pFrom, pTo-pFrom); //extract nameplate name

                outArray = new string[] {internalName,visibleName};
            }else{
                outArray = new string[] {parsed[0]};
            }

            setSpeaker.Invoke(outArray); //set the speaker
        }else{
            boxLine = line; //use old version. am i really gonna go in and rewrite all of my previous script? fuck no bro
        }

        setText.Invoke(boxLine.Trim()); //set box text
        
    }
}
