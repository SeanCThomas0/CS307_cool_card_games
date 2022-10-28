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
        PlayerPrefs.SetInt("customDesign", (int)Card.customDesign.RICK_ROLL);
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            Debug.Log(hit.collider);
        }
    }

    public void changeCardSize()
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
}
