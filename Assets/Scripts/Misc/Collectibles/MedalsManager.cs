using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;
using Yarn;

[System.Serializable]
public class medal{
    public string id;
    public string title;
    public string description;
    public string icon;
}

[System.Serializable]
public class medalsObject{ //array of medals
    public medal[] medals;
}

public class MedalsManager : MonoBehaviour
{
    public UnityEvent<string[]> OnEarned;
    public List<string> medals;
    public string JSONPath;
    public Animator UIAnimator;
    PlayerMain playerMain;
    medalsObject trophycase;
    // Start is called before the first frame update
    void Start()
    {
        string JSONs = File.ReadAllText(JSONPath);
        medals = new List<string>();
        trophycase = JsonUtility.FromJson<medalsObject>(JSONs);
        playerMain = GameObject.Find("Player").GetComponent<PlayerMain>();
        playerMain.d.dia.AddCommandHandler("EarnMedal",earnMedal);
    }

    public void setMedals(List<string> c){
        medals = c;
    }

    public void earnMedal(string[] c){
        string toFind = c[0];
        if (! medals.Contains(toFind)){

            medals.Add(toFind); //add to internal list
            List<string> dat = new List<string>(); //data to pass to OnEarned

            List<medal> caseList = new List<medal>(trophycase.medals); //create list from array
            medal m = caseList.Find(x => x.id == toFind); //get medal object

            //add medal data to list
            dat.Add(m.id);
            dat.Add(m.title);
            dat.Add(m.description);
            dat.Add(m.icon);

            UIAnimator.SetTrigger("get");

            //pass data to invoked events
            OnEarned.Invoke(dat.ToArray());
            GetComponent<CollectiblesSaver>().Save();
        }
        
    }
}
