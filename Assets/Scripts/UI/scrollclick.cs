using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollclick : MonoBehaviour
{
    public AudioSource click;
    public AudioClip clip;
    public Scrollbar bar;
    public float multiplier = 7.5f;
    float delta;
    float previousValue = 0f;
    
    public void CalcClick(){
        delta = (bar.value - previousValue) * multiplier;
        click.pitch = 1f+Mathf.Abs(delta);
        click.volume = Mathf.Abs(delta)*3f;
        //Debug.Log(bar.value - previousValue);
        previousValue += (bar.value - previousValue) / 2 ;
        click.Play();
        //Debug.Log("delta: "+delta);
        
    }
}
