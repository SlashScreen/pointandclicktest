using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOpenDialogBox : MonoBehaviour
{
    public GameObject DiaBox;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Yarn.Unity.DialogueRunner>().AddCommandHandler("HideDialog",HideDialogue);
        GetComponent<Yarn.Unity.DialogueRunner>().AddCommandHandler("ShowDialog",ShowDialogue);
    }

    public void HideDialogue(string[] a){
        DiaBox.SetActive(false);
    }

    public void ShowDialogue(string[] a){
        DiaBox.SetActive(true);
    }
}
