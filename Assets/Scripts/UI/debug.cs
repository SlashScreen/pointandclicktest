using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debug : MonoBehaviour
{
    public void log(string s){
        Debug.LogError(s);
    }
}
