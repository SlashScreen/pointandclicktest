using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelButtonScript : MonoBehaviour
{
    UnityEngine.UI.Button button;
    string node;
    InteractiveObject o = null;
    NPCscript o_npc;
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

    public void setup_NPC(NPCscript obj, InteractiveObject.option opt){
        text.text = opt.tooltip;
        o_npc = obj;
        node = opt.node;
    }

    public void clickedBehavior(){
        if (o != null){
            o.beginDialog(node);
        }else{
            o_npc.beginDialog(node);
        }
        
        panel.Hide();
    }

}
