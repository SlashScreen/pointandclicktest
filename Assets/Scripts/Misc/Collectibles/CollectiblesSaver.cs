using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

[System.Serializable]
public class collectibles
{
    public List<string> cards = new List<string>();
    public List<string> medals = new List<string>();
    public DateTime modified = new DateTime();
}

public class CollectiblesSaver : MonoBehaviour
{
    //NTS: tamper proofing by comparing a timestamp with modified date

    collectibles col;
    public MedalsManager medals;
    public CardsManager cards;
    void Start()
    {
        col = new collectibles();
        try  //try load. if there's an error it means that there was no preexisting save file.
        {
            Load();
        }
        catch (System.Exception)
        {
            Debug.Log("No prior save file for collectibles.");
        }
        
    }

    void Load(){
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/saves/collect.ibles"); //open file
        col = JsonUtility.FromJson<collectibles>(sr.ReadToEnd());
        sr.Close();
        DateTime lastMod = File.GetLastWriteTime(Application.persistentDataPath + "/saves/collect.ibles");
        //TODO: fix tampering
        //if (col.modified != lastMod){
            //Debug.Log("Tampered!!");
        //}else{
            medals.setMedals(col.medals);
            cards.setCards(col.cards);
        //}
    }

    public void Save(){ //nts call from save manager
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/saves/collect.ibles"); //open file

        col.medals = medals.medals; //sync medals
        col.cards = cards.cards; //sync cards
        col.modified = DateTime.Now; //get now for tamper protection

        sw.Write(JsonUtility.ToJson(col)); //write to file in hopefully the same second as now
        sw.Close(); //close file.
    }
}
