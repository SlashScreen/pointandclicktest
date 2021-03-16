using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteController : PlayerSpriteController
{
    public NPCscript npc;

    public override void getDir(){
        dir = npc.direction;
    }
}
