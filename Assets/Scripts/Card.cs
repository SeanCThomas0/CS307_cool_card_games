using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum suit {
        CLUBS,
        HEARTS,
        SPADES,
        DIAMONDS,
        JOKER,
        BACK
    }

    // public int index_ID;
    public int numValue;
    public suit suitValue;
}
