using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarString : MonoBehaviour
{
    public float amp = 1;
    public float decay = .5f;
    SineWave s;
    float initAmp;
    float modCos = 0f;
    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<SineWave>();
        initAmp = amp;
        amp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        amp -= (amp * decay) * Time.deltaTime; //calculate decay
        if (amp < .01f){
            amp = 0f;
        }
        s.amplitude = amp;
        modCos += .5f; //modifier  = Cos (modCos*pi) . this is for guitar vibration 
        if (modCos > 2f){
            modCos = 0f;
        }
        s.modifier = Mathf.Cos(modCos*Mathf.PI);
    }

    public void Twang(){
        amp = initAmp; //reset string
    }
}
