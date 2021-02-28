using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn;

public class journal : MonoBehaviour
{
    public TMP_Text text;
    List<string> entries = new List<string>();

    private void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.AddFunction("AddJournalEntry",1,addEntry);
    }

    public object addEntry(Yarn.Value[] e){
        
        if (entries.Exists(x => x == e[0].AsString)){
            return null;
        }

        entries.Add(e[0].AsString);
        string tx = "";
        foreach(string ent in entries){
            tx = tx + "-" + ent + "\n";
        }

        text.text = tx;
        //text.m_margin = new Vector4(text.m_margin.x,text.m_margin.y,text.m_margin.z,50 - (entries.Count * 25));

        return null;
        //todo: little animation
    }
}
