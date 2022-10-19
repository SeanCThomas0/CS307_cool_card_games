using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum suit
    {
        CLUBS,
        HEARTS,
        SPADES,
        DIAMONDS,
        JOKER,
        BACK
    }

    public enum backColor
    {
        BLUE,
        GREEN,
        RED
    }

    public enum backDesign
    {
        PLAIN,
        OUTLINE,
        OUTLINE_PATTERN,
        OUTLINE_SIMPLE_PATTERN,
        PATTERN
    }

    public enum customDesign {
        
    }

    public enum cardSize {
        SMALL,
        DEFAULT,
        LARGE
    }

    // values as int
    public int numValue;
    public suit suitValue;
    
    // values as string
    public string numValueString; // same as 'faceValue' below, but for here again for naming consistency
    public string suitValueString;

    // for Go Fish
    public bool inPool; 

    // for Euchre
    public bool showingFront;
    public string faceValue;
}
