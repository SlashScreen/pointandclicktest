using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    public void Unload(){
        //unload scene this script is in
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
