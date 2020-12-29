using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Yarn.Unity;

public class MusicScript : MonoBehaviour
{
    public AudioClip music;

    private void Start()
    {
        gameObject.GetComponent<AudioSource>().clip = music;
        gameObject.GetComponent<AudioSource>().Play();
    }

    [YarnCommand("PlayMusic")]
    void playMusic(string[] clipSource){
        gameObject.GetComponent<AudioSource>().Pause();
        AudioClip clip = Resources.Load<AudioClip>("Music/"+clipSource[0]+".ogg");
        StartCoroutine(startMusic(clip));
    }

    IEnumerator startMusic(AudioClip clip){
        //Play the track, then continue
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        yield return new WaitUntil(() => GetComponent<AudioSource>().isPlaying == false);
        gameObject.GetComponent<AudioSource>().UnPause();
    }

    
}
