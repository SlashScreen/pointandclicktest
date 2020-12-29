using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Dialog/Speaker Controller Data")]
public class AnimControllerData : ScriptableObject
{
    [System.Serializable]
    public struct state{
        public string name;
        public int number;
    }

    public state[] states;

    public int getNumberOfState(string name = "idle"){
        try {
            return Array.Find(states,x => x.name == name).number;
        }
        catch {
            return -1;
        }
    }
}
