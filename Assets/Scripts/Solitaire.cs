using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    public GameObject SolitaireGame;
    CardDealer cardDealer;
    public GameObject cardPrefab;

    public GameObject[] deck;
    // Start is called before the first frame update
    void Start()
    {
        cardDealer = SolitaireGame.GetComponent<CardDealer>();
        deck = cardDealer.RandomCards(52);
        SolitaireDeal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SolitaireDeal() {
        for (int i = 0; i < deck.Length; i++) {
            GameObject newCard = Instantiate(deck[i], transform.position, Quaternion.identity);
            
        }
    }
}
