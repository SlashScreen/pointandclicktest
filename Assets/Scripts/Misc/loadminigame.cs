using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class loadminigame : MonoBehaviour
{
    public string game;
    [YarnCommand("LoadMinigame")]
    public void LoadMinigame(){
        GetComponent<scenemanager>().LoadSceneAdditive(game);
    }


}
