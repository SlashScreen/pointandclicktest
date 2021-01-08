using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AudioPlayer : MonoBehaviour
{
    [YarnCommand("PlaySFX")]
    public IEnumerator playSFX(string[] clipSource){
        //play SFX and wait until done
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+clipSource[0]);
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitUntil(() => GetComponent<AudioSource>().isPlaying == false);
    }

    [YarnCommand("StartSFX")]
    public void startSFX(string[] clipSource){
        //Play SFX but not wait until done
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+clipSource[0]);
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
    }

}
