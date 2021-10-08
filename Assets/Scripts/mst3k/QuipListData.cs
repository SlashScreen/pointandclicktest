using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Misc/Quip")]
public class QuipListData : ScriptableObject{

    public bool canBeSilent;
    public float chanceForSilence;
    public string[] quips;

}