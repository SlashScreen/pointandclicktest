using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionspanel : MonoBehaviour
{
    
    public PanelButtonScript button1;
    public PanelButtonScript button2;
    public void Show(){
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
    public void setPosition(Vector3 t){
        Debug.Log("set positions");
        transform.position = t;
    }

    public void setButtons(InteractiveObject obj){
        Debug.Log("set buttons");
        button1.setup(obj,obj.options[0]);
        button2.setup(obj,obj.options[1]);
    }

    public void setButtons_NPC(NPCscript obj){
        Debug.Log("set buttons");
        button1.setup_NPC(obj,obj.options[0]);
        button2.setup_NPC(obj,obj.options[1]);
    }

    private void Start()
    {
        Hide();
    }
}
