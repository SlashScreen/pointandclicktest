using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class quip_timeclock : MonoBehaviour
{
    public UnityEvent onTimeLoop;
    public float timebetween;
    public float wobble;
    public bool looping = false;

    public void BeginTimeLoop(){
        looping = true;
        StartCoroutine(Looping());
    }

    public void StopTimeLoop(){
        looping = false;
    }

    IEnumerator Looping(){
        while (looping)
        {
            yield return new WaitForSeconds(timebetween + (Random.Range(-1f,1f)*wobble));
            onTimeLoop.Invoke(); 
        }
    }
}
