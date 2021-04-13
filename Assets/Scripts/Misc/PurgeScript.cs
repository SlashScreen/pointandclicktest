using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgeScript : MonoBehaviour
{
    public void PurgeData(){
        GameObject.Find("Menu").GetComponent<SaveManager>().PurgeNonEssentialData();
    }
}
