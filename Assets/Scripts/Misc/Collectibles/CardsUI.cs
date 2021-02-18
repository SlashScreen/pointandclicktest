using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsUI : MonoBehaviour
{

    public Image icon;

    public void getMedal(string[] dat){
        Debug.Log("Got Card!");
        icon.sprite = Resources.Load<Sprite>(dat[1]);
        //TODO: Play animation
    }
}
