using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
public class MedalBinder : MonoBehaviour
{
    public GameObject medalObject;
    public string JSONPath;
    medalsObject trophycase;
    // Start is called before the first frame update
    void Start()
    {
        //get cards from file
        string JSONs = File.ReadAllText(JSONPath);
        trophycase = JsonUtility.FromJson<medalsObject>(JSONs);

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

        foreach (var medal in trophycase.medals)
        {
            GameObject mObj = Instantiate<GameObject>(medalObject); //spawn card
            mObj.transform.SetParent(gameObject.transform); //put in container
             //if medal is unlocked
            mObj.GetComponent<MedalBinderSetup>().Setup(medal.icon, medal.title, medal.description, col.medals.Exists(c => c == medal.id)); //give data
            
            //TODO: card back
        }
    }
}
