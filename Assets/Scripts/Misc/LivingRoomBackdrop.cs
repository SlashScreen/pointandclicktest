using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LivingRoomBackdrop : MonoBehaviour
{
    //not always for living room; whenever there is an item baked into the backdrop, this is used 
    public Sprite noshoes;

    //on load, see if "custom flag" true. if so, "shoes" taken. it's just piggybacking off the interactive object save system
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad; //set up onloaded yield
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoad; //unbind yield
    }

     void OnLoad(Scene scene, LoadSceneMode sceneMode){ //called once the scene is loaded
        SaveManager.itemFlags flags = GameObject.Find("Menu").GetComponent<SaveManager>().pullFlags(gameObject.name); //get flags from svaegame data
        Debug.Log("Livingroom flags " + flags.custom + ", " + flags.hidden + ", " + flags.name);
        if (flags.custom){ //if custom flag tripped
            removeShoes();
        }
     }
    
    public void removeShoes(){
        Debug.Log("Shoes removed");
        GetComponent<SpriteRenderer>().sprite = noshoes;
        updateTheFlags();
    }

    void updateTheFlags(){
        GameObject.Find("Menu").GetComponent<SaveManager>().updateFlags(gameObject.name, false, true); //sets savegame flag
    }
}
