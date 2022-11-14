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

    public enum customDesign
    {
        NONE,
        BLUE,
        BLUE_OUTLINE,
        BLUE_OUTLINE_PATTERN,
        BLUE_OUTLINE_SIMPLE,
        BLUE_PATTERN,
        GREEN,
        GREEN_OUTLINE,
        GREEN_OUTLINE_PATTERN,
        GREEN_OUTLINE_SIMPLE,
        GREEN_PATTERN,
        RED,
        RED_OUTLINE,
        RED_OUTLINE_PATTERN,
        RED_OUTLINE_SIMPLE,
        RED_PATTERN,
        CHECKER_BLACK,
        CHECKER_RED,
        BOILERMAKER_SPECIAL,
        CANDY_CANE,
        DADDY_DANIELS,
        DOTS,
        EMOJI,
        FISH,
        FOOD,
        LOGO,
        PETS,
        PURDUE_PETE,
        PURDUE,
        RICK_ROLL,
        TURKSTRA,
        UPLOAD
    }

    public enum cardSize
    {
        SMALL,
        DEFAULT,
        LARGE,
        HUGE
    }

    // values as int
    public int numValue;
    public suit suitValue;

    // values as string
    public string numValueString; // same as 'faceValue' below, but for here again for naming consistency
    public string suitValueString;
    public string frontColor;

    // tells whether the front is showing
    public bool showingFront;

    // for Go Fish
    public bool inPool;

    // for Euchre
    public string faceValue;

    //for solitiare
    public bool top = false;
    public int row;
}
