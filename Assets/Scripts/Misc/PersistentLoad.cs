using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentLoad : MonoBehaviour
{
    public scenemanager sm;
    void Start(){
        //TODO Loading support
        sm.loadscene("Testrange");
    }

}
