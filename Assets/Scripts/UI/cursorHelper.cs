using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public class cursorHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad;
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        GetComponent<cursor>().enabled = true;
        GetComponent<PlayerInput>().enabled = true;
    }
}
