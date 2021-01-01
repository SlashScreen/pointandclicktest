using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class MainMenuLoadDropdown : MonoBehaviour
{
    TMPro.TMP_Dropdown dropdown;
    private void OnEnable()
    {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/saves"); //get all items in save directory
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray(); //sort by creation time
        Array.Reverse(files); //reverse array to make the most recent on top

        List<TMPro.TMP_Dropdown.OptionData> items = new List<TMPro.TMP_Dropdown.OptionData>();
        dropdown.ClearOptions();

        foreach (FileInfo file in files){
            TMPro.TMP_Dropdown.OptionData item = new TMPro.TMP_Dropdown.OptionData();
            item.text = file.Name;
            items.Add(item);
        }

        dropdown.AddOptions(items);
    }

    public void PassItThrough(){
        GameObject.Find("Menu").GetComponent<MainMenu>().completeLoad(dropdown.options[dropdown.value].text);
    }
}
