using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingRoomBackdrop : MonoBehaviour
{
    public Sprite noshoes;
    
    public void removeShoes(){
        Debug.Log("shoes removed");
        GetComponent<SpriteRenderer>().sprite = noshoes;
    }
}
