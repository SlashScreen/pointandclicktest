using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class InteractiveObject : MonoBehaviour
{
    
    public string useNode; //items used on the object
    public YarnProgram yarnDialog;
    DialogControllerComponent dialog;
    //used for interacting
    public option[] options;
    public bool customFlag = false;
    public bool hidden = false;
    public UnityEvent customScript;
    public Transform talkPoint;
    int sprite = 0;
    

    [System.Serializable]
    public struct option{
        public string tooltip;
        public string node;
    }

    

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad;
    }

    private void Start()
    {
        dialog = GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>();
        try{
            dialog.dia.Add(yarnDialog);
        } catch {
            
        }
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        SaveManager.itemFlags flags = GameObject.Find("Menu").GetComponent<SaveManager>().pullFlags(gameObject.name);
        hidden = flags.hidden;
        customFlag = flags.custom;
        Debug.Log("Item is hidden: " + hidden);
        if (hidden) {
            gameObject.SetActive(false);
        }

    }
        
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoad;
    }
    

    public void beginDialog(string node){
        dialog.dia.StartDialogue(node);
    }
    
    [YarnCommand("Show")]

    public void Show(){
        hidden = false;
        updateTheFlags();
        gameObject.SetActive(true);
    }

    [YarnCommand("Hide")]
    public void Hide(){
        hidden = true;
        updateTheFlags();
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
        updateTheFlags();
    }

    void updateTheFlags(){
        GameObject.Find("Menu").GetComponent<SaveManager>().updateFlags(gameObject.name, hidden, customFlag);
    }


    int getSprite(){
        return sprite;
    }
}
