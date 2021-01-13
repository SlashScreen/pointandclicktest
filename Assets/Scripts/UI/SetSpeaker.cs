using System.Collections;
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
            speaker = s[0]; 
        }

        if (s.Length > 1) { ///this isn't in the other thing in case you dont want to change voices but you want to change the name in the dialogue box
                text.text = s[1]; //set text to text override. 2nd argument used if you want to be like. ??? is speaking
            }else{
                text.text = s[0]; //set text too  speaker
            }
    }

}
