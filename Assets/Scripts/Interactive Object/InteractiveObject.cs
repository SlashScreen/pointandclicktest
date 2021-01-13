using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class InteractiveObject : MonoBehaviour
{
    [Header("Yarn stuff")]
    public string useNode; //items used on the object
    public YarnProgram yarnDialog; //dialogue of the object
    public option[] options; //options to show with the wheel
    [Header("Custom stuff")]
    public bool customFlag = false;
    public UnityEvent customScript;
    [Header("Misc")]
    public bool hidden = false;
    public Transform talkPoint;
    int sprite = 0;
    DialogControllerComponent dialog;
    //used for interacting
    

    [System.Serializable]
    public struct option{
        public string tooltip;
        public string node;
    }

    

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad; //set up onloaded yield
    }

    private void Start()
    {
        dialog = GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>(); //get dialogue controller component form persistent scene
        try{
            dialog.dia.Add(yarnDialog); //try to add the dialog; if it already exists, just exit the script
        } catch {
            return; //exit
        }
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){ //called once the scene is loaded
        SaveManager.itemFlags flags = GameObject.Find("Menu").GetComponent<SaveManager>().pullFlags(gameObject.name); //get flags from svaegame data
        //set the flags
        hidden = flags.hidden;
        customFlag = flags.custom;
        if (hidden) { //if hidden in the save game, hide it
            gameObject.SetActive(false);
        }
    }
        
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoad; //unbind yield
    }
    

    public void beginDialog(string node){ //begin the dialogue set here
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
    public void InvokeEvent(){
        Debug.Log("Invoked");
        customScript.Invoke();
    }
    [YarnCommand("SetSprite")]
    public void SetSprite(string[] sp){
        sprite = int.Parse(sp[0]);
    }
    [YarnCommand("setCustomFlag")]
    public void setFlag(string[] b){
        customFlag = bool.Parse(b[0]);
        updateTheFlags();
    }

    void updateTheFlags(){
        GameObject.Find("Menu").GetComponent<SaveManager>().updateFlags(gameObject.name, hidden, customFlag); //sync f;ags with savegame
    }

    int getSprite(){
        return sprite;
    }
}
