using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string userID;
    public List<GameObject> hand;

    public float xStartPos;
    public float yStartPos;
    public float zStartPos;

    public int numOfSetsOfFour;
    public GameObject text;

    // -----
    public string Username;
    public string Password;

    public Player() { }

    public Player(string name, string pass)
    {
        Username = name;
        Password = pass;
    }
    // -----

    public void AddToHand(GameObject card)
    {
        hand.Add(card);
    }

    public bool RemoveFromHand(GameObject card)
    {
        return hand.Remove(card);
    }

    public List<GameObject> RemoveAllNumFromHand(int numValue)
    {
        List<GameObject> removed = new List<GameObject>();

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].GetComponent<Card>().numValue == numValue)
            {
                removed.Add(hand[i]);
                hand.RemoveAt(i);
                i--;
            }
        }
        return removed;
    }

    public int RemoveAllSuitFromHand(Card.suit suit)
    {
        int numRemoved = 0;

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i].GetComponent<Card>().suitValue == suit)
            {
                hand.RemoveAt(i);
                numRemoved++;
                i--;
            }
        }
        return numRemoved;
    }
}


// [Serializable]
// public class Player 
// {

// }