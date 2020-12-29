using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioPlayer : MonoBehaviour
{
    [YarnCommand("PlaySFX")]
    IEnumerator playSFX(string[] clipSource){
        //play SFX and wait until done
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+clipSource[0]+".ogg");
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitUntil(() => GetComponent<AudioSource>().isPlaying == false);
    }

    [YarnCommand("StartSFX")]
    void startSFX(string[] clipSource){
        //Play SFX but not wait until done
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+clipSource[0]+".ogg");
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
    }

}
