using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayShoeScript : MonoBehaviour
{
    //literally just for the apartment hallway
    void Start()
    {
        if (GameObject.Find("Player").GetComponent<PlayerInventory>().ItemInInventory_NoYarn(6)){ //if shoe in inventory, change sprite
            GetComponent<LivingRoomBackdrop>().removeShoes();
        }
    }

}
