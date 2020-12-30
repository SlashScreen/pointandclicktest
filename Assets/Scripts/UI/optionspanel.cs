﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.EventSystems;

public class optionspanel : MonoBehaviour 
{
    
    public PanelButtonScript button1;
    public PanelButtonScript button2;
    public void Show(){
        GetComponentInParent<Canvas>().enabled = true;
    }
    public void Hide(){
        GetComponentInParent<Canvas>().enabled = false;
    }
    public void setPosition(Vector3 t){
        transform.position = t;
    }

    public void setButtons(InteractiveObject obj){ //i wonder if I can generic this stuff
        Debug.Log("setting up buttons");
        button1.setup(obj,obj.options[0]);
        button2.setup(obj,obj.options[1]);
    }

    public void setButtons_NPC(NPCscript obj){
        button1.setup_NPC(obj,obj.options[0]);
        button2.setup_NPC(obj,obj.options[1]); 
    }

    private void Start()
    {
        Hide();
    }
}