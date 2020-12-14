using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelButtonScript : MonoBehaviour
{
    string node;
    InteractiveObject o = null;
    NPCscript o_npc;
    public PlayerControl player;
    public TMPro.TMP_Text text;
    public optionspanel panel;
 
    public void setup(InteractiveObject obj, InteractiveObject.option opt){
        //setup script for InteractiveObjects
        text.text = opt.tooltip;
        o = obj;
        node = opt.node;
    }

    public void setup_NPC(NPCscript obj, InteractiveObject.option opt){
        //setup script for NPCs
        text.text = opt.tooltip;
        o_npc = obj;
        node = opt.node;
    }

    public void clickedBehavior(){
        //when clicked, move player, and once player is done moving, call beginDialogScripts
        panel.Hide();
        StartCoroutine(player.movePlayer(new string[] {player.targetPosition.x.ToString(),player.targetPosition.y.ToString(),player.targetPosition.z.ToString()},beginDialogScripts));
        
    }

    public void beginDialogScripts(){
        //begin dialogues for both kinds of objects
        if (o != null){
            o.beginDialog(node);
        }else{
            o_npc.beginDialog(node);
        }
    }

}
