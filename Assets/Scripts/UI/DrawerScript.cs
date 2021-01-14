using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerScript : MonoBehaviour
{
    List<GameObject> items = new List<GameObject>();//item buttons
    public GameObject prefab;
    public float offset = 100f;
    public float shift = -850f;
    public GameObject dialogueObject;
    Yarn.VariableStorage varstore;
    
    private void Start()
    {
        varstore = dialogueObject.GetComponent<Yarn.VariableStorage>();
        
    }
    public void updateInventory(List<drawerItem> inventory){ //probably inefficient, TODO: have it remove and add stuff as needed rather than regenerate
        //Delete all
        foreach(var item in items){
            GameObject.Destroy(item); //destroy item
        }
        items.Clear();
        //Create new
        int iter = 1; 
        items = new List<GameObject>();
        foreach (var i in inventory){
            Vector3 place = new Vector3((offset*iter),-60,0.1f); //created the spot for the button to be placed based on which it is in the index (iter)
            GameObject item = Instantiate(prefab,place,Quaternion.identity); //refers to gameobject
            item.GetComponent<DrawerItemScript>().setUp(i, gameObject.GetComponent<DrawerScript>()); //run setup routine
            item.transform.SetParent(transform.Find("Viewport").Find("DrawerItems").transform); //set parent
            item.transform.localPosition = place;
            items.Add(item); //add to list
            iter++; //index increment
        }
    }

    public void passSelectedItem(int n){
        varstore.SetValue("$selectedInventory", new Yarn.Value(n));
    }

}
