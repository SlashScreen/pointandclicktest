using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarString : MonoBehaviour
{
    public float amp = 1;
    public float decay = .5f;
    SineWave s;
    // Start is called before the first frame update
    void Start()
    {
        s = GetComponent<SineWave>();
    }

    // Update is called once per frame
    void Update()
    {
        amp -= (amp * decay) * Time.deltaTime; //calculate decay
        s.amplitude = amp;
        s.reversed = !s.reversed; //flip directions
    }
}
