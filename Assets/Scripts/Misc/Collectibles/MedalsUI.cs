using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalsUI : MonoBehaviour
{
    public Text title;
    public Text description;
    public Image icon;

    public void getMedal(string[] dat){
        Debug.Log("Got Medal!");
        title.text = dat[1];
        description.text = dat[2];
        icon.sprite = Resources.Load<Sprite>(dat[3]);
        //TODO: Play animation
    }
}
