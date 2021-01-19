using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class DialogueVoice : MonoBehaviour
{
    public string speaker;
    bool isSpeaking;
    AudioSource source;
    AudioClip[] clips;
    int lastClip;
    bool silence; //true = no speaking, false = speaking
    
    private void Start(){
        source = GetComponent<AudioSource>(); //get audio source
        GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>().dia.AddCommandHandler("SetSilence",SetSilence); //add yarn command for set silence
        SetSpeaker(speaker); //set speaker to default
    }

    public void BeginSpeaking(){ //wrapper for StartSpeaking
        StartCoroutine(StartSpeaking());
    }

    IEnumerator StartSpeaking(){
        isSpeaking = true;
        while (isSpeaking && !silence){ //loop while speaking
            source.PlayOneShot(clips[RandomClip()]); //play the sound
            yield return new WaitUntil(() => source.isPlaying == false); //wait until sound is done
        }
    }

    public void StopSpeaking(){
        isSpeaking = false; //speaking is false now
    }

    public void SetSpeaker(string s){ //nts centralize to something that can be connected to yarn 
        speaker = s;
        clips = Resources.LoadAll<AudioClip>("Audio/Voice/"+speaker); //probably a more efficient way than loading and unloading 
    }

    int RandomClip(){
        int n = Random.Range(0, clips.Length-1); //random number
        while (n == lastClip){ //if it's equal to the last clip, create a new one
            n = Random.Range(0, clips.Length-1);
        }
        lastClip = n; //set last clip
        return n;
    }

    public void SetSilence(string[] s){
        silence = bool.Parse(s[0]); //set to bool 
    }

    public bool GetSilence(){
        return silence;
    }
}
