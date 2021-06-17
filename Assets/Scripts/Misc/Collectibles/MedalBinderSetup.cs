using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalBinderSetup : MonoBehaviour
{
    public Image icon;
    public TMPro.TextMeshProUGUI TMPtitle;
    public TMPro.TextMeshProUGUI TMPdesc;

    public void Setup(string ico, string title, string desc, bool unlocked){
        TMPtitle.text = title;
        if (unlocked){
            icon.sprite = Resources.Load<Sprite>(ico);
            TMPdesc.text = desc;
        }else{
            TMPdesc.text = "This medal not yet unlocked...";
        }
        
    }
}
