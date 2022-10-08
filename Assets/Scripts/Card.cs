using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public enum suit {
        CLUBS,
        HEARTS,
        SPADES,
        DIAMONDS,
        JOKER,
        BACK
    }

    private GameObject gameObject;
    private int numValue;
    private suit suitValue;

    public Card(GameObject gameObject, int numValue, suit suitValue) {
        this.gameObject = gameObject;
        this.numValue = numValue;
        this.suitValue = suitValue;
    }

    public GameObject getGameObject() {
        return this.gameObject;
    }

    public int getNumValue() {
        return this.numValue;
    }

    public int getSuitValue() {
        return (int)this.suitValue;
    }
}
