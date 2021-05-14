using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioAudioEngine : MonoBehaviour
{
    float channel;
    float frequency;
    bool play; //if playing
    Dictionary <float,float> tones = new Dictionary<float, float>();

    string code =   @"

		                //Noise n => BPF bp;
                        SawOsc s;
                        //float le;
                        //{0} => le;
                        s => dac;
                        //0.1 => foo.gain;
                        0.3 => s.gain;

                        fun float lerp(float v0,float v1,float t){{
                            return v0 + t * (v1 - v0);
                        }}

			            repeat( 10 )
			            {{
                            {0} => s.freq;
                            //{0} => bp.freq;
				            1::ms => now;
			            }}

	                ";
    private void Start()
    {
        //Debug.Log(tones);
        for (float i = 70f; i < 110f; i+=0.1f)
        {
            //Debug.Log(Mathf.Round(i*10)/10);
            tones.Add(Mathf.Round(i*10)/10,Mathf.Lerp(1f,100f,Random.value));
        }
        //Debug.Log(tones.Count);
    }
    public void EngineStart(){
        //Debug.Log("Staring audio engine...");
        GetComponent<ChuckSubInstance>().RunCode(string.Format(code,frequency));
    }

    public void updateChannel(float angle, bool playing){
        channel = Mathf.Round(Mathf.Lerp(80f,100f,(angle/360))*10)/10;
        //Debug.Log(channel);
        frequency = tones[channel];
        play = playing;
        EngineStart();
    }
}
