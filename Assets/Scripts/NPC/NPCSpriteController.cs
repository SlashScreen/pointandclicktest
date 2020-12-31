using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteController : PlayerSpriteController
{
    public NPCscript npc;

    public override void Start(){
         //sert runtime stuff
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        horizon = player.horizon;
    }

    public override void getDir(){
        dir = npc.direction;
    }
}
