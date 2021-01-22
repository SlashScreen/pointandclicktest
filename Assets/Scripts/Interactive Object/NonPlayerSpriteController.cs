using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonPlayerSpriteController : MonoBehaviour
{
    float horizon;
    public float baseScale = 1;
    public float sortingOffet = -.1f; //minus means forward in layer
    public bool scaleToSize = true;
    GameObject horizonLine;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad;
    }
    private void Start()
    {
        horizon = GameObject.Find("Player").GetComponent<PlayerControl>().horizon;
    }

    void OnLoad(Scene scene, LoadSceneMode sceneMode){
        horizonLine = GameObject.Find("HorizonLine");
    }
    void Update()
    {
        if (scaleToSize){
            float scale =  Mathf.Pow(Mathf.Abs(transform.position.y-horizonLine.transform.position.y),1.1f) * baseScale; //calculate scale
            transform.localScale = new Vector3(scale, scale, scale); //set scale
        }
        
        transform.localPosition = new Vector3 (transform.localPosition.x,transform.localPosition.y,  (horizonLine.transform.position.y - (transform.position.y+sortingOffet)) * -1); //set sorting order
    }
}
