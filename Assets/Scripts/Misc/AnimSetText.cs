using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimSetText : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    // Start is called before the first frame update
    public void setText(string s){
        text.text = s;
    }
}
