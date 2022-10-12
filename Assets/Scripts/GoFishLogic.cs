using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFishLogic : MonoBehaviour
{
    public GameObject Square;
    CardDealer cardDealer;

    // Start is called before the first frame update
    void Start()
    {
        cardDealer = Square.GetComponent<CardDealer>();

        // GameObject jokerCard = cardDealer.jokerCard();
        List<GameObject> someCards = cardDealer.RandomCards(52, 1, 1, true, false, false, false);

        for (int i = 0; i < someCards.Count; i++) {
            GameObject cardOne = Instantiate(someCards[i], new Vector3(0,0,0), Quaternion.identity);
        }

        // GameObject joker = Instantiate(jokerCard, new Vector3(0,0,0), Quaternion.identity);

        // GameObject[] shuffledCards = cardDealer.ShuffleCards(someCards);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
