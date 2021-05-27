using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IfThisThenDO : MonoBehaviour
{
    //this script is a nightmare, sorry
    //so how to use this:
    //Link this to a unity event invoke
    //select correct function for data type
    //set Arg2 to be the comparison
    //when it is true, it invokes thenDo
    //if not, it invokes elseDo

    public UnityEngine.Events.UnityEvent thenDo;
    public UnityEngine.Events.UnityEvent elseDo;
    public string Arg2;
    public void IsEquals<T>(T arg1, T arg2){
        if (arg1.Equals(arg2)){
            thenDo.Invoke();
        }else{
            elseDo.Invoke();
        }
        
    }

    public void IsEqualsInt(int arg1){
        IsEquals<int>(arg1,int.Parse(Arg2));
    }
    public void IsEqualsString(string arg1){
        IsEquals<string>(arg1,Arg2);
    }
    public void IsEqualsTextString(TextMeshProUGUI arg1){
        //Debug.Log(arg1.text);
        IsEquals<string>(arg1.text.Trim(),Arg2);
    }
}
