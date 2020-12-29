using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class drawerItem{
    public string name;
    public int id;
    public string iconPath;
}
[System.Serializable]
public class drawerJSON{
    public drawerItem[] items;
}

public class JSONItemParser : MonoBehaviour
{
    public drawerJSON drawer;
    public string JSONPath;
    string JSONs;
    void Start()
    {
        JSONs = File.ReadAllText(JSONPath);
        drawer = JsonUtility.FromJson<drawerJSON>(JSONs);
    }

}
