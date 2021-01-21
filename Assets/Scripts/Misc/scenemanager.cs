using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : Singleton<MonoBehaviour>
{
    // Start is called before the first frame update
    int ind;
    bool doOnLoadBehavior = true;
    void Start()
    {
        //loadscene("Testrange");
        SceneManager.sceneLoaded += OnLoad;
    }

    public void loadscene(string scene, int index = 0){
        GameObject.Find("Player").GetComponent<PlayerControl>().ind = index;
        
        for(int i=0; i<SceneManager.sceneCount;i++){
            //Debug.Log(SceneManager.GetSceneAt(i).name);
            if (SceneManager.GetSceneAt(i).name != "Persistent"){
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }
        SceneManager.LoadScene(scene,LoadSceneMode.Additive);
    }

    public void InitialLoad(string s){
        SceneManager.LoadScene("Persistent");
    }

    public void LoadSceneAdditive(string s){ //meant for minigames
        doOnLoadBehavior = false;
        SceneManager.LoadScene(s,LoadSceneMode.Additive);
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        if (doOnLoadBehavior){
            foreach(var point in GameObject.FindObjectsOfType<PlayerSpawn>()){ //setplayerposition
                if (point.index == GameObject.Find("Player").GetComponent<PlayerControl>().ind ){
                    GameObject.Find("Player").transform.position = point.transform.position;
                }
            }
        }
        else{
            doOnLoadBehavior = true;
        }
    }
}
