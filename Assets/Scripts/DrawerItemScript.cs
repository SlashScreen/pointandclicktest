using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DrawerItemScript : MonoBehaviour
{
    public drawerItem s; //class name could be clearer. This is connected to JSONItemParser
    public Image image;
    DrawerScript drawer;

    public void setUp(drawerItem item, DrawerScript d){ //sets up the button
        s = item; //set up internal class
        drawer = d; //get reference to parent drawer
        Sprite sp = Resources.Load<Sprite>(s.iconPath); //local could be an issue
        image.sprite = sp;
    }

    public void passNumber(){ //passes own ID back to drawer. Called by OnClick
        drawer.passSelectedItem(s.id);
    }
}
