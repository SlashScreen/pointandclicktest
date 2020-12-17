using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Yarn.Unity;


public class DrawerItemScript : MonoBehaviour
{
    public drawerItem s; //class name could be clearer. This is connected to JSONItemParser
    public Image image;
    public string useNode;
    DrawerScript drawer;
    GameObject dia;

    public void setUp(drawerItem item, DrawerScript d){ //sets up the button
        s = item; //set up internal class
        drawer = d; //get reference to parent drawer
        Sprite sp = Resources.Load<Sprite>(s.iconPath); //local could be an issue
        image.sprite = sp;
        dia = GameObject.Find("Dialogue");
    }

    public void passNumber(){ //passes own ID back to drawer. Called by OnClick
        drawer.passSelectedItem(s.id);
    }

    public void clickBehavior(){
        if (dia.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber != 0){

            dia.GetComponent<Yarn.VariableStorage>().SetValue("$clickedID", new Yarn.Value(s.id)); //Pass ID to yarn for comparison

            dia.GetComponent<DialogControllerComponent>().dia.StartDialogue(useNode); //Potential problem: player can move around during this conversation
            
        }else{
            passNumber();
        }
    }
}
