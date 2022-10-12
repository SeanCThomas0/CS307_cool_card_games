using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public GameObject card;
    public Sprite[] clubs;
    public Sprite[] hearts;
    public Sprite[] spades;
    public Sprite[] diamonds;
    public Sprite joker;
    public Sprite[] blue;
    public Sprite[] green;
    public Sprite[] red;

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
    public List<GameObject> StandardDeck() {
        List<GameObject> standardDeck = new List<GameObject>();

        // loops through each card, returning a standard 52 card deck
        for (int i = 0; i < clubs.Length; i++) {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<SpriteRenderer>().sprite = clubs[i];
            newCard.GetComponent<Card>().numValue = i + 1;
            newCard.GetComponent<Card>().suitValue = Card.suit.CLUBS;

            standardDeck.Add(newCard);
        }

        for (int i = 0; i < hearts.Length; i++) {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<SpriteRenderer>().sprite = hearts[i];
            newCard.GetComponent<Card>().numValue = i + 1;
            newCard.GetComponent<Card>().suitValue = Card.suit.HEARTS;

            standardDeck.Add(newCard);
        }

        for (int i = 0; i < spades.Length; i++) {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<SpriteRenderer>().sprite = spades[i];
            newCard.GetComponent<Card>().numValue = i + 1;
            newCard.GetComponent<Card>().suitValue = Card.suit.SPADES;

            standardDeck.Add(newCard);
        }

        for (int i = 0; i < diamonds.Length; i++) {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<SpriteRenderer>().sprite = diamonds[i];
            newCard.GetComponent<Card>().numValue = i + 1;
            newCard.GetComponent<Card>().suitValue = Card.suit.DIAMONDS;

            standardDeck.Add(newCard);
        }

        return standardDeck;
    }

    /*
    returns the joker card
    */
    public GameObject JokerCard() {
        // returns joker card
        GameObject newCard = Instantiate(card) as GameObject;
        newCard.SetActive(false);

        newCard.GetComponent<SpriteRenderer>().sprite = joker;
        newCard.GetComponent<Card>().numValue = 0;
        newCard.GetComponent<Card>().suitValue = Card.suit.JOKER;
        return newCard;
    }

    /*
    returns a list of specified card backs

    count - number of cards to return
    color - color of card back (use by stating "CardDealer.backColor.COLOR" where COLOR is BLUE, GREEN, or RED)
    design - design of card back (use by stating "CardDealer.backDesign.DESIGN" where DESIGN is PLAIN, OUTLINE, OUTLINE_PATTERN, OUTLINE_SIMPLE_PATTERN, or PATTERN)
    */
    public List<GameObject> CardBacks(int count, backColor color, backDesign design) {
        List<GameObject> cardBacks = new List<GameObject>();

        int index = -1;
        switch (design)
        {
            case backDesign.PLAIN:
                index = 0;
                break;
            case backDesign.OUTLINE:
                index = 1;
                break;
            case backDesign.OUTLINE_PATTERN:
                index = 2;
                break;
            case backDesign.OUTLINE_SIMPLE_PATTERN:
                index = 3;
                break;
            case backDesign.PATTERN:
                index = 4;
                break;
        }
        
        if (color == backColor.BLUE) {
            for (int i = 0; i < count; i++) {
                GameObject newCard = Instantiate(card) as GameObject;
                newCard.SetActive(false);

                newCard.GetComponent<SpriteRenderer>().sprite = blue[index];
                newCard.GetComponent<Card>().numValue = 0;
                newCard.GetComponent<Card>().suitValue = Card.suit.BACK;

                cardBacks.Add(newCard);
            }
        } else if (color == backColor.GREEN) {
            for (int i = 0; i < count; i++) {
                GameObject newCard = Instantiate(card) as GameObject;
                newCard.SetActive(false);

                newCard.GetComponent<SpriteRenderer>().sprite = green[index];
                newCard.GetComponent<Card>().numValue = 0;
                newCard.GetComponent<Card>().suitValue = Card.suit.BACK;

                cardBacks.Add(newCard);
            }
        } else if (color == backColor.RED) {
            for (int i = 0; i < count; i++) {
                GameObject newCard = Instantiate(card) as GameObject;
                newCard.SetActive(false);

                newCard.GetComponent<SpriteRenderer>().sprite = red[index];
                newCard.GetComponent<Card>().numValue = 0;
                newCard.GetComponent<Card>().suitValue = Card.suit.BACK;

                cardBacks.Add(newCard);
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
    public List<GameObject> RandomCards(int count)
    {
        if (count <= 0) {
            Debug.Log("\"count\" must be 1 or greater.");
            return null;
        }

        List<GameObject> randomCards = new List<GameObject>();

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
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.CLUBS)) {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = clubs[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.CLUBS;

                        randomCards.Add(newCard);
                    }
                    break;
                case 1:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.HEARTS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = hearts[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.HEARTS;

                        randomCards.Add(newCard);
                    }
                    break;
                case 2:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.SPADES))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = spades[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.SPADES;

                        randomCards.Add(newCard);
                    }
                    break;
                case 3:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.DIAMONDS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = diamonds[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.DIAMONDS;

                        randomCards.Add(newCard);
                    }
                    break;
                default:
                    Debug.Log("Error...this should never be reached.");
                    return null;
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
    public List<GameObject> RandomCards(int count, int minCardNum, int maxCardNum, bool inclClubs, bool inclHearts, bool inclSpades, bool inclDiamonds)
    {
        if (count <= 0) {
            Debug.Log("\"count\" must be 1 or greater.");
            return null;
        }

        if (minCardNum < 1 || minCardNum > 13 || maxCardNum < 1 || maxCardNum > 13) {
            Debug.Log("minCardNum and maxCardNum must be greater than or equal to 1 and less than or equal to 13.");
            return null;
        }

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

            // add cards to array
            switch (randomSuit)
            {
                case 0:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.CLUBS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = clubs[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.CLUBS;

                        randomCards.Add(newCard);
                    }
                    break;
                case 1:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.HEARTS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = hearts[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.HEARTS;

                        randomCards.Add(newCard);
                    }
                    break;
                case 2:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.SPADES))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = spades[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.SPADES;

                        randomCards.Add(newCard);
                    }
                    break;
                case 3:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.DIAMONDS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<SpriteRenderer>().sprite = diamonds[randomNum];
                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.DIAMONDS;

                        randomCards.Add(newCard);
                    }
                    break;
                default:
                    Debug.Log("Error...this should also never be reached.");
                    return null;
            }
        }

        return randomCards;
    }

    /*
    Card shuffler.

    Given an array of GameObject cards, the function will randomly shuffle the cards and return a new, shuffled array containing the same GameObjects.

    toShuffle - array of GameObject cards to shuffle
    */
    public List<GameObject> ShuffleCards(List<GameObject> toShuffle) {
        if (toShuffle.Count <= 1 || toShuffle == null) {
            Debug.Log("\"toShuffle\" must be of length 2 or greater in order to be shuffled.");
        }

        List<GameObject> shuffled = new List<GameObject>();

        while (toShuffle.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, toShuffle.Count);

            shuffled.Add(toShuffle[randomIndex]);
            toShuffle.RemoveAt(randomIndex);

        }

        return shuffled;
    }

    /*
    to be used instead of List.Contains
    
    list - the list to search for card in
    card - the card to check for in list
    */
    public bool ContainsCard(List<GameObject> list, int numValue, Card.suit suitValue) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i].GetComponent<Card>().numValue == numValue
            && list[i].GetComponent<Card>().suitValue == suitValue) {
                return true;
            }
        }

        return false;
    }

    public void ToggleView(GameObject card, bool toggle) {
        card.SetActive(toggle);
    }

    public void ChangeSprite() {
        
    }
}
