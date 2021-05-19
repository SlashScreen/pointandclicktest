using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTextEngine : MonoBehaviour
{
    public float textspeed = 0.05f;
    public UnityEvent onTextStart;
    public UnityEvent onTextEnd;
    public UnityEvent<string> onTextUpdate;

    public void beginEngine(string text){
        onTextStart.Invoke();
        StartCoroutine(revealText(text));
    }

    public IEnumerator revealText(string text){
        string o = "";
        while (o.Length < text.Length)
        {
            //nts style doesnt work
            yield return new WaitForSeconds(textspeed);
            o += text[o.Length]; //add next character
            onTextUpdate.Invoke(o); //pass to text
        }
        onTextEnd.Invoke();
        yield return null;
    }
}
