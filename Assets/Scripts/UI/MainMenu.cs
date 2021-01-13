using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SaveManager saveManager;
    public scenemanager sm;
    public GameObject dropdown;
    public string firstLevel = "Testrange";
    string mode;
    string s;

    //HOW LOADING WORKS:
    //Load button hit
    //call load()
    //dropdown opened by lead has a button that will pass the dropdown text to completeLoad
    //loads persistent, and OnLoad, it will do savemanager.load for that option

    void Start()
    {
        //loadscene("Testrange");
        SceneManager.sceneLoaded += OnLoad;
    }

    public void play(){
        SceneManager.LoadScene("Persistent",LoadSceneMode.Additive);
        mode = "Play";
    }

    public void load(){
        dropdown.SetActive(true);
    }

    public void options(){

    }

    public void quit(){
        Application.Quit();
    }

    public void completeLoad(string save){
        s = save;
        mode = "Load";
        SceneManager.LoadScene("Persistent",LoadSceneMode.Additive);
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        if (scene.name == "Persistent"){
            if (mode == "Play"){
                sm.loadscene(firstLevel);

            }else if (mode == "Load"){
                GameObject.Find("Menu").GetComponent<SaveManager>().Load(s,GameObject.Find("Player").GetComponent<PlayerMain>());
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        }else if(scene.name == firstLevel){
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        }
    }
}
