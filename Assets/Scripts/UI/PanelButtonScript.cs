using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelButtonScript : MonoBehaviour
{
    string node;
    InteractiveObject o = null;
    NPCscript o_npc;
    PlayerControl player;
    public TMPro.TMP_Text text;
    public optionspanel panel;
    
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        Hide();
    }
    public void setup(InteractiveObject obj, InteractiveObject.option opt){
        //setup script for InteractiveObjects
        Show();
        text.text = opt.tooltip;
        o = obj;
        node = opt.node;
    }

    public void setup_NPC(NPCscript obj, InteractiveObject.option opt){
        //setup script for NPCs
        Show();
        text.text = opt.tooltip;
        o_npc = obj;
        node = opt.node;
    }

    public void clickedBehavior(){
        //when clicked, move player, and once player is done moving, call beginDialogScripts
        panel.Hide();
        StartCoroutine(player.MovePlayer(new string[] {player.targetPosition.x.ToString(),player.targetPosition.y.ToString()},beginDialogScripts, toTalkPoint:true));
        Hide(); //potential problem: doesnt actually do that thing because setactive
    }

    public void beginDialogScripts(){
        //begin dialogues for both kinds of objects
        if (o != null){
            o.beginDialog(node);
        }else{
            o_npc.beginDialog(node);
        }
    }

    public void Show(){
        GetComponent<Image>().enabled = true;
        GetComponent<Button>().enabled = true;
        text.enabled = true;
    }
    public void Hide(){
        GetComponent<Image>().enabled = false;
        GetComponent<Button>().enabled = false;
        text.enabled = false;
    }

}
