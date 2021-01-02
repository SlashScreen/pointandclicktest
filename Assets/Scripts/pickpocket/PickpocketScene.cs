using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class PickpocketScene : MonoBehaviour
{
    [YarnCommand("Unload")]
    public void unload(){
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    [YarnCommand("Reset")]
    public void reset(){
        //resets all grabbable objects
        foreach(var obj in GameObject.FindObjectsOfType<Grabbable>()){
            obj.reset();
        }
    }
}
