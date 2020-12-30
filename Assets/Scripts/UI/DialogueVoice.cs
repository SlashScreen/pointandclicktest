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
    
    private void Start(){
        source = GetComponent<AudioSource>(); //get audio source
        SetSpeaker(speaker);
    }

    public void BeginSpeaking(){ //wrapper for StartSpeaking
        StartCoroutine(StartSpeaking());
    }

    IEnumerator StartSpeaking(){
        isSpeaking = true;
        while (isSpeaking){ //loop while speaking
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
}
