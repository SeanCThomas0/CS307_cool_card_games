using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPreferences : MonoBehaviour
{
    public Card.cardSize cardSize = Card.cardSize.DEFAULT;
    public Card.customDesign customDesign = Card.customDesign.GREEN;

    public GameObject cardSizeButtonText;

    void OnDisable()
    {
        PlayerPrefs.SetInt("cardSize", (int)cardSize);
        PlayerPrefs.SetInt("customDesign", (int)customDesign);
    }

    void OnEnable()
    {
        cardSize = (Card.cardSize)PlayerPrefs.GetInt("cardSize");
        customDesign = (Card.customDesign)PlayerPrefs.GetInt("customDesign");
    }

    void Start()
    {
        switch (cardSize)
        {
            case Card.cardSize.SMALL:
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Small";
                break;
            case Card.cardSize.DEFAULT:
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Default";
                break;
            case Card.cardSize.LARGE:
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Large";
                break;
        }
    }

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

    //         RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

    //         Debug.Log(hit.collider);
    //     }
    // }

    public void ChangeCardSize()
    {
        switch (cardSize)
        {
            case Card.cardSize.SMALL:
                cardSize = Card.cardSize.DEFAULT;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Default";
                break;
            case Card.cardSize.DEFAULT:
                cardSize = Card.cardSize.LARGE;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Large";
                break;
            case Card.cardSize.LARGE:
                cardSize = Card.cardSize.SMALL;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Small";
                break;
        }
    }

    public void ChangeDesign()
    {
        string clickedName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("clicked " + clickedName);

        switch (clickedName)
        {
            case "blue":
                customDesign = Card.customDesign.BLUE;
                break;
            case "blue_outline":
                customDesign = Card.customDesign.BLUE_OUTLINE;
                break;
            case "blue_outline_pattern":
                customDesign = Card.customDesign.BLUE_OUTLINE_PATTERN;
                break;
            case "blue_outline_simple":
                customDesign = Card.customDesign.BLUE_OUTLINE_SIMPLE;
                break;
            case "blue_pattern":
                customDesign = Card.customDesign.BLUE_PATTERN;
                break;
            case "green":
                customDesign = Card.customDesign.GREEN;
                break;
            case "green_outline":
                customDesign = Card.customDesign.GREEN_OUTLINE;
                break;
            case "green_outline_pattern":
                customDesign = Card.customDesign.GREEN_OUTLINE_PATTERN;
                break;
            case "green_outline_simple":
                customDesign = Card.customDesign.GREEN_OUTLINE_SIMPLE;
                break;
            case "green_pattern":
                customDesign = Card.customDesign.GREEN_PATTERN;
                break;
            case "red":
                customDesign = Card.customDesign.RED;
                break;
            case "red_outline":
                customDesign = Card.customDesign.RED_OUTLINE;
                break;
            case "red_outline_pattern":
                customDesign = Card.customDesign.RED_OUTLINE_PATTERN;
                break;
            case "red_outline_simple":
                customDesign = Card.customDesign.RED_OUTLINE_SIMPLE;
                break;
            case "red_pattern":
                customDesign = Card.customDesign.RED_PATTERN;
                break;
            case "checkered_black":
                customDesign = Card.customDesign.CHECKER_BLACK;
                break;
            case "checkered_red":
                customDesign = Card.customDesign.CHECKER_RED;
                break;
            case "boilermaker_special":
                customDesign = Card.customDesign.BOILERMAKE_SPECIAL;
                break;
            case "candy_cane":
                customDesign = Card.customDesign.CANDY_CANE;
                break;
            case "daddy_daniels":
                customDesign = Card.customDesign.DADDY_DANIELS;
                break;
            case "dots":
                customDesign = Card.customDesign.DOTS;
                break;
            case "emoji":
                customDesign = Card.customDesign.EMOJI;
                break;
            case "fish":
                customDesign = Card.customDesign.FISH;
                break;
            case "food":
                customDesign = Card.customDesign.FOOD;
                break;
            case "logo":
                customDesign = Card.customDesign.LOGO;
                break;
            case "pets":
                customDesign = Card.customDesign.PETS;
                break;
            case "purdue_pete":
                customDesign = Card.customDesign.PURDUE_PETE;
                break;
            case "purdue":
                customDesign = Card.customDesign.PURDUE;
                break;
            case "rick_roll":
                customDesign = Card.customDesign.RICK_ROLL;
                break;
            case "turkstra":
                customDesign = Card.customDesign.TURKSTRA;
                break;
        }
    }
}
