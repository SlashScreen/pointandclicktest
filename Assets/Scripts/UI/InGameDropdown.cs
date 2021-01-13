using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;

public class InGameDropdown : MonoBehaviour
{
    public PlayerMain player;
    public InputField input;
    public GameObject panel;
    public Dropdown dropdown;
    public bool hidden = true;
    public void save(){ //called by button
        DateTime dt = DateTime.Now; //get date time
        string defaultText = dt.ToString("yyyy-MM-dd-HH-mm-ss"); //set default text to formatted date
        input.gameObject.SetActive(true); //show input field
        input.text = defaultText; //set text to date 
        hidden = false;
    }

    public void completeSave(){ //called once text inputted
        GetComponent<SaveManager>().Save(input.text,player); //call save function
        input.gameObject.SetActive(false); //get rid of input field 
        hidden = true;
    }

    public void beginLoad(){ //called by button
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/saves"); //get all items in save directory
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray(); //sort by creation time
        Array.Reverse(files); //reverse array to make the most recent on top

        List<Dropdown.OptionData> items = new List<Dropdown.OptionData>();
        dropdown.ClearOptions();

        foreach (FileInfo file in files){
            Dropdown.OptionData item = new Dropdown.OptionData();
            item.text = file.Name;
            items.Add(item);
        }

        dropdown.AddOptions(items);
        panel.SetActive(true);
        hidden = false;
    }

    public void Cancel(){ //half assed way to cancel when clicked out of menu
        panel.SetActive(false);
        input.gameObject.SetActive(false);
        hidden = true;
    }

    public void completeLoad(){ //called once savegame selected
        panel.SetActive(false);
        hidden = true;
        GetComponent<SaveManager>().Load(dropdown.options[dropdown.value].text, player); //Load from text of current selected option
    }
}
