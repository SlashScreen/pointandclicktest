using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Yarn.Unity;

public class InteractiveObject : MonoBehaviour
{
    
    public string useNode; //items used on the object
    public YarnProgram yarnDialog;
    DialogControllerComponent dialog;
    //used for interacting
    public option[] options;
    public bool customFlag = false;
    public UnityEvent customScript;
    int sprite = 0;
    

    [System.Serializable]
    public struct option{
        public string tooltip;
        public string node;
    }
    

    private void Start()
    {
        dialog = GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>();
        dialog.dia.Add(yarnDialog);
    }
    public void beginDialog(string node){
        dialog.dia.StartDialogue(node);
    }
    
    [YarnCommand("Show")]
    public void Show(){
        gameObject.SetActive(true);
    }
    [YarnCommand("Hide")]
    public void Hide(){
        gameObject.SetActive(false);
    }
    [YarnCommand("Invoke")]
    void InvokeEvent(){
        customScript.Invoke();
    }
    [YarnCommand("SetSprite")]
    void SetSprite(string[] sp){
        sprite = int.Parse(sp[0]);
    }
    [YarnCommand("setCustomFlag")]
    void setFlag(string[] b){
        customFlag = bool.Parse(b[0]);
    }


    int getSprite(){
        return sprite;
    }
}
