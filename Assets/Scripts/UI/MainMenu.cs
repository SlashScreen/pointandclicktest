using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public scenemanager sm;
    public void play(){
        sm.InitialLoad("Testrange");
    }
    public void load(){
        //TODO load the game which might be hard
    }
    public void options(){

    }
    public void quit(){
        Application.Quit();
    }
}
