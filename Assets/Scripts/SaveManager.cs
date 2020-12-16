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
    public scenemanager sm;

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
        sm.loadscene(sv.room);
        string JSON = JsonUtility.ToJson(sv); //serialize as JSON

        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/saves/" + SaveName +".save");
        sw.Write(JSON);
        sw.Close();
        Debug.Log("Game Saved.");
    }

    public  void Load(string selectedFile, PlayerControl player){
        Debug.Log("Loading Game...");
        //LOAD game
        StreamReader sr = new StreamReader(Application.persistentDataPath + "/saves/" + selectedFile);
        SaveGame sv = JsonUtility.FromJson<SaveGame>(sr.ReadToEnd()); //create savegame from JSON from selectedFile
        sr.Close();

        player.room = sv.room; //set room
        //StartCoroutine(waitForFrames(sv.room)); //Wait 2 frames, because room actually loads on next frame
        sm.loadscene(sv.room);
        Debug.Log("Done");

        player.activatedNodes = sv.activatedNodes; //set activated nodes
        foreach(var item in sv.inventory){
            player.addItem(new string[] { item.ToString() }); //Add item to inventory
        }
        Debug.Log("Game Loaded.");
    }

    void finishLoad(SaveGame sv, PlayerControl player){
        player.activatedNodes = sv.activatedNodes; //set activated nodes
        foreach(var item in sv.inventory){
            player.addItem(new string[] { item.ToString() }); //Add item to inventory
        }
        Debug.Log("Game Loaded.");
    }

    private IEnumerator waitForFrames(string name){
        Debug.Log("Yielding");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            Debug.Log("Not Done");
            yield return null;
        }
        yield return 0;
        Debug.Log("Done async");
        
    }
}
