using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Yarn.Unity;

public class loadminigame : MonoBehaviour
{
    public string game;
    public UnityEvent[] ev;
    [YarnCommand("LoadMinigame")]
    public void LoadMinigame(string[] args){
        //GetComponent<scenemanager>().LoadSceneAdditive(game);
        foreach(var e in ev){
            e.Invoke();
        }
    }

}
