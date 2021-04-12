using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuBlock : MonoBehaviour
{
    public void PlayerInMenu(){
        GameObject.Find("Player").GetComponent<PlayerMain>().ui.inMenu = true;
    }
    public void PlayerOutOfMenu(){
        GameObject.Find("Player").GetComponent<PlayerMain>().ui.inMenu = false;
    }
}
