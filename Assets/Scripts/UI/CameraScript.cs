using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    CinemachineBrain brain;
    private void OnEnable(){
        SceneManager.sceneLoaded += OnLoad; //set up onloaded yield
    }

    void OnDisable(){
        SceneManager.sceneLoaded -= OnLoad; //unbind yield
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        Debug.Log(scene);
        brain = GetComponent<CinemachineBrain>();
        StartCoroutine(FocusPlayer());
    }

    IEnumerator FocusPlayer(){
        yield return new WaitUntil(() => brain.ActiveVirtualCamera != null); //wait until camera exists
        Debug.Log(brain.ActiveVirtualCamera);
        FocusCamera(new string[] {"Player"}); //when scene load, focus camera on player
        Debug.Log(brain.ActiveVirtualCamera.Follow.gameObject.name);
    }

    [YarnCommand("FocusCamera")]
    public void FocusCamera(string[] target){
        try{
            brain.ActiveVirtualCamera.Follow = GameObject.Find(target[0]).transform; //set follow camera target to gameobject
        }catch (System.Exception e){
            Debug.LogError("You used the wrong name for " + target[0] + " in a yarn script for FocusCamera: "+e);
        }
    }
}
