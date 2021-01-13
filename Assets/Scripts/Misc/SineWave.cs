using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    public int points = 10;
    public float length = 5;
    public float amplitude;
    public bool reversed;
    LineRenderer line;
    Vector3[] pts;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        pts = new Vector3[points];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < pts.Length; i++){
            pts[i].x = transform.position.x + ((length/length)*i); //set x
            if(!reversed){ //vibrate string 
                pts[i].y = Mathf.Sin(pts[i].x)*amplitude;
            }else{
                pts[i].y = Mathf.Sin(pts[i].x)*amplitude*-1;
            }
            
            pts[i].z = transform.position.z;
        }

        line.SetPositions(pts);
    }
}
