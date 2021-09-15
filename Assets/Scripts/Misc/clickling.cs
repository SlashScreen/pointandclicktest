using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class clickling : MonoBehaviour
{
    public UnityEvent onClick; 
    public void Invoke(){
        onClick.Invoke();
    }
}
