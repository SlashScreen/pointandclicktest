using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : Singleton<MonoBehaviour>
{
    // Start is called before the first frame update
    int ind;
    void Start()
    {
        //loadscene("Testrange");
        SceneManager.sceneLoaded += OnLoad;
    }

    public void loadscene(string scene, int index = 0){
        ind = index;
        
        for(int i=0; i<SceneManager.sceneCount;i++){
            Debug.Log(SceneManager.GetSceneAt(i).name);
            if (SceneManager.GetSceneAt(i).name != "Persistent"){
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }
        SceneManager.LoadScene(scene,LoadSceneMode.Additive);
        
    }

    public void InitialLoad(string s){
        SceneManager.LoadScene("Persistent");
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
        foreach(var point in GameObject.FindObjectsOfType<PlayerSpawn>()){ //setplayerposition
            Debug.Log(point.index);
            if (point.index == ind ){
                GameObject.Find("Player").transform.position = point.transform.position;
            }
        }
    }
}
