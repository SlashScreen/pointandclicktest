using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLoadButton : MonoBehaviour
{
    public Dropdown dropdown;
    int index;
    public void setIndex(int i){
        index = i;
        GetComponent<DoorScript>().room = dropdown.options[i].text;
    }
}
