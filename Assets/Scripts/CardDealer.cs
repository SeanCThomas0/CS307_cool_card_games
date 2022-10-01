using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public GameObject[] clubs;
    public GameObject[] hearts;
    public GameObject[] spades;
    public GameObject[] diamonds;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    A random card generator.
    
    provide just count to create a random number of cards with random values and random suits.
    provide all of the parameters to limit randomness.

    count - number of random cards to produce

    minCardNum - the minimum card number to use. the lowest possible value for a card is 1.
    maxCardNum - the maximum card number to use. the highest possible value for a card is 14.
    inclClubs - boolean to specify whether to include club suits
    inclHearts - boolean to specify whether to include heart suit
    inclSpades - boolean to specify whether to include spades suit
    inclDiamonds - boolean to specify whether to include diamonds suit
    */

    public GameObject[] CreateCards(int count)
    {
        GameObject[] randomCards = new GameObject[count];

        for (int i = 0; i < count; i++) {
            // picks a random card number (index)
            int randomNum = Random.Range(0, 13);

            // picks a random suit (index)
            int randomSuit = Random.Range(0, 4);

            // adds card to array
            if (randomSuit == 0) {
                randomCards[i] = clubs[randomNum];
            } else if (randomSuit == 1) {
                randomCards[i] = hearts[randomNum];
            } else if (randomSuit == 2) {
                randomCards[i] = spades[randomNum];
            } else if (randomSuit == 3) {
                randomCards[i] = diamonds[randomNum];
            }
        }

        return randomCards;
    }

    public GameObject[] CreateCards(int count, int minCardNum, int maxCardNum, bool inclClubs, bool inclHearts, bool inclSpades, bool inclDiamonds)
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

        for (int i = 0; i < suits.Length; i++)
        {
            Debug.Log(suits[i]);
        }

        // get random cards
        for (int i = 0; i < count; i++) {
            // picks a random card number (index)
            int randomNum = Random.Range(minCardNum - 1, maxCardNum);

            // picks a random suit (index)
            int randomSuit = Random.Range(0, numOfSuites);

            // adds card to array
            if (suits[randomSuit].Equals("clubs")) {
                randomCards[i] = clubs[randomNum];
            } else if (suits[randomSuit].Equals("hearts")) {
                randomCards[i] = hearts[randomNum];
            } else if (suits[randomSuit].Equals("spades")) {
                randomCards[i] = spades[randomNum];
            } else if (suits[randomSuit].Equals("diamonds")) {
                randomCards[i] = diamonds[randomNum];
            }
        }

        return randomCards;
    }
}
