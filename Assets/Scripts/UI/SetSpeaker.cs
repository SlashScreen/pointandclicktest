﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSpeaker : MonoBehaviour
{
    public DialogueVoice voice;
    public Text text;
    string speaker; 
    void Start(){
        GetComponent<DialogControllerComponent>().dia.AddCommandHandler("SetSpeaker", SetSpeakerTo); //add command handler
    }

    public void SetSpeakerTo(string[] s){ //potential problem: if not called before start voice voice will be wrong
        if (s[0] != speaker){ //makes sure it isnt unnecessarily leading stuff
            voice.SetSpeaker(s[0]); //set voice
            text.text = s[0]; //set text
            speaker = s[0]; 
        }
    }

}
