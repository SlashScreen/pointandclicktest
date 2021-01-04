﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class LivingRoom_TV : MonoBehaviour
{
    int currentChannel = 0;
    int maxChannel = 4; //5 channels including 0

    [YarnCommand("ChangeChannel")]
    public IEnumerator ChangeChannel(){
        Debug.Log("channel changed");
        currentChannel++;
        if (currentChannel > maxChannel){
            currentChannel = 0;
        }
        GetComponent<Animator>().SetInteger("channel", currentChannel);
        GameObject.Find("Dialogue").GetComponent<Yarn.Unity.VariableStorageBehaviour>().SetValue("$channel",currentChannel);
        yield return 0;
    }
}
