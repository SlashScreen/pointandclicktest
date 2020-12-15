using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SaveGame
{
    public List<int> inventory = new List<int>();
    public List<string> activatedNodes = new List<string>();
    public string room;
}

public class SaveManager : MonoBehaviour{
    private void Start()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/saves");
    }
    public void Save(string SaveName, PlayerControl player){
        Debug.Log("Saving Game...");
        //SAVE game
        //TODO: Save custom states for interactive objects
        SaveGame sv = new SaveGame(); //create object

        foreach(var item in player.inventory){ //store item IDs (saves on space and processing time)
            sv.inventory.Add(item.id);
        }
        sv.activatedNodes = player.activatedNodes; //store nodes
        sv.room = SceneManager.GetActiveScene().name; //store room

        string JSON = JsonUtility.ToJson(sv); //serialize as JSON

        BinaryFormatter bf = new BinaryFormatter(); //do... formatting
        FileStream file = File.Create(Application.persistentDataPath + "/saves/" + SaveName +".save"); //create savefile
        bf.Serialize(file, sv); //inject data
        file.Close(); //close
        Debug.Log("Game Saved.");
    }

    public void Load(string selectedFile, PlayerControl player){
        Debug.Log("Loading Game...");
        //LOAD game
        SaveGame sv = JsonUtility.FromJson<SaveGame>(File.ReadAllText(Application.persistentDataPath + "/saves/" + selectedFile)); //create savegame from JSON from selectedFile

        player.room = sv.room; //set room
        SceneManager.LoadScene(sv.room);
        player.activatedNodes = sv.activatedNodes; //set activated nodes
        foreach(var item in sv.inventory){
            player.addItem(new string[] { item.ToString() }); //Add item to inventory
        }
        Debug.Log("Game Loaded.");
    }
}
