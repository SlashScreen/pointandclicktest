using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;
using Yarn;

[System.Serializable]
public class card{
    public int id;
    public string graphic;
}

[System.Serializable]
public class cardsObject{ //array of medals
    public card[] cards;
}

public class CardsManager : MonoBehaviour
{
    public UnityEvent<string[]> OnEarned;
    public List<int> cards;
    public string JSONPath;
    public Animator UIAnimator;
    PlayerMain playerMain;
    cardsObject trophycase;
    // Start is called before the first frame update
    void Start()
    {
        string JSONs = File.ReadAllText(JSONPath);
        cards = new List<int>();
        trophycase = JsonUtility.FromJson<cardsObject>(JSONs);
        playerMain = GameObject.Find("Player").GetComponent<PlayerMain>();
        playerMain.d.dia.AddCommandHandler("EarnCard",earnCard);
    }

    public void setCards(List<int> c){
        cards = c;
    }

    public void earnCard(string[] c){
        int toFind = int.Parse(c[0]);
        if (! cards.Contains(toFind)){

            cards.Add(toFind); //add to internal list
            List<string> dat = new List<string>(); //data to pass to OnEarned

            List<card> caseList = new List<card>(trophycase.cards); //create list from array
            card m = caseList.Find(x => x.id == toFind); //get medal object

            //add medal data to list
            dat.Add(m.id.ToString());
            dat.Add(m.graphic);
            UIAnimator.SetTrigger("get");
            //pass data to invoked events
            OnEarned.Invoke(dat.ToArray());
            GetComponent<CollectiblesSaver>().Save();
        }
        
    }
}
