using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayBass : MonoBehaviour
{
    public GuitarString[] strings = new GuitarString[4];
    public AudioPlayer audiop;
    public bool active;
    public Sprite[] sprites = new Sprite[2];

    public void OnBass1(InputValue input){
        Play(0,"SFX/bass1");
    }
    public void OnBass2(InputValue input){
        Play(1,"SFX/bass2");
    }
    public void OnBass3(InputValue input){
        Play(2,"SFX/bass3");
    }
    public void OnBass4(InputValue input){
        Play(3,"SFX/bass4_2");
    }

    public void setActive(bool t){
        active = t;
    }

    void Play(int i, string s){
        Debug.Log(active);
        if(active){
            Debug.Log(s);
            strings[i].Twang();
            string[] sfx = new string[1];
            sfx[0] = s;
            audiop.startSFX(sfx);
            transform.position = new Vector3 (transform.position.x,strings[i].transform.position.y,0);
            StartCoroutine(Strum());
        }
    }

    IEnumerator Strum(){ //changes the hand
        GetComponent<SpriteRenderer>().sprite = sprites[1];
        yield return new WaitForSeconds(.1f);
        GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
}
