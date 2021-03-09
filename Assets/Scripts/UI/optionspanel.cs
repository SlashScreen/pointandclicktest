using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class optionspanel : MonoBehaviour 
{
    
    public PanelButtonScript[] buttons;
    public bool hidden;
    
    public void Show(){
        GetComponent<Image>().enabled = true;
        hidden = false;
    }
    public void Hide(){
        GetComponent<Image>().enabled = false;
        foreach(var button in buttons){
            button.Hide();
        }
        hidden = true;
    }
    public void setPosition(Vector3 t){
        transform.position = t;
    } 

    public void OnMove(InputValue input){
        if (!hidden){
            Hide();
        }
    }

    public void setButtons(InteractiveObject obj){ //i wonder if I can generic this stuff
        Debug.Log("setting up buttons");
        if (obj.options.Length == 1){
            buttons[2].setup(obj,obj.options[0]);
        }else{
            buttons[0].setup(obj,obj.options[0]);
            buttons[1].setup(obj,obj.options[1]);
        } 
    }

    public void setButtons_NPC(NPCscript obj){
        Debug.Log("setting up buttons");
        if (obj.options.Count == 1){
            buttons[2].setup_NPC(obj,obj.options[0]);
        }else{
            buttons[0].setup_NPC(obj,obj.options[0]);
            buttons[1].setup_NPC(obj,obj.options[1]);
        } 
    }

    private void Start()
    {
        Hide();
    }
}
