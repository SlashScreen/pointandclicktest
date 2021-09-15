using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Miscinvoker : MonoBehaviour
{
    public UnityEvent miscevents; 
    public void invokeit(){
        miscevents.Invoke();
    }
}
