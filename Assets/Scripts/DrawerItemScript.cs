using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerItemScript : MonoBehaviour
{
    public drawerItem s; //class name could be clearer. This is connected to JSONItemParser
    DrawerScript drawer;

    public void setUp(drawerItem item, DrawerScript d){ //sets up the button
        s = item; //set up internal class
        drawer = d; //get reference to parent drawer
    }

    public void passNumber(){ //passes own ID back to drawer. Called by OnClick
        drawer.passSelectedItem(s.id);
    }
}
