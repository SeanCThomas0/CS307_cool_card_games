using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public GameObject[] clubs;
    public GameObject[] hearts;
    public GameObject[] spades;
    public GameObject[] diamonds;
    public GameObject joker;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    returns a standard deck of 52 cards
    */
    public List<GameObject> StandardDeck() {
        List<GameObject> standardDeck = new List<GameObject>();

        for (int i = 0; i < clubs.Length; i++) {
            standardDeck.Add(clubs[i]);
        }

        for (int i = 0; i < hearts.Length; i++) {
            standardDeck.Add(hearts[i]);
        }

        for (int i = 0; i < spades.Length; i++) {
            standardDeck.Add(spades[i]);
        }

        for (int i = 0; i < diamonds.Length; i++) {
            standardDeck.Add(diamonds[i]);
        }

        return standardDeck;
    }

    /*
    returns the joker card
    */
    public GameObject JokerCard() {
        return joker;
    }

    /*
    A random card generator.
    
    provide just count to create a random number of cards with random values and random suits.
    provide all of the parameters to limit randomness.

    count - number of random cards to produce

    minCardNum - the minimum card number to use. the lowest possible value for a card is 1.
    maxCardNum - the maximum card number to use. the highest possible value for a card is 13.
    inclClubs - boolean to specify whether to include club suits
    inclHearts - boolean to specify whether to include heart suit
    inclSpades - boolean to specify whether to include spades suit
    inclDiamonds - boolean to specify whether to include diamonds suit
    */

    public List<GameObject> RandomCards(int count)
    {
        List<GameObject> randomCards = new List<GameObject>();

        while(randomCards.Count < count) {
            // picks a random card number (index)
            int randomNum = UnityEngine.Random.Range(0, 13);

            // picks a random suit (index)
            int randomSuit = UnityEngine.Random.Range(0, 4);

            // adds card to array
            switch (randomSuit)
            {
                case 0:
                    randomCards.Add(clubs[randomNum]);
                    break;
                case 1:
                    randomCards.Add(hearts[randomNum]);
                    break;
                case 2:
                    randomCards.Add(spades[randomNum]);
                    break;
                case 3:
                    randomCards.Add(diamonds[randomNum]);
                    break;
                default:
                    Debug.Log("Error...added joker card as a joke. Not really as a joke, but at least this will avoid an infinite loop in the case this error occurs, although it never should.");
                    randomCards.Add(joker);
                    break;
            }
        }

        return randomCards;
    }

    public List<GameObject> RandomCards(int count, int minCardNum, int maxCardNum, bool inclClubs, bool inclHearts, bool inclSpades, bool inclDiamonds)
    {
        List<GameObject> randomCards = new List<GameObject>();

        // add suits to array to enable randomness later
        List<string> suits = new List<string>();

        int numOfSuits = 0;
        if (inclClubs) {
            suits.Add("clubs");
            numOfSuits++;
        }
        
        if (inclHearts) {
            suits.Add("hearts");
            numOfSuits++;
        }
        
        if (inclSpades) {
            suits.Add("spades");
            numOfSuits++;
        }

        if (inclDiamonds) {
            suits.Add("diamonds");
            numOfSuits++;
        }

        // get random cards
        while (randomCards.Count < count) {
            // picks a random card number (index)
            int randomNum = UnityEngine.Random.Range(minCardNum - 1, maxCardNum);

            // picks a random suit (index)
            int randomSuit = UnityEngine.Random.Range(0, numOfSuits);

            switch (randomSuit)
            {
                case 0:
                    randomCards.Add(clubs[randomNum]);
                    break;
                case 1:
                    randomCards.Add(hearts[randomNum]);
                    break;
                case 2:
                    randomCards.Add(spades[randomNum]);
                    break;
                case 3:
                    randomCards.Add(diamonds[randomNum]);
                    break;
                default:
                    Debug.Log("Error...added joker card as a joke. Not really as a joke, but at least this will avoid an infinite loop in the case this error occurs, although it never should.");
                    randomCards.Add(joker);
                    break;
            }
        }

        return randomCards;
    }

    /*
    Card shuffler.

    Given an array of GameObject cards, the function will randomly shuffle the cards and return a new, shuffled array containing the same GameObjects.

    toShuffle - array of GameObject cards to shuffle
    */
    public List<GameObject> ShuffleCards(GameObject[] toShuffle) {
        // for (int i = 0; i < toShuffle.Length; i++) {
        //     Debug.Log("old: " + toShuffle[i]);
        // }

        int currentIndex = 0;
        List<GameObject> shuffled = new List<GameObject>();
        bool[] shuffleStatus = new bool[toShuffle.Length];

        while (currentIndex < toShuffle.Length) {
            int randomNewIndex = UnityEngine.Random.Range(0, toShuffle.Length);

            if (shuffleStatus[randomNewIndex] == false) {
                shuffled[randomNewIndex] = toShuffle[currentIndex];

                shuffleStatus[randomNewIndex] = true;

                currentIndex++;
            }
        }

        // for (int i = 0; i < shuffled.Length; i++) {
        //     Debug.Log("new:" + shuffled[i]);
        // }

        return shuffled;
    }
}
