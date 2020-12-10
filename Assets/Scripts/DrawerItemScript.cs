using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerItemScript : MonoBehaviour
{
    public int number;
    DrawerScript drawer;

    public void setUp(int n, DrawerScript d){
        number = n;
        drawer = d;
    }

    public void passNumber(){
        Debug.Log("PassNumber");
        drawer.passSelectedItem(number);
    }
}
