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
    public bool pizzaflag;
    public bool wearingShoes = false;
    public bool wearingMask = false;
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
        public InteractiveObject.option[] opts;
    }
    public scenemanager sm;

    public List<itemFlags> flagsl = new List<itemFlags>();
    List<itemFlags> flagslCache = new List<itemFlags>();
    bool shoes;
    bool mask;
    bool purgeNextCycle = false;

    private void Start()
    {
        Directory.CreateDirectory(Application.persistentDataPath + "/saves");
    }

    public void Save(string SaveName, PlayerMain player){
        Debug.Log("Saving Game...");
        //SAVE game
        //TODO: Save custom states for interactive objects
        SaveGame sv = new SaveGame(); //create object
        
        //purge non essential data if Purge is on.
        //to be done each time the player enters a new area that they cannot return from.
        //this will remove all data that will not be used again from the save file, such as activated nodes, and item data.
        //if zero is going to a new area, why save it? this is a way to keep file sizes down in a way that doesnt have me doing like, byte math or whatever
        if(purgeNextCycle){
            player.yarn.activatedNodes = new List<string>(); //purge activated nodes
            flagslCache = new List<itemFlags>();
            flagsl =  new List<itemFlags>();
            purgeNextCycle = false;
        }else{
            NPCscript[] npcs = GameObject.FindObjectsOfType<NPCscript>();
            foreach (var npc in npcs){
                npc.updateTheFlags();
            }
        }
        
        foreach(var item in player.inventory.inventory){ //store item IDs (saves on space and processing time)
            sv.inventory.Add(item.id);
        }
        sv.activatedNodes = player.yarn.activatedNodes; //store nodes
        sv.room = SceneManager.GetActiveScene().name; //store room
        sv.pos = player.transform.position; //store pos
        sv.room = SceneManager.GetSceneAt(1).name; //store room
        //clothes
        sv.wearingShoes = !player.accessories.shoes.hidden;
        sv.wearingMask = !player.accessories.mask.hidden;

        sv.flags = flagsl;
        string JSON = JsonUtility.ToJson(sv); //serialize as JSON
        //write file
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

        player.accessories.shoes.hidden = !sv.wearingShoes;
        player.accessories.mask.hidden = !sv.wearingMask;

        sm.loadscene(sv.room); //load the scene

        player.transform.position = sv.pos; //set pos
        //player.drawer.updateInventory(player.inventory);
        Debug.Log("Game Loaded.");
    }

    public void updateFlags(string name, bool hidden, bool custom,  InteractiveObject.option[] op, int state = 0){ //change flags in internal database
        itemFlags f = new itemFlags();
        f.name = name;
        f.hidden = hidden;
        f.custom = custom;
        f.state = state;
        f.opts = op;

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

    public void PurgeNonEssentialData(){ //call on entering a new area manually
        purgeNextCycle = true;
    }
}
