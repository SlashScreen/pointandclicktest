using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogControllerComponent : MonoBehaviour
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

    public void BeginSpeakingAnimations(){
        //Debug.Log("Begin Speaking Animations");

        if (GetComponent<SetSpeaker>().voice.GetSilence()){ //if silent, just skip everything 
            return;
        }

        string sp = GetComponent<SetSpeaker>().getSpeaker();

        if (sp.ToLower() == "zero"){
            Debug.Log("zero animation");
            GameObject.Find("Player").GetComponent<PlayerSpriteController>().setSprite(new string[] {"1"}); //yeah hardcoded but w/e
        }else{
            GameObject.Find(sp).GetComponent<NPCscript>().spriteController.setSprite(new string[] {"1"});
        }
    }

    public void StopSpeakingAnimation(){
        if (GetComponent<SetSpeaker>().voice.GetSilence()){ //if silent, just skip everything 
            return;
        }

        string sp = GetComponent<SetSpeaker>().getSpeaker();

        if (sp.ToLower() == "zero"){
            GameObject.Find("Player").GetComponent<PlayerSpriteController>().setSprite(new string[] {"0"}); //yeah hardcoded but w/e
        }else{
            GameObject.Find(sp).GetComponent<NPCscript>().spriteController.setSprite(new string[] {"0"});
        }
    }
    //TODO: NPC stuff
}
