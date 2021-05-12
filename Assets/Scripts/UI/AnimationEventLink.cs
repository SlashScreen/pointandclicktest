using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Link
{
    public UnityEvent events;
    public void activate(){
        events.Invoke();
    }
}

public class AnimationEventLink : MonoBehaviour
{
    public Link[] links;
    public void activatelink(int index){
        links[index].activate();
    }
}
