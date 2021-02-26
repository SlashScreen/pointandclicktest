using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingHeadsController : MonoBehaviour
{
    public AnimControllerData datum;
    public Animator head;
    

    public void SetSpeaker(string speaker){
        List<AnimControllerData.state> ls = new List<AnimControllerData.state> (datum.states);
        int talk = ls[ls.FindIndex(foo => foo.name == speaker.ToLower())].number; //get number of item in datum that matches speaker
        Debug.Log(head);
        head.SetInteger("character",talk);
        head.SetTrigger("switch");
    }

    public void SetTalking(bool a){
        head.SetBool("talking",a);
    }
}
