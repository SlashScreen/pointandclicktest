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
    public Vector3 pos;
    public List<SaveManager.itemFlags> flags = new List<SaveManager.itemFlags>();
}



public class SaveManager : MonoBehaviour{
    [System.Serializable]
    public struct itemFlags
    {
        public string name;
        public bool hidden;
        public bool custom;
        public int state;
    }
    public scenemanager sm;

    public List<itemFlags> flagsl = new List<itemFlags>();
    List<itemFlags> flagslCache = new List<itemFlags>();

    private void Start()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/saves");
    }

    public void Save(string SaveName, PlayerMain player){
        Debug.Log("Saving Game...");
        //SAVE game
        //TODO: Save custom states for interactive objects
        SaveGame sv = new SaveGame(); //create object

        foreach(var item in player.inventory.inventory){ //store item IDs (saves on space and processing time)
            sv.inventory.Add(item.id);
        }
        sv.activatedNodes = player.yarn.activatedNodes; //store nodes
        sv.room = SceneManager.GetActiveScene().name; //store room
        sv.pos = player.transform.position; //store pos
        sv.room = SceneManager.GetSceneAt(1).name; //store room

        sv.flags = flagsl;
        string JSON = JsonUtility.ToJson(sv); //serialize as JSON

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/saves/" + SaveName +".save");
        sw.Write(JSON);
        sw.Close();

        Debug.Log("Game Saved.");
    }

    public void Load(string selectedFile, PlayerMain player){
        Debug.Log("Loading Game...");
        //LOAD game

        //file loading
        flagsl.Clear(); //clear flags list
        player.inventory.inventory.Clear(); //clear inventory
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/saves/" + selectedFile); //open file
        SaveGame sv = JsonUtility.FromJson<SaveGame>(sr.ReadToEnd()); //create savegame from JSON from selectedFile
        sr.Close(); //close file

        player.yarn.activatedNodes = sv.activatedNodes; //set activated nodes
        flagsl = sv.flags; //set flags

        foreach(var item in sv.inventory){ //add all saved items
            player.inventory.AddItem(new string[] { item.ToString() }); //Add item to inventory
        }

        sm.loadscene(sv.room); //load the scene

        player.transform.position = sv.pos; //set pos
        //player.drawer.updateInventory(player.inventory);
        Debug.Log("Game Loaded.");
    }

    public void updateFlags(string name, bool hidden, bool custom, int state = 0){ //change flags in internal database
        itemFlags f = new itemFlags();
        f.name = name;
        f.hidden = hidden;
        f.custom = custom;
        f.state = state;

        if(flagsl.Exists(x => x.name == name)){ //if item with that name already exists, replace with f
            flagsl[flagsl.FindIndex(x => x.name == name)] = f;
        }else{ //else add it
            flagsl.Add(f);
        }
    }

    public itemFlags pullFlags(string name){ //pull flags from savegame
        //PROBLEM: Flags cleared at this function. i dont know why.
        if(flagsl.Exists(x => x.name == name)){ //if item with that name exists, return said item
            return flagsl[flagsl.FindIndex(x => x.name == name)];
        }else{
            return new itemFlags();
        }
    }
}
