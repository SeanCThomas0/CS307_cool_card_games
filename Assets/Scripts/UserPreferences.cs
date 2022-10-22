using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPreferences : MonoBehaviour
{
    public Card.cardSize cardSize = Card.cardSize.DEFAULT;

    public GameObject cardSizeButtonText;

    void OnDisable() {
        PlayerPrefs.SetInt("cardSize", (int)cardSize);
    }

    void OnEnable() {
        cardSize = (Card.cardSize) PlayerPrefs.GetInt("cardSize");
    }

    void Start() {
        switch (cardSize) {
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

    public void changeCardSize() {
        switch (cardSize) {
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
}
