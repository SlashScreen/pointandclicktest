using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class KeyPadEnter : MonoBehaviour
{
    public void EnterNumber(int n){
        string text = GetComponent<TMPro.TextMeshProUGUI>().text;
        int enteredLength = text.Length - text.Count(f => f == '-'); //counts fow many numbers have been entered

        if (enteredLength == text.Length){
            return;
        }
        StringBuilder sb = new StringBuilder(text);
        sb[(enteredLength)] = char.Parse(n.ToString());
        GetComponent<TMPro.TextMeshProUGUI>().text = sb.ToString();
    }
}
