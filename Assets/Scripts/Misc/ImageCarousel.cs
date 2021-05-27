using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCarousel : MonoBehaviour
{
    int index;
    public UnityEngine.Events.UnityEvent<int> numberSpecificAction;
    public Sprite[] sprites;
    public void increment(int i){
        index += i;
        if(index >= sprites.Length){
            index = 0;
        }else if(index < 0){
            index = sprites.Length - 1;
        }
        numberSpecificAction.Invoke(index);
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}
