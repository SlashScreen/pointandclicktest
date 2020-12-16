using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : Singleton<MonoBehaviour>
{
    // Start is called before the first frame update
    void Start()
    {
        loadscene("Testrange");
    }

    public void loadscene(string scene){
        for(int i=0; i<SceneManager.sceneCount;i++){
            Debug.Log(SceneManager.GetSceneAt(i).name);
            if (SceneManager.GetSceneAt(i).name != "Persistent"){
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
            }
        }
        SceneManager.LoadScene(scene,LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Persistent"));
    }
}
