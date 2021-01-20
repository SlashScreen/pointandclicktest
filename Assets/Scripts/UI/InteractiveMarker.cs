using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveMarker : MonoBehaviour
{
    //Misleading name; actually spawns the markers
    public GameObject prefab;
    public void OnShowInteractive(){
        InteractiveObject[] inter = GameObject.FindObjectsOfType<InteractiveObject>(); //get all Interactive Items
        NPCscript[] NPCs = GameObject.FindObjectsOfType<NPCscript>(); //get all NPCs

        //for each item, spawn a ring
        foreach (var i in inter){
            Debug.Log(i.gameObject.name);
            spawn(i.gameObject);
        }
        foreach (var n in NPCs){
            spawn(n.gameObject);
        }
    }

    void spawn(GameObject obj){
        //Instantiate and set parent
        GameObject circle = Instantiate(prefab, obj.GetComponent<Collider2D>().bounds.center, Quaternion.identity);
        circle.transform.SetParent(transform);
    }
}
