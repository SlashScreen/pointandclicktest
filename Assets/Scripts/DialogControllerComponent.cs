using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogControllerComponent : Singleton<DialogControllerComponent>
{
    
    public DialogueRunner dialogRunner;

    void Start()
    {
        Show();
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

}
