using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserPreferences : MonoBehaviour
{
    private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    public Card.cardSize cardSize = Card.cardSize.DEFAULT;
    public Card.customDesign customDesign = Card.customDesign.GREEN;

    public GameObject cardSizeButtonText;
    public GameObject checkDefault;
    public GameObject checkUnlocked;

    List<string> unlockedDesigns;
    public GameObject errorText;

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

        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        unlockedDesigns = new List<string>();

        DetermineUnlock();

        PositionCheck();
    }

    private async void DetermineUnlock()
    {
        // string curUsername = null;
        // databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("user_data/username").GetValueAsync().ContinueWith(task =>
        // {
        //     if (task.IsCanceled)
        //     {
        //         Debug.Log("UserPreferences.cs curUsername canceled");
        //     }
        //     if (task.IsFaulted)
        //     {
        //         Debug.Log("UserPreferences.cs curUsername faulted");
        //     }
        //     else
        //     {
        //         curUsername = task.Result.Value.ToString();
        //         Debug.Log("username: " + curUsername);
        //     }
        // });

        string cardToCheck = "blue_outline_simple";
        for (int i = 0; i < 15; i++)
        {
            switch (i)
            {
                case 0:
                    cardToCheck = "checkered_red";
                    break;
                case 1:
                    cardToCheck = "boilermaker_special";
                    break;
                case 2:
                    cardToCheck = "candy_cane";
                    break;
                case 3:
                    cardToCheck = "daddy_daniels";
                    break;
                case 4:
                    cardToCheck = "dots";
                    break;
                case 5:
                    cardToCheck = "emoji";
                    break;
                case 6:
                    cardToCheck = "fish";
                    break;
                case 7:
                    cardToCheck = "food";
                    break;
                case 8:
                    cardToCheck = "logo";
                    break;
                case 9:
                    cardToCheck = "pets";
                    break;
                case 10:
                    cardToCheck = "purdue_pete";
                    break;
                case 11:
                    cardToCheck = "purdue";
                    break;
                case 12:
                    cardToCheck = "rick_roll";
                    break;
                case 13:
                    cardToCheck = "turkstra";
                    break;
                case 14:
                    cardToCheck = "checkered_black";
                    break;
            }

            await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/" + cardToCheck).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("card_set_to = blue_outline_simple");
                }
                if (task.IsFaulted)
                {
                    Debug.Log("card_set_to = blue_outline_simple");
                }
                else
                {
                    if (task.Result.Value != null && (bool)task.Result.Value)
                    {
                        unlockedDesigns.Add(cardToCheck);
                        Debug.Log("unlocked = " + unlockedDesigns[unlockedDesigns.Count - 1]);
                    }
                }
            });
        }
    }

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
                errorText.SetActive(false);
                break;
            case "blue_outline":
                customDesign = Card.customDesign.BLUE_OUTLINE;
                errorText.SetActive(false);
                break;
            case "blue_outline_pattern":
                customDesign = Card.customDesign.BLUE_OUTLINE_PATTERN;
                errorText.SetActive(false);
                break;
            case "blue_outline_simple":
                customDesign = Card.customDesign.BLUE_OUTLINE_SIMPLE;
                errorText.SetActive(false);
                break;
            case "blue_pattern":
                customDesign = Card.customDesign.BLUE_PATTERN;
                errorText.SetActive(false);
                break;
            case "green":
                customDesign = Card.customDesign.GREEN;
                errorText.SetActive(false);
                break;
            case "green_outline":
                customDesign = Card.customDesign.GREEN_OUTLINE;
                errorText.SetActive(false);
                break;
            case "green_outline_pattern":
                customDesign = Card.customDesign.GREEN_OUTLINE_PATTERN;
                errorText.SetActive(false);
                break;
            case "green_outline_simple":
                customDesign = Card.customDesign.GREEN_OUTLINE_SIMPLE;
                errorText.SetActive(false);
                break;
            case "green_pattern":
                customDesign = Card.customDesign.GREEN_PATTERN;
                errorText.SetActive(false);
                break;
            case "red":
                customDesign = Card.customDesign.RED;
                errorText.SetActive(false);
                break;
            case "red_outline":
                customDesign = Card.customDesign.RED_OUTLINE;
                errorText.SetActive(false);
                break;
            case "red_outline_pattern":
                customDesign = Card.customDesign.RED_OUTLINE_PATTERN;
                errorText.SetActive(false);
                break;
            case "red_outline_simple":
                customDesign = Card.customDesign.RED_OUTLINE_SIMPLE;
                errorText.SetActive(false);
                break;
            case "red_pattern":
                customDesign = Card.customDesign.RED_PATTERN;
                errorText.SetActive(false);
                break;
            case "checkered_black":
                if (unlockedDesigns.Contains("checkered_black"))
                {
                    customDesign = Card.customDesign.CHECKER_BLACK;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "checkered_red":
                if (unlockedDesigns.Contains("checkered_red"))
                {
                    customDesign = Card.customDesign.CHECKER_RED;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "boilermaker_special":
                if (unlockedDesigns.Contains("boilermaker_special"))
                {
                    customDesign = Card.customDesign.BOILERMAKER_SPECIAL;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "candy_cane":
                if (unlockedDesigns.Contains("candy_cane"))
                {
                    customDesign = Card.customDesign.CANDY_CANE;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "daddy_daniels":
                if (unlockedDesigns.Contains("daddy_daniels"))
                {
                    customDesign = Card.customDesign.DADDY_DANIELS;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "dots":
                if (unlockedDesigns.Contains("dots"))
                {
                    customDesign = Card.customDesign.DOTS;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "emoji":
                if (unlockedDesigns.Contains("emoji"))
                {
                    customDesign = Card.customDesign.EMOJI;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "fish":
                if (unlockedDesigns.Contains("fish"))
                {
                    customDesign = Card.customDesign.FISH;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "food":
                if (unlockedDesigns.Contains("food"))
                {
                    customDesign = Card.customDesign.FOOD;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "logo":
                if (unlockedDesigns.Contains("logo"))
                {
                    customDesign = Card.customDesign.LOGO;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "pets":
                if (unlockedDesigns.Contains("pets"))
                {
                    customDesign = Card.customDesign.PETS;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "purdue_pete":
                if (unlockedDesigns.Contains("purdue_pete"))
                {
                    customDesign = Card.customDesign.PURDUE_PETE;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "purdue":
                if (unlockedDesigns.Contains("purdue"))
                {
                    customDesign = Card.customDesign.PURDUE;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "rick_roll":
                if (unlockedDesigns.Contains("rick_roll"))
                {
                    customDesign = Card.customDesign.RICK_ROLL;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
            case "turkstra":
                if (unlockedDesigns.Contains("turkstra"))
                {
                    customDesign = Card.customDesign.TURKSTRA;
                    errorText.SetActive(false);
                }
                else
                {
                    errorText.SetActive(true);
                }
                break;
        }

        if (!errorText.activeSelf)
        {
            databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(clickedName);
        }

        PositionCheck();
    }

    private void PositionCheck()
    {
        switch (customDesign)
        {
            case Card.customDesign.BLUE:
                checkDefault.transform.localPosition = new Vector3(-340, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.BLUE_OUTLINE:
                checkDefault.transform.localPosition = new Vector3(-235, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.BLUE_OUTLINE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(-125, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.BLUE_OUTLINE_SIMPLE:
                checkDefault.transform.localPosition = new Vector3(-18, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.BLUE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(90, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.GREEN:
                checkDefault.transform.localPosition = new Vector3(-340, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.GREEN_OUTLINE:
                checkDefault.transform.localPosition = new Vector3(-235, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.GREEN_OUTLINE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(-125, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.GREEN_OUTLINE_SIMPLE:
                checkDefault.transform.localPosition = new Vector3(-18, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.GREEN_PATTERN:
                checkDefault.transform.localPosition = new Vector3(90, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.RED:
                checkDefault.transform.localPosition = new Vector3(-340, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.RED_OUTLINE:
                checkDefault.transform.localPosition = new Vector3(-235, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.RED_OUTLINE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(-125, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.RED_OUTLINE_SIMPLE:
                checkDefault.transform.localPosition = new Vector3(-18, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.RED_PATTERN:
                checkDefault.transform.localPosition = new Vector3(90, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                break;
            case Card.customDesign.CHECKER_BLACK:
                checkUnlocked.transform.localPosition = new Vector3(-340, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.CHECKER_RED:
                checkUnlocked.transform.localPosition = new Vector3(-235, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.BOILERMAKER_SPECIAL:
                checkUnlocked.transform.localPosition = new Vector3(-125, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.CANDY_CANE:
                checkUnlocked.transform.localPosition = new Vector3(-18, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.DADDY_DANIELS:
                checkUnlocked.transform.localPosition = new Vector3(90, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.DOTS:
                checkUnlocked.transform.localPosition = new Vector3(-340, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.EMOJI:
                checkUnlocked.transform.localPosition = new Vector3(-235, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.FISH:
                checkUnlocked.transform.localPosition = new Vector3(-125, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.FOOD:
                checkUnlocked.transform.localPosition = new Vector3(-18, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.LOGO:
                checkUnlocked.transform.localPosition = new Vector3(90, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.PETS:
                checkUnlocked.transform.localPosition = new Vector3(-340, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.PURDUE_PETE:
                checkUnlocked.transform.localPosition = new Vector3(-235, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.PURDUE:
                checkUnlocked.transform.localPosition = new Vector3(-125, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.RICK_ROLL:
                checkUnlocked.transform.localPosition = new Vector3(-18, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
            case Card.customDesign.TURKSTRA:
                checkUnlocked.transform.localPosition = new Vector3(90, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                break;
        }
    }
}
