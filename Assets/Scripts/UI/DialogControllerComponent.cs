using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogControllerComponent : Singleton<DialogControllerComponent>
{
    
    public DialogueRunner dia;

    void Start()
    {
        Show();
        dia = GetComponent<DialogueRunner>();
        dia.AddCommandHandler("Close",HideText);
        Time.timeScale = 1;
    }

    public void HideText(string[] args){
        GameObject.Find("Dialogue Container").SetActive(false);
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }
}
