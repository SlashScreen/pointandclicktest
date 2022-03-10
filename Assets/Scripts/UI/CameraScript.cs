using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    //public bool forceZ = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad; //set up onloaded yield
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoad; //unbind yield
    }

    private void Update()
    {
        //if(forceZ){
        //GameObject b = GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
        // b.transform.position = new Vector3 (b.transform.position.x,b.transform.position.y,-90);
        //}
        try
        {
            if (GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow.Equals(null))
            {
                StartCoroutine(FocusPlayer());
            }
        }
        catch
        {
            StartCoroutine(FocusPlayer());
        }

    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Loading cam...");
        //brain = GetComponent<CinemachineBrain>();
        StartCoroutine(FocusPlayer());
        Camera cam = GetComponent<Camera>();
        GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.AddCommandHandler("FocusCamera", FocusCamera);
        Debug.Log("cam loaded");
        //cam.enabled = false;
        //cam.enabled = true;
    }

    IEnumerator FocusPlayer()
    {
        yield return new WaitUntil(() => GetComponent<CinemachineBrain>().ActiveVirtualCamera != null); //wait until camera exists
        FocusCamera(new string[] { "Player" }); //when scene load, focus camera on player
    }

    public void FocusCamera(string[] target)
    {
        try
        {
            GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = GameObject.Find(target[0]).transform; //set follow camera target to gameobject
        }
        catch (System.Exception e)
        {
            Debug.LogError("You used the wrong name for " + target[0] + " in a yarn script for FocusCamera: " + e);
        }
    }
}
