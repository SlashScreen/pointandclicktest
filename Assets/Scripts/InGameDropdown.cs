using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameDropdown : MonoBehaviour
{
    public PlayerControl player;
    public InputField input;
    public void save(){
        DateTime dt = DateTime.Now;
        string defaultText = dt.ToString("yyyy-MM-dd-HH-mm-ss");
        input.gameObject.SetActive(true);
        input.text = defaultText;
    }

    public void completeSave(){
        GetComponent<SaveManager>().Save(input.text,player);
        input.gameObject.SetActive(false);
    }
}
