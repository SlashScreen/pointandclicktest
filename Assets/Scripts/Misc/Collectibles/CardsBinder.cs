using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class CardsBinder : MonoBehaviour
{
    public GameObject cardObj;
    public string JSONPath;
    cardsObject trophycase;
    // Start is called before the first frame update
    void Start()
    {
        //get cards from file
        string JSONs = File.ReadAllText(JSONPath);
        trophycase = JsonUtility.FromJson<cardsObject>(JSONs);

        //Get saved collectibles
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/saves/collect.ibles"); //open file
        collectibles col = JsonUtility.FromJson<collectibles>(sr.ReadToEnd());
        sr.Close();

        //tamper check
        DateTime lastMod = File.GetLastWriteTime(Application.persistentDataPath + "/saves/collect.ibles");
        if (col.t != lastMod.ToString("yyyyMMddHHmmss")){
            Debug.Log("Tampered!!");
            return;
        }

        foreach (var card in trophycase.cards)
        {
            GameObject cObj = Instantiate<GameObject>(cardObj); //spawn card
            cObj.transform.SetParent(gameObject.transform); //put in container
            if (col.cards.Exists(c => c == card.id)){ //if card is unlocked
                cObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(card.graphic); //give image
            }
            //TODO: card back
        }
    }
}
