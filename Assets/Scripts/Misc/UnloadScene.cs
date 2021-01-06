using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    public void Unload(){
        //unload scene this script is in
        //GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>().dia.Stop();
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
