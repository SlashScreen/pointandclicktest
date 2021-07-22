using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnClues : MonoBehaviour
{
    public List<string> clues = new List<string>();

    private void Start()
    {
        GetComponent<PlayerMain>().d.dia.AddFunction("AddClue",1,addClue);
        GetComponent<PlayerMain>().d.dia.AddFunction("HasClue",1,hasFoundClue);
    }

    public object hasFoundClue(Yarn.Value[] v){
        if (v.Length == 1){
            return clues.Exists(x => x == v[0].ToString());
        }else{
            foreach (var item in v)
            {
                if (!clues.Exists(x => x == item.ToString())){
                    return false;
                }
            }
            return true;
        }
        
    }

    public object addClue(Yarn.Value[] v){
        clues.Add(v[0].ToString());
        return null;
    }
}
