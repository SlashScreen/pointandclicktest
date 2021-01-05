using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;


public class Album : MonoBehaviour
{
    [System.Serializable]
    public struct album{
        public Sprite sprite;
        public string node;
    }

    public album[] albums;

    private void Start()
    {
        GetComponent<Image>().enabled = false;
    }

    public void SetImage(int index){
        GetComponent<Image>().sprite = albums[index].sprite;
        GetComponent<Image>().enabled = true;
        GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>().dia.StartDialogue(albums[index].node);
    }

    [YarnCommand("Hide")]
    public void Hide(){
        GetComponent<Image>().enabled = false;
    }
}
