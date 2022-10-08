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
    public GameObject[] blue;
    public GameObject[] green;
    public GameObject[] red;

    public enum backColor {
        BLUE,
        GREEN,
        RED
    }

    public enum backDesign {
        PLAIN,
        OUTLINE,
        OUTLINE_PATTERN,
        OUTLINE_SIMPLE_PATTERN,
        PATTERN
    }

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
    public List<Card> StandardDeck() {
        List<Card> standardDeck = new List<Card>();

        // loops through each card, returning a standard 52 card deck
        for (int i = 0; i < clubs.Length; i++) {
            standardDeck.Add(new Card(clubs[i], 0, Card.suit.CLUBS));
        }

        for (int i = 0; i < hearts.Length; i++) {
            standardDeck.Add(new Card(hearts[i], 0, Card.suit.HEARTS));
        }

        for (int i = 0; i < spades.Length; i++) {
            standardDeck.Add(new Card(spades[i], 0, Card.suit.SPADES));
        }

        for (int i = 0; i < diamonds.Length; i++) {
            standardDeck.Add(new Card(diamonds[i], 0, Card.suit.DIAMONDS));
        }

        return standardDeck;
    }

    /*
    returns the joker card
    */
    public Card JokerCard() {
        // returns joker card
        return new Card(joker, 0, Card.suit.JOKER);
    }

    /*
    returns a list of specified card backs

    count - number of cards to return
    color - color of card back (use by stating "CardDealer.backColor.COLOR" where COLOR is BLUE, GREEN, or RED)
    design - design of card back (use by stating "CardDealer.backDesign.DESIGN" where DESIGN is PLAIN, OUTLINE, OUTLINE_PATTERN, OUTLINE_SIMPLE_PATTERN, or PATTERN)
    */
    public List<Card> CardBacks(int count, backColor color, backDesign design) {
        List<Card> cardBacks = new List<Card>();

        for (int i = 0; i < count; i++) {
            if (color == backColor.BLUE) {
                switch (design) {
                    case backDesign.PLAIN:
                        cardBacks.Add(new Card(blue[0], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE:
                        cardBacks.Add(new Card(blue[1], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE_PATTERN:
                        cardBacks.Add(new Card(blue[2], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE_SIMPLE_PATTERN:
                        cardBacks.Add(new Card(blue[3], 0, Card.suit.BACK));
                        break;
                    case backDesign.PATTERN:
                        cardBacks.Add(new Card(blue[4], 0, Card.suit.BACK));
                        break;
                }
               
            } else if (color == backColor.GREEN) {
                switch (design) {
                    case backDesign.PLAIN:
                        cardBacks.Add(new Card(green[0], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE:
                        cardBacks.Add(new Card(green[1], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE_PATTERN:
                        cardBacks.Add(new Card(green[2], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE_SIMPLE_PATTERN:
                        cardBacks.Add(new Card(green[3], 0, Card.suit.BACK));
                        break;
                    case backDesign.PATTERN:
                        cardBacks.Add(new Card(green[4], 0, Card.suit.BACK));
                        break;
                }
            } else if (color == backColor.RED) {
                switch (design) {
                    case backDesign.PLAIN:
                        cardBacks.Add(new Card(red[0], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE:
                        cardBacks.Add(new Card(red[1], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE_PATTERN:
                        cardBacks.Add(new Card(red[2], 0, Card.suit.BACK));
                        break;
                    case backDesign.OUTLINE_SIMPLE_PATTERN:
                        cardBacks.Add(new Card(red[3], 0, Card.suit.BACK));
                        break;
                    case backDesign.PATTERN:
                        cardBacks.Add(new Card(red[4], 0, Card.suit.BACK));
                        break;
                }
            }
        }

        return cardBacks;
    }

    /*
    A random card generator.
    
    provide just count to create a random number of cards with random values and random suits.
    provide all of the parameters to limit randomness.

    count - number of random cards to produce
    */
    public List<Card> RandomCards(int count)
    {
        if (count <= 0) {
            Debug.Log("\"count\" must be 1 or greater.");
            return null;
        }

        List<Card> randomCards = new List<Card>();

        // ensures number of returned cards is the requested size
        while(randomCards.Count < count) {
            // picks a random card number (index)
            int randomNum = UnityEngine.Random.Range(0, 13);

            // picks a random suit (index)
            int randomSuit = UnityEngine.Random.Range(0, 4);

            // adds card to array
            switch (randomSuit)
            {
                case 0:
                    randomCards.Add(new Card(clubs[randomNum], randomNum + 1, Card.suit.CLUBS));
                    break;
                case 1:
                    randomCards.Add(new Card(hearts[randomNum], randomNum + 1, Card.suit.HEARTS));
                    break;
                case 2:
                    randomCards.Add(new Card(spades[randomNum], randomNum + 1, Card.suit.SPADES));
                    break;
                case 3:
                    randomCards.Add(new Card(diamonds[randomNum], randomNum + 1, Card.suit.DIAMONDS));
                    break;
                default:
                    Debug.Log("Error...added joker card as a joke. Not really as a joke, but at least this will avoid an infinite loop in the case this error occurs, although it never should.");
                    randomCards.Add(new Card(joker, 0, Card.suit.JOKER));
                    break;
            }
        }

        return randomCards;
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
    public List<Card> RandomCards(int count, int minCardNum, int maxCardNum, bool inclClubs, bool inclHearts, bool inclSpades, bool inclDiamonds)
    {
        if (count <= 0) {
            Debug.Log("\"count\" must be 1 or greater.");
            return null;
        }

        if (minCardNum < 1 || minCardNum > 13 || maxCardNum < 1 || maxCardNum > 13) {
            Debug.Log("minCardNum and maxCardNum must be greater than or equal to 1 and less than or equal to 13.");
            return null;
        }

        List<Card> randomCards = new List<Card>();

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

            // add cards to array
            switch (randomSuit)
            {
                case 0:
                    randomCards.Add(new Card(clubs[randomNum], randomNum + 1, Card.suit.CLUBS));
                    break;
                case 1:
                    randomCards.Add(new Card(hearts[randomNum], randomNum + 1, Card.suit.HEARTS));
                    break;
                case 2:
                    randomCards.Add(new Card(spades[randomNum], randomNum + 1, Card.suit.SPADES));
                    break;
                case 3:
                    randomCards.Add(new Card(diamonds[randomNum], randomNum + 1, Card.suit.DIAMONDS));
                    break;
                default:
                    Debug.Log("Error...added joker card as a joke. Not really as a joke, but at least this will avoid an infinite loop in the case this error occurs, although it never should.");
                    randomCards.Add(new Card(joker, 0, Card.suit.JOKER));
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
    public List<Card> ShuffleCards(List<Card> toShuffle) {
        if (toShuffle.Count <= 1 || toShuffle == null) {
            Debug.Log("\"toShuffle\" must be of length 2 or greater in order to be shuffled.");
        }

        // for (int i = 0; i < toShuffle.Length; i++) {
        //     Debug.Log("old: " + toShuffle[i]);
        // }

        int currentIndex = 0;
        List<Card> shuffled = new List<Card>();
        bool[] shuffleStatus = new bool[toShuffle.Count];

        while (currentIndex < toShuffle.Count) {
            int randomNewIndex = UnityEngine.Random.Range(0, toShuffle.Count);

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
