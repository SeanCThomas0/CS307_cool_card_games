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
            if (randomSuit == 0) {
                if (randomCards.Contains(clubs[randomNum])) {
                    randomCards.Add(clubs[randomNum]);
                }
            } else if (randomSuit == 1) {
                if (randomCards.Contains(hearts[randomNum])) {
                    randomCards.Add(hearts[randomNum]);
                }
            } else if (randomSuit == 2) {
                if (randomCards.Contains(spades[randomNum])) {
                    randomCards.Add(spades[randomNum]);
                }
            } else if (randomSuit == 3) {
                if (randomCards.Contains(diamonds[randomNum])) {
                    randomCards.Add(diamonds[randomNum]);
                }
            }
        }

        return randomCards;
    }

    public GameObject[] RandomCards(int count, int minCardNum, int maxCardNum, bool inclClubs, bool inclHearts, bool inclSpades, bool inclDiamonds)
    {
        GameObject[] randomCards = new GameObject[count];

        // determine how big to make array to make randomness easier later
        int numOfSuites = 0;

        if (inclClubs) {
            numOfSuites++;
        }
        
        if (inclHearts) {
            numOfSuites++;
        }
        
        if (inclSpades) {
            numOfSuites++;
        }

        if (inclDiamonds) {
            numOfSuites++;
        }

        // add suits to array to enable randomness later
        string[] suits = new string[numOfSuites];
        int currentIndex = 0;

        if (inclClubs) {
            suits[currentIndex] = "clubs";
            currentIndex++;
        }
        
        if (inclHearts) {
            suits[currentIndex] = "hearts";
            currentIndex++;
        }
        
        if (inclSpades) {
            suits[currentIndex] = "spades";
            currentIndex++;
        }

        if (inclDiamonds) {
            suits[currentIndex] = "diamonds";
        }

        // get random cards
        int numberReturned = 0;
        while (numberReturned < count) {
            // picks a random card number (index)
            int randomNum = UnityEngine.Random.Range(minCardNum - 1, maxCardNum);

            // picks a random suit (index)
            int randomSuit = UnityEngine.Random.Range(0, numOfSuites);

            // adds card to array
            if (suits[randomSuit].Equals("clubs")) {
                if (!Array.Exists(randomCards, element => element == clubs[randomNum])) {
                    randomCards[numberReturned] = clubs[randomNum];
                    numberReturned++;
                }
            } else if (suits[randomSuit].Equals("hearts")) {
                if (!Array.Exists(randomCards, element => element == hearts[randomNum])) {
                    randomCards[numberReturned] = hearts[randomNum];
                    numberReturned++;
                }
            } else if (suits[randomSuit].Equals("spades")) {
                if (!Array.Exists(randomCards, element => element == spades[randomNum])) {
                    randomCards[numberReturned] = spades[randomNum];
                    numberReturned++;
                }
            } else if (suits[randomSuit].Equals("diamonds")) {
                if (!Array.Exists(randomCards, element => element == diamonds[randomNum])) {
                    randomCards[numberReturned] = diamonds[randomNum];
                    numberReturned++;
                }
            }
        }

        return randomCards;
    }

    /*
    Card shuffler.

    Given an array of GameObject cards, the function will randomly shuffle the cards and return a new, shuffled array containing the same GameObjects.

    toShuffle - array of GameObject cards to shuffle
    */
    public GameObject[] ShuffleCards(GameObject[] toShuffle) {
        // for (int i = 0; i < toShuffle.Length; i++) {
        //     Debug.Log("old: " + toShuffle[i]);
        // }

        int currentIndex = 0;
        GameObject[] shuffled = new GameObject[toShuffle.Length];
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
