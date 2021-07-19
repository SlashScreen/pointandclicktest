using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class mystery : MonoBehaviour
{
    public string answer;
    public YarnProgram yarnDialog;
    public string successNode;
    public string failureNode;
    public Dropdown[] drops;
    // Start is called before the first frame update
    private void Start()
    {
        if (GameObject.Find("Player")){
            GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.AddFunction("AddOption",2,AddOption);
        }
        try{
            GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.Add(yarnDialog); //try to add the dialog; if it already exists, just exit the script
        } catch {
            return; //exit
        }
    }

    void AddOption(Yarn.Value[] vals){
        //adds an option to the dropdown
        drops[Mathf.RoundToInt(vals[0].AsNumber)].options.Add(new Dropdown.OptionData(vals[1].AsString));
    }

    string getDropdownText(int which){
        return drops[which].options[drops[which].value].text;
    }

    [YarnCommand("Hide")]
    public void Hide(){
        gameObject.SetActive(false);
    }

    [YarnCommand("Show")]
    public void Show(){
        gameObject.SetActive(true);
    }

    public void recalculate(){
        string res = "It was " + getDropdownText(0) + " in " + getDropdownText(1) + " with " + getDropdownText(2) + " because " + getDropdownText(3);
        
        if (res.ToLower() == answer.ToLower()){
            Debug.Log("you guessed it!");
            if (GameObject.Find("Player")){
                GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.StartDialogue(successNode);
            }
        }else{
            if (GameObject.Find("Player")){ //inefficient but like. nobody playing this is going to be using a pentium 2 and this isn't called every frame. so. 
                GameObject.Find("Player").GetComponent<PlayerMain>().d.dia.StartDialogue(failureNode);
            }
        }
    }
}
