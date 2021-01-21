using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerAccessories : MonoBehaviour
{
    public AccessoryAnimatiorController shoes;
    public AccessoryAnimatiorController mask;

    private void Start()
    {
        GetComponent<PlayerMain>().d.dia.AddFunction("HasShoes",0,HasShoes);
        GetComponent<PlayerMain>().d.dia.AddFunction("HasMask",0,HasMask);
    }

    public object HasShoes(Yarn.Value[] args){
        return !shoes.hidden;
    }

    public object HasMask(Yarn.Value[] args){
        return !shoes.hidden;
    }
}
