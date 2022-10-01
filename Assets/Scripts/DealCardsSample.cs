using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCardsSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] suits = {"clubs"};
        string[] cards = CreateCards(3);
        Debug.Log(cards[0] + ", " + cards[1] + ", " + cards[2]);
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
    suits - specify the suits to use. options include "clubs", "diamonds", "hearts", "spades"

    */

    string[] CreateCards(int count)
    {
        // returns null if any arguments are invalid
        if (count <= 0) {
            return null;
        }

        // determines whether to pick from all suits or only specified suits
        string[] suitsToChoose = {"clubs", "diamonds", "hearts", "spades"};

        // generate 'count' number of random cards
        string[] randomCards = new string[count];
        for (int i = 0; i < count; i++) {
            // picks a random card suit
            int randomSuit = Random.Range(0, suitsToChoose.Length);

            // picks a random card number
            int randomNum = Random.Range(2, 15);
        
            // adds to array of randomly chosen cards
            randomCards[i] = randomNum + "_" + suitsToChoose[randomSuit];
        }

        return randomCards;
    }

    string[] CreateCards(int count, int minCardNum, int maxCardNum, string[] suits)
    {
        // returns null if any arguments are invalid
        if (count <= 0 || minCardNum < 0 || maxCardNum > 14 || suits == null || suits.Length > 4) {
            return null;
        }

        // generate 'count' number of random cards
        string[] randomCards = new string[count];
        for (int i = 0; i < count; i++) {
            // picks a random card suit
            int randomSuit = Random.Range(0, suits.Length);

            int randomNum = Random.Range(minCardNum, maxCardNum + 1);
        
            // adds to array of randomly chosen cards
            randomCards[i] = randomNum + "_" + suits[randomSuit];
        }

        return randomCards;
    }
}
