using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardsManager : MonoBehaviour
{
    public List<string> cards;
    // Start is called before the first frame update
    void Start()
    {
        cards = new List<string>();
    }

    public void setCards(List<string> c){
        cards = c;
    }

    public void earnCard(string[] c){
        
    }
}
