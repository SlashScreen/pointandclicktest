using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerScript : MonoBehaviour
{
    List<GameObject> items = new List<GameObject>();//item buttons
    public GameObject prefab;
    public float offset = 100f;
    public GameObject dialogueObject;
    Yarn.VariableStorage varstore;

    private void Start()
    {
        varstore = dialogueObject.GetComponent<Yarn.VariableStorage>();
    }
    public void updateInventory(List<int> inventory){ //probably inefficient, TODO: have it remove and add stuff as needed rather than regenerate
        int iter = 1; 
        items = new List<GameObject>();
        foreach (var i in inventory){
            Vector3 place = new Vector3(offset*iter,transform.position.y,0.1f);
            GameObject item = Instantiate(prefab,place,Quaternion.identity);
            item.GetComponent<DrawerItemScript>().setUp(i, gameObject.GetComponent<DrawerScript>());
            item.transform.SetParent(gameObject.transform);
            items.Add(item);
            iter++;
        }
    }

    public void passSelectedItem(int n){
        Debug.Log("passSelectedItem");
        varstore.SetValue("$selectedInventory", new Yarn.Value(n));
        Debug.Log(varstore.GetValue("$selectedInventory"));
    }

}
