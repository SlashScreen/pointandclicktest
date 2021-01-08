using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that works as a door, to be applied to an interactive object and connected to the custom event that is invoked via yarn
public class DoorScript : MonoBehaviour
{
    public string room;
    public int spawnpoint = 0;
    public scenemanager sm;
    public void OpenDoor(){
        Debug.Log("Oepning door to " + room);
        sm.loadscene(room,spawnpoint); //load scene "room" at spawnpoint "spawnpoint"
    }
}
