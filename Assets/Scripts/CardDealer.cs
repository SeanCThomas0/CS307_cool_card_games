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

    public Card.cardSize cardSize;

    void OnEnable() {
        cardSize = (Card.cardSize) PlayerPrefs.GetInt("cardSize");
    }

    /*
    returns a standard deck of 52 cards
    */
    public List<GameObject> StandardDeck()
    {
        List<GameObject> standardDeck = new List<GameObject>();

        // loops through each card, returning a standard 52 card deck
        for (int i = 0; i < clubs.Length; i++)
        {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<Card>().numValue = i + 1;

            switch(i + 1) {
                case 1:
                    newCard.GetComponent<Card>().numValueString = "ace";
                    break;
                case 11:
                    newCard.GetComponent<Card>().numValueString = "jack";
                    break;
                case 12:
                    newCard.GetComponent<Card>().numValueString = "queen";
                    break;
                case 13:
                    newCard.GetComponent<Card>().numValueString = "king";
                    break;
                default:
                    newCard.GetComponent<Card>().numValueString = "" + (i + 1);
                    break;
            }

            newCard.GetComponent<Card>().suitValue = Card.suit.CLUBS;
            newCard.GetComponent<Card>().suitValueString = "clubs";

            SetSprite(newCard);

            standardDeck.Add(newCard);
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<Card>().numValue = i + 1;

            switch(i + 1) {
                case 1:
                    newCard.GetComponent<Card>().numValueString = "ace";
                    break;
                case 11:
                    newCard.GetComponent<Card>().numValueString = "jack";
                    break;
                case 12:
                    newCard.GetComponent<Card>().numValueString = "queen";
                    break;
                case 13:
                    newCard.GetComponent<Card>().numValueString = "king";
                    break;
                default:
                    newCard.GetComponent<Card>().numValueString = "" + (i + 1);
                    break;
            }

            newCard.GetComponent<Card>().suitValue = Card.suit.HEARTS;
            newCard.GetComponent<Card>().suitValueString = "hearts";

            SetSprite(newCard);

            standardDeck.Add(newCard);
        }

        for (int i = 0; i < spades.Length; i++)
        {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<Card>().numValue = i + 1;

            switch(i + 1) {
                case 1:
                    newCard.GetComponent<Card>().numValueString = "ace";
                    break;
                case 11:
                    newCard.GetComponent<Card>().numValueString = "jack";
                    break;
                case 12:
                    newCard.GetComponent<Card>().numValueString = "queen";
                    break;
                case 13:
                    newCard.GetComponent<Card>().numValueString = "king";
                    break;
                default:
                    newCard.GetComponent<Card>().numValueString = "" + (i + 1);
                    break;
            }

            newCard.GetComponent<Card>().suitValue = Card.suit.SPADES;
            newCard.GetComponent<Card>().suitValueString = "spades";

            SetSprite(newCard);

            standardDeck.Add(newCard);
        }

        for (int i = 0; i < diamonds.Length; i++)
        {
            GameObject newCard = Instantiate(card) as GameObject;
            newCard.SetActive(false);

            newCard.GetComponent<Card>().numValue = i + 1;

            switch(i + 1) {
                case 1:
                    newCard.GetComponent<Card>().numValueString = "ace";
                    break;
                case 11:
                    newCard.GetComponent<Card>().numValueString = "jack";
                    break;
                case 12:
                    newCard.GetComponent<Card>().numValueString = "queen";
                    break;
                case 13:
                    newCard.GetComponent<Card>().numValueString = "king";
                    break;
                default:
                    newCard.GetComponent<Card>().numValueString = "" + (i + 1);
                    break;
            }

            newCard.GetComponent<Card>().suitValue = Card.suit.DIAMONDS;
            newCard.GetComponent<Card>().suitValueString = "diamonds";

            SetSprite(newCard);

            standardDeck.Add(newCard);
        }

        setSize(standardDeck);

        return standardDeck;
    }

    /*
    A random card generator.
    
    provide just count to create a random number of cards with random values and random suits.
    provide all of the parameters to limit randomness.

    count - number of random cards to produce
    */
    public List<GameObject> RandomCards(int count)
    {
        if (count <= 0)
        {
            Debug.Log("\"count\" must be 1 or greater.");
            return null;
        }

        List<GameObject> randomCards = new List<GameObject>();

        // ensures number of returned cards is the requested size
        while (randomCards.Count < count)
        {
            // picks a random card number (index)
            int randomNum = UnityEngine.Random.Range(0, 13);

            // picks a random suit (index)
            int randomSuit = UnityEngine.Random.Range(0, 4);

            // adds card to array
            switch (randomSuit)
            {
                case 0:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.CLUBS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;

                        switch(randomNum + 1) {
                            case 1:
                                newCard.GetComponent<Card>().numValueString = "ace";
                                break;
                            case 11:
                                newCard.GetComponent<Card>().numValueString = "jack";
                                break;
                            case 12:
                                newCard.GetComponent<Card>().numValueString = "queen";
                                break;
                            case 13:
                                newCard.GetComponent<Card>().numValueString = "king";
                                break;
                            default:
                                newCard.GetComponent<Card>().numValueString = "" + (randomNum + 1);
                                break;
                        }

                        newCard.GetComponent<Card>().suitValue = Card.suit.CLUBS;
                        newCard.GetComponent<Card>().suitValueString = "clubs";

                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                case 1:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.HEARTS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;

                        switch(randomNum + 1) {
                            case 1:
                                newCard.GetComponent<Card>().numValueString = "ace";
                                break;
                            case 11:
                                newCard.GetComponent<Card>().numValueString = "jack";
                                break;
                            case 12:
                                newCard.GetComponent<Card>().numValueString = "queen";
                                break;
                            case 13:
                                newCard.GetComponent<Card>().numValueString = "king";
                                break;
                            default:
                                newCard.GetComponent<Card>().numValueString = "" + (randomNum + 1);
                                break;
                        }

                        newCard.GetComponent<Card>().suitValue = Card.suit.HEARTS;
                        newCard.GetComponent<Card>().suitValueString = "hearts";

                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                case 2:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.SPADES))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;

                        switch(randomNum + 1) {
                            case 1:
                                newCard.GetComponent<Card>().numValueString = "ace";
                                break;
                            case 11:
                                newCard.GetComponent<Card>().numValueString = "jack";
                                break;
                            case 12:
                                newCard.GetComponent<Card>().numValueString = "queen";
                                break;
                            case 13:
                                newCard.GetComponent<Card>().numValueString = "king";
                                break;
                            default:
                                newCard.GetComponent<Card>().numValueString = "" + (randomNum + 1);
                                break;
                        }

                        newCard.GetComponent<Card>().suitValue = Card.suit.SPADES;
                        newCard.GetComponent<Card>().suitValueString = "spades";

                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                case 3:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.DIAMONDS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;

                        switch(randomNum + 1) {
                            case 1:
                                newCard.GetComponent<Card>().numValueString = "ace";
                                break;
                            case 11:
                                newCard.GetComponent<Card>().numValueString = "jack";
                                break;
                            case 12:
                                newCard.GetComponent<Card>().numValueString = "queen";
                                break;
                            case 13:
                                newCard.GetComponent<Card>().numValueString = "king";
                                break;
                            default:
                                newCard.GetComponent<Card>().numValueString = "" + (randomNum + 1);
                                break;
                        }

                        newCard.GetComponent<Card>().suitValue = Card.suit.DIAMONDS;
                        newCard.GetComponent<Card>().suitValueString = "diamonds";

                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                default:
                    Debug.Log("Error...this should never be reached.");
                    return null;
            }
        }

        setSize(randomCards);

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
        if (count <= 0)
        {
            Debug.Log("\"count\" must be 1 or greater.");
            return null;
        }

        if (minCardNum < 1 || minCardNum > 13 || maxCardNum < 1 || maxCardNum > 13)
        {
            Debug.Log("minCardNum and maxCardNum must be greater than or equal to 1 and less than or equal to 13.");
            return null;
        }

        List<GameObject> randomCards = new List<GameObject>();

        // add suits to array to enable randomness later
        List<string> suits = new List<string>();

        int numOfSuits = 0;
        if (inclClubs)
        {
            suits.Add("clubs");
            numOfSuits++;
        }

        if (inclHearts)
        {
            suits.Add("hearts");
            numOfSuits++;
        }

        if (inclSpades)
        {
            suits.Add("spades");
            numOfSuits++;
        }

        if (inclDiamonds)
        {
            suits.Add("diamonds");
            numOfSuits++;
        }

        // get random cards
        while (randomCards.Count < count)
        {
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

                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.CLUBS;
                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                case 1:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.HEARTS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.HEARTS;
                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                case 2:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.SPADES))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.SPADES;
                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                case 3:
                    if (!ContainsCard(randomCards, randomNum + 1, Card.suit.DIAMONDS))
                    {
                        GameObject newCard = Instantiate(card) as GameObject;
                        newCard.SetActive(false);

                        newCard.GetComponent<Card>().numValue = randomNum + 1;
                        newCard.GetComponent<Card>().suitValue = Card.suit.DIAMONDS;
                        SetSprite(newCard);

                        randomCards.Add(newCard);
                    }
                    break;
                default:
                    Debug.Log("Error...this should also never be reached.");
                    return null;
            }
        }

        setSize(randomCards);

        return randomCards;
    }

    /*
    Card shuffler.

    Given an array of GameObject cards, the function will randomly shuffle the cards and return a new, shuffled array containing the same GameObjects.

    toShuffle - array of GameObject cards to shuffle
    */
    public List<GameObject> ShuffleCards(List<GameObject> toShuffle)
    {
        if (toShuffle.Count <= 1 || toShuffle == null)
        {
            Debug.Log("\"toShuffle\" must be of length 2 or greater in order to be shuffled.");
        }

        List<GameObject> shuffled = new List<GameObject>();

        while (toShuffle.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, toShuffle.Count);

            shuffled.Add(toShuffle[randomIndex]);
            toShuffle.RemoveAt(randomIndex);

        }

        return shuffled;
    }

    /*
    returns the joker card
    */
    public GameObject JokerCard()
    {
        // returns joker card
        GameObject newCard = Instantiate(card) as GameObject;
        newCard.SetActive(false);

        newCard.GetComponent<Card>().numValue = 0;
        newCard.GetComponent<Card>().suitValue = Card.suit.JOKER;
        SetSprite(newCard);

        // switch (cardSize) {
        //     case Card.cardSize.SMALL:
        //         newCard.transform.localScale = new Vector3(.5f, .5f, .5f);
        //         cardSize = Card.cardSize.SMALL;
        //         break;
        //     case Card.cardSize.DEFAULT:
        //         newCard.transform.localScale = new Vector3(.75f, .75f, .75f);
        //         cardSize = Card.cardSize.DEFAULT;
        //         break;
        //     case Card.cardSize.LARGE:
        //         newCard.transform.localScale = new Vector3(1f, 1f, 1f);
        //         cardSize = Card.cardSize.LARGE;
        //         break;
        // }

        return newCard;
    }

    public void SetSprite(GameObject card)
    {
        switch (card.GetComponent<Card>().suitValue)
        {
            case Card.suit.CLUBS:
                card.GetComponent<SpriteRenderer>().sprite = clubs[card.GetComponent<Card>().numValue - 1];
                break;
            case Card.suit.SPADES:
                card.GetComponent<SpriteRenderer>().sprite = spades[card.GetComponent<Card>().numValue - 1];
                break;
            case Card.suit.HEARTS:
                card.GetComponent<SpriteRenderer>().sprite = hearts[card.GetComponent<Card>().numValue - 1];
                break;
            case Card.suit.DIAMONDS:
                card.GetComponent<SpriteRenderer>().sprite = diamonds[card.GetComponent<Card>().numValue - 1];
                break;
            case Card.suit.JOKER:
                card.GetComponent<SpriteRenderer>().sprite = joker;
                break;
        }

        card.GetComponent<Card>().showingFront = true;
    }

    /*


    
    color - color of card back (use by stating "Card.backColor.COLOR" where COLOR is BLUE, GREEN, or RED)
    design - design of card back (use by stating "Card.backDesign.DESIGN" where DESIGN is PLAIN, OUTLINE, OUTLINE_PATTERN, OUTLINE_SIMPLE_PATTERN, or PATTERN)
    */
    public void ShowBackKeepValue(GameObject card, Card.backColor color, Card.backDesign design)
    {
        List<GameObject> cardBacks = new List<GameObject>();

        int index = -1;
        switch (design)
        {
            case Card.backDesign.PLAIN:
                index = 0;
                break;
            case Card.backDesign.OUTLINE:
                index = 1;
                break;
            case Card.backDesign.OUTLINE_PATTERN:
                index = 2;
                break;
            case Card.backDesign.OUTLINE_SIMPLE_PATTERN:
                index = 3;
                break;
            case Card.backDesign.PATTERN:
                index = 4;
                break;
        }

        if (color == Card.backColor.BLUE)
        {
            card.GetComponent<SpriteRenderer>().sprite = blue[index];
        }
        else if (color == Card.backColor.GREEN)
        {
            card.GetComponent<SpriteRenderer>().sprite = green[index];
        }
        else if (color == Card.backColor.RED)
        {
            card.GetComponent<SpriteRenderer>().sprite = red[index];
        }

        card.GetComponent<Card>().showingFront = false;
    }

    /*


    
    custom - Card.backDesign name that you would like the back design set to
    */
    public void ShowBackKeepValue(GameObject card, Card.customDesign custom)
    {
        List<GameObject> cardBacks = new List<GameObject>();

        int index = -1;
        switch (custom)
        {
            // case Card.customDesign.PLAIN:
            //     index = 0;
            //     break;
            // case Card.customDesign.OUTLINE:
            //     index = 1;
            //     break;
            // case Card.customDesign.OUTLINE_PATTERN:
            //     index = 2;
            //     break;
            // case Card.customDesign.OUTLINE_SIMPLE_PATTERN:
            //     index = 3;
            //     break;
            // case Card.customDesign.PATTERN:
            //     index = 4;
            //     break;
        }

        // if (color == Card.backColor.BLUE)
        // {
        //     card.GetComponent<SpriteRenderer>().sprite = blue[index];
        // }
        // else if (color == Card.backColor.GREEN)
        // {
        //     card.GetComponent<SpriteRenderer>().sprite = green[index];
        // }
        // else if (color == Card.backColor.RED)
        // {
        //     card.GetComponent<SpriteRenderer>().sprite = red[index];
        // }

        // newCard.GetComponent<Card>().showingFront = false;
    }

    public void SortCards(List<GameObject> cards)
    {
        // referred to https://www.geeksforgeeks.org/bubble-sort/ to recall bubble sort
        for (int i = 0; i < cards.Count - 1; i++)
        {
            for (int j = 0; j < cards.Count - i - 1; j++)
            {
                if (cards[j].GetComponent<Card>().numValue > cards[j + 1].GetComponent<Card>().numValue)
                {
                    GameObject temp = cards[j];
                    cards[j] = cards[j + 1];
                    cards[j + 1] = temp;
                }
            }
        }
    }

    /*
    to be used instead of List.Contains
    
    list - the list to search for card in
    card - the card to check for in list
    */
    public bool ContainsCard(List<GameObject> list, int numValue, Card.suit suitValue)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetComponent<Card>().numValue == numValue
            && list[i].GetComponent<Card>().suitValue == suitValue)
            {
                return true;
            }
        }

        return false;
    }

    private void setSize(List<GameObject> cards) {
        for (int i = 0; i < cards.Count; i++)
        {
            switch (cardSize) {
                case Card.cardSize.SMALL:
                    cards[i].transform.localScale = new Vector3(.5f, .5f, .5f);
                    cardSize = Card.cardSize.SMALL;
                    break;
                case Card.cardSize.DEFAULT:
                    cards[i].transform.localScale = new Vector3(.75f, .75f, .75f);
                    cardSize = Card.cardSize.DEFAULT;
                    break;
                case Card.cardSize.LARGE:
                    cards[i].transform.localScale = new Vector3(1f, 1f, 1f);
                    cardSize = Card.cardSize.LARGE;
                    break;
                case Card.cardSize.HUGE:
                    Debug.Log("Set to huge");
                    cards[i].transform.localScale = new Vector3(50f, 50f, 50f);
                    cardSize = Card.cardSize.HUGE;
                    break;
            }
        }
    }
    
}
