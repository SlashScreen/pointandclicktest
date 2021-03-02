using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FMVPlayer : MonoBehaviour
{
    public UnityEvent startPlayback;
    public UnityEvent endPlayback;
    UnityEngine.Video.VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.AddCommandHandler("PlayVideo",PlayVideoWrapper);
    }

    public void PlayVideoWrapper(string[] e, System.Action onComplete){ //yarn wrapper for PlayVideo
        StartCoroutine(PlayVideo(e[0],onComplete));
    }

    public IEnumerator PlayVideo(string e, System.Action onComplete){
        UnityEngine.Video.VideoClip clip = Resources.Load<UnityEngine.Video.VideoClip>("FMV/"+e); //load resource
        videoPlayer.Play(); //begin preparation and play
        yield return new WaitUntil(() => videoPlayer.isPrepared == true); //wait until prepared
        startPlayback.Invoke(); //show stuff
        yield return new WaitUntil(() => videoPlayer.isPlaying == false); //mait until stopped playing
        endPlayback.Invoke(); //hide stuff
        onComplete(); //yarn continue
    }
}
