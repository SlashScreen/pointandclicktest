using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelButtonScript : MonoBehaviour
{
    UnityEngine.UI.Button button;
    string node;
    InteractiveObject o;
    public TMPro.TMP_Text text;
    public optionspanel panel;
    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
    }

    public void setup(InteractiveObject obj, InteractiveObject.option opt){
        text.text = opt.tooltip;
        o = obj;
        node = opt.node;
    }

    public void clickedBehavior(){
        o.beginDialog(node);
        panel.Hide();
    }

}
