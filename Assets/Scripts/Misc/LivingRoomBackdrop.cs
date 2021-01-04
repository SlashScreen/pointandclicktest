using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingRoomBackdrop : MonoBehaviour
{
    public Sprite noshoes;
    private void Start()
    {
        GameObject.Find("Dialogue").GetComponent<DialogControllerComponent>().dia.AddCommandHandler("removeShoesLivingRoom",removeShoes);
    }
    public void removeShoes(string[] s){
        Debug.Log("shoes removed");
        GetComponent<SpriteRenderer>().sprite = noshoes;
    }
}
