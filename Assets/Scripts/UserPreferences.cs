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

    public GameObject errorText;
    private bool changeFailed;

    // void OnDisable()
    // {
    //     PlayerPrefs.SetInt("cardSize", (int)cardSize);
    //     PlayerPrefs.SetInt("customDesign", (int)customDesign);
    // }

    // void OnEnable()
    // {
    //     cardSize = (Card.cardSize)PlayerPrefs.GetInt("cardSize");
    //     customDesign = (Card.customDesign)PlayerPrefs.GetInt("customDesign");
    // }

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

        changeFailed = false;

        RetrieveVarsFromFriebase();
    }

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

    private async void RetrieveVarsFromFriebase()
    {
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").GetValueAsync().ContinueWith(task =>
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
                    if (task.Result.Value != null)
                    {
                        SetVariable(task.Result.Value.ToString());
                        Debug.Log("from firebase =" + customDesign);
                    }
                }
            });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/card_size").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                // Debug.Log("card_set_to = blue_outline_simple");
            }
            if (task.IsFaulted)
            {
                // Debug.Log("card_set_to = blue_outline_simple");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    cardSize = (Card.cardSize)Int32.Parse(task.Result.Value.ToString());

                    Debug.Log("card sizee =" + cardSize);
                }
            }
        });
    }

    // private async void DetermineUnlock()
    // {
    //     string cardToCheck = "blue_outline_simple";
    //     for (int i = 0; i < 15; i++)
    //     {
    //         switch (i)
    //         {
    //             case 0:
    //                 cardToCheck = "checkered_red";
    //                 break;
    //             case 1:
    //                 cardToCheck = "boilermaker_special";
    //                 break;
    //             case 2:
    //                 cardToCheck = "candy_cane";
    //                 break;
    //             case 3:
    //                 cardToCheck = "daddy_daniels";
    //                 break;
    //             case 4:
    //                 cardToCheck = "dots";
    //                 break;
    //             case 5:
    //                 cardToCheck = "emoji";
    //                 break;
    //             case 6:
    //                 cardToCheck = "fish";
    //                 break;
    //             case 7:
    //                 cardToCheck = "food";
    //                 break;
    //             case 8:
    //                 cardToCheck = "logo";
    //                 break;
    //             case 9:
    //                 cardToCheck = "pets";
    //                 break;
    //             case 10:
    //                 cardToCheck = "purdue_pete";
    //                 break;
    //             case 11:
    //                 cardToCheck = "purdue";
    //                 break;
    //             case 12:
    //                 cardToCheck = "rick_roll";
    //                 break;
    //             case 13:
    //                 cardToCheck = "turkstra";
    //                 break;
    //             case 14:
    //                 cardToCheck = "checkered_black";
    //                 break;
    //         }

    //         await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/" + cardToCheck).GetValueAsync().ContinueWith(task =>
    //         {
    //             if (task.IsCanceled)
    //             {
    //                 Debug.Log("card_set_to = blue_outline_simple");
    //             }
    //             if (task.IsFaulted)
    //             {
    //                 Debug.Log("card_set_to = blue_outline_simple");
    //             }
    //             else
    //             {
    //                 if (task.Result.Value != null && (bool)task.Result.Value)
    //                 {
    //                     unlockedDesigns.Add(cardToCheck);
    //                     Debug.Log("unlocked = " + unlockedDesigns[unlockedDesigns.Count - 1]);
    //                 }
    //             }
    //         });
    //     }
    // }

    public async void ChangeCardSize()
    {
        switch (cardSize)
        {
            case Card.cardSize.SMALL:
                cardSize = Card.cardSize.DEFAULT;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Default";
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(1);
                break;
            case Card.cardSize.DEFAULT:
                cardSize = Card.cardSize.LARGE;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Large";
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(2);
                break;
            case Card.cardSize.LARGE:
                cardSize = Card.cardSize.SMALL;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Small";
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(0);
                break;
        }
    }

    public async void ChangeDesign()
    {
        string clickedName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("clicked " + clickedName);

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/" + clickedName).GetValueAsync().ContinueWith(task =>
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
                        SetVariable(clickedName);
                    }
                    else
                    {
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
                            default:
                                changeFailed = true;
                                break;
                        }
                    }
                }
            });

        if (changeFailed)
        {
            errorText.SetActive(true);
            changeFailed = false;
        }
        else
        {
            Debug.Log("customDesign = " + customDesign);
            await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(clickedName);
            PositionCheck();
        }
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
    public void setResolution()
    {
        Screen.SetResolution(640, 480, FullScreenMode.MaximizedWindow);
        Debug.Log("Change Resolution");
    }

    private async void SetVariable(string setToString)
    {
        switch (setToString)
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
                customDesign = Card.customDesign.BOILERMAKER_SPECIAL;
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

        Debug.Log("customDesign = " + customDesign);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(setToString);

        PositionCheck();
    }
}
