using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class quipbubble : MonoBehaviour
{

    public TMPro.TextMeshProUGUI text;
    public UnityEvent onBegin;
    public UnityEvent onEnd;

    public IEnumerator ShowText(string line){ //set text as line, wait a bit, then remove
        //Debug.Log("Begin");
        text.text = line; //todo: make stuff move?
        onBegin.Invoke(); //show bubble
        //Debug.Log(line);
        yield return new WaitForSeconds (line.Length*0.2f); //wait
        onEnd.Invoke(); //hide bubble
        text.text = ""; //reset text
    }
}
