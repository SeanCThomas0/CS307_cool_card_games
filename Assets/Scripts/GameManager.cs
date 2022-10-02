using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public List<Card> deck = new List<Card>();
    public Transform[] cardSlots;
    public bool[] availibleCardSlots;

    public void DrawCard() {
        Debug.Log("Draw Card");
        if (deck.Count >= 1) {
            Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availibleCardSlots.Length; i++) {
                if (availibleCardSlots[i] == true) {
                    Debug.Log("Card Availible");
                    randCard.gameObject.SetActive(true);
                    randCard.transform.position = cardSlots[i].position;
                    availibleCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }

    public void Hello() {
        Debug.Log("Hello World");
    }
}
