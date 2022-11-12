using System;
// using System.Collections;
// using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

public class UserPreferences : MonoBehaviour
{
    public enum backgroundColor
    {
        GREEN,
        BLUE,
        ORANGE,
        RED,
        PURPLE
    }

    private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    public Card.cardSize cardSize;
    public Card.customDesign customDesign;
    public backgroundColor backColor;

    public GameObject cardSizeButtonText;
    public GameObject checkDefault;
    public GameObject checkUnlocked;
    public GameObject checkUploadOne;
    public GameObject checkUploadTwo;

    public GameObject errorText;
    private bool changeFailed;
    public GameObject mainCam;

    private int numUnlocked;
    private bool hasWon;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        changeFailed = false;
        numUnlocked = 0;

        RetrieveVarsFromFirebase();

        DetermineUnlock();
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("cardSize", (int)cardSize);
        PlayerPrefs.SetInt("customDesign", (int)customDesign);
        PlayerPrefs.SetInt("backgroundColor", (int)backColor);
    }

    private async void RetrieveVarsFromFirebase()
    {
        // GET SELECTED DESIGN
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").GetValueAsync().ContinueWith(work =>
            {
                if (work.IsCanceled)
                {
                    Debug.Log("get selected design cancelled");
                }
                if (work.IsFaulted)
                {
                    Debug.Log("get selected design faulted");
                }
                else
                {
                    if (work.Result.Value != null)
                    {
                        SetCustomDesignVariable(work.Result.Value.ToString());
                        Debug.Log("from firebase (design) =" + customDesign);
                    }
                    else
                    {
                        SetCustomDesignVariable("blue_outline_simple");
                        Debug.Log("nothing in firebase (design), set to =" + customDesign);
                    }
                }
            });

        // GET CARD SIZE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/card_size").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("get card size cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("get card size faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    cardSize = (Card.cardSize)Int32.Parse(work.Result.Value.ToString());

                    Debug.Log("from firebase (size) =" + cardSize);
                }
                else
                {
                    cardSize = Card.cardSize.DEFAULT;
                    databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/card_size").SetValueAsync(1);
                    Debug.Log("nothing in firebase (size), set to =" + cardSize);
                }
            }
        });

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

        // GET BACKGROUND COLOR
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/background_color").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("get background color cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("get background color faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    backColor = (backgroundColor)Int32.Parse(work.Result.Value.ToString());

                    Debug.Log("from firebase (background) =" + backColor);
                }
                else
                {
                    backColor = backgroundColor.GREEN;
                    Debug.Log("nothing in firebase (background), set to =" + backColor);
                }
            }
        });

        SetBackgroundVariable();

        PositionCheck();
    }

    private void SetBackgroundVariable()
    {
        switch (backColor)
        {
            case backgroundColor.GREEN:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(49, 121, 58, 255);
                break;
            case backgroundColor.BLUE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(43, 100, 159, 255);
                break;
            case backgroundColor.RED:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(222, 50, 73, 255);
                break;
            case backgroundColor.ORANGE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(226, 119, 28, 255);
                break;
            case backgroundColor.PURPLE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(120, 37, 217, 255);
                break;
        }

        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/background_color").SetValueAsync((int)backColor);
    }

    public void ChangeBackgroundColor()
    {
        string clickedName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (clickedName)
        {
            case "green":
                backColor = backgroundColor.GREEN;
                break;
            case "blue":
                backColor = backgroundColor.BLUE;
                break;
            case "red":
                backColor = backgroundColor.RED;
                break;
            case "orange":
                backColor = backgroundColor.ORANGE;
                break;
            case "purple":
                backColor = backgroundColor.PURPLE;
                break;
        }

        SetBackgroundVariable();
    }

    private async void DetermineUnlock()
    {
        // UNLOCK EMOJI
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/solitaire/win_count").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("unlock emoji cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("unlock emoji faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    if (Int32.Parse(work.Result.Value.ToString()) >= 5)
                    {
                        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/emoji").SetValueAsync(true);
                        numUnlocked++;
                    }

                    if (Int32.Parse(work.Result.Value.ToString()) > 0)
                    {
                        hasWon = true;
                    }
                }
            }
        });

        // UNLOCK FISH
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/set_count").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("unlock fish cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("unlock fish faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    if (Int32.Parse(work.Result.Value.ToString()) >= 20)
                    {
                        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/fish").SetValueAsync(true);
                        numUnlocked++;
                    }
                }
            }
        });

        // UNLOCK PETS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/win_count").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("unlock pets cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("unlock pets faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    if (Int32.Parse(work.Result.Value.ToString()) >= 5)
                    {
                        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/pets").SetValueAsync(true);
                        numUnlocked++;
                    }

                    if (Int32.Parse(work.Result.Value.ToString()) > 0)
                    {
                        hasWon = true;
                    }
                }
            }
        });

        // UNLOCK PURDUE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/euchre/trick_count").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("unlock purdue cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("unlock purdue faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    if (Int32.Parse(work.Result.Value.ToString()) >= 20)
                    {
                        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/purdue").SetValueAsync(true);
                        numUnlocked++;
                    }
                }
            }
        });

        // UNLOCK CHECKERED BLACK
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/euchre/win_count").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("unlock checkered black cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("unlock checkered black faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    if (Int32.Parse(work.Result.Value.ToString()) >= 5)
                    {
                        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/checkered_black").SetValueAsync(true);
                        numUnlocked++;
                    }

                    if (Int32.Parse(work.Result.Value.ToString()) > 0)
                    {
                        hasWon = true;
                    }
                }
            }
        });

        // UNLOCK TURKSTRA
        if (numUnlocked >= 15)
        {
            await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/turkstra").SetValueAsync(true);
        }

        // UNLOCK LOGO
        if (hasWon)
        {
            await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/logo").SetValueAsync(true);
        }
    }

    public async void ChangeCardSize()
    {
        switch (cardSize)
        {
            case Card.cardSize.SMALL:
                cardSize = Card.cardSize.DEFAULT;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Default";
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/card_size").SetValueAsync(1);
                break;
            case Card.cardSize.DEFAULT:
                cardSize = Card.cardSize.LARGE;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Large";
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/card_size").SetValueAsync(2);
                break;
            case Card.cardSize.LARGE:
                cardSize = Card.cardSize.SMALL;
                cardSizeButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Small";
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/card_size").SetValueAsync(0);
                break;
        }
    }

    public async void ChangeDesign()
    {
        string clickedName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("clicked " + clickedName);
        errorText.SetActive(false);

        DetermineUnlock();

        // DETERMINE IF CLICKED DESIGN IS UNLOCKED
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/unlocked_cards/" + clickedName).GetValueAsync().ContinueWith(work =>
            {
                if (work.IsCanceled)
                {
                    Debug.Log("unlock check cancelled");
                }
                if (work.IsFaulted)
                {
                    Debug.Log("unlock check faulted");
                }
                else
                {
                    if (work.Result.Value != null && (bool)work.Result.Value)
                    {
                        SetCustomDesignVariable(clickedName);
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
                            case "upload":
                                customDesign = Card.customDesign.UPLOAD;
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
            switch (clickedName)
            {
                case "checkered_black":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Win at least 5 games of Euchre to unlock this design. Please choose another design.";
                    break;
                case "checkered_red":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "boilermaker_special":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "candy_cane":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "daddy_daniels":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "dots":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "emoji":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Win at least 5 games of Solitaire to unlock this design. Please choose another design.";
                    break;
                case "fish":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Achieve a set count of at least 20 in Go Fish to unlock this design. Please choose another design.";
                    break;
                case "food":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "logo":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Win at least once in any card game to unlock this design. Please choose another design.";
                    break;
                case "pets":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Win at least 5 games of Go Fish to unlock this design. Please choose another design.";
                    break;
                case "purdue_pete":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "purdue":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Achieve a trick count of at least 20 in Euchre to unlock this design. Please choose another design.";
                    break;
                case "rick_roll":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "You have not yet unlocked this design. Please choose another design.";
                    break;
                case "turkstra":
                    errorText.GetComponent<TMPro.TextMeshProUGUI>().text = "Unlock all other card designs to unlock this design. Please choose another design.";
                    break;
            }

            errorText.SetActive(true);
            changeFailed = false;
        }
        else
        {
            Debug.Log("customDesign changed = " + customDesign);
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
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.BLUE_OUTLINE:
                checkDefault.transform.localPosition = new Vector3(-235, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.BLUE_OUTLINE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(-125, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.BLUE_OUTLINE_SIMPLE:
                checkDefault.transform.localPosition = new Vector3(-18, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.BLUE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(90, 175, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.GREEN:
                checkDefault.transform.localPosition = new Vector3(-340, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.GREEN_OUTLINE:
                checkDefault.transform.localPosition = new Vector3(-235, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.GREEN_OUTLINE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(-125, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.GREEN_OUTLINE_SIMPLE:
                checkDefault.transform.localPosition = new Vector3(-18, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.GREEN_PATTERN:
                checkDefault.transform.localPosition = new Vector3(90, 45, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.RED:
                checkDefault.transform.localPosition = new Vector3(-340, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.RED_OUTLINE:
                checkDefault.transform.localPosition = new Vector3(-235, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.RED_OUTLINE_PATTERN:
                checkDefault.transform.localPosition = new Vector3(-125, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.RED_OUTLINE_SIMPLE:
                checkDefault.transform.localPosition = new Vector3(-18, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.RED_PATTERN:
                checkDefault.transform.localPosition = new Vector3(90, -85, -10);
                checkDefault.SetActive(true);
                checkUnlocked.SetActive(false);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.CHECKER_BLACK:
                checkUnlocked.transform.localPosition = new Vector3(-340, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.CHECKER_RED:
                checkUnlocked.transform.localPosition = new Vector3(-235, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.BOILERMAKER_SPECIAL:
                checkUnlocked.transform.localPosition = new Vector3(-125, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.CANDY_CANE:
                checkUnlocked.transform.localPosition = new Vector3(-18, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.DADDY_DANIELS:
                checkUnlocked.transform.localPosition = new Vector3(90, 175, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.DOTS:
                checkUnlocked.transform.localPosition = new Vector3(-340, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.EMOJI:
                checkUnlocked.transform.localPosition = new Vector3(-235, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.FISH:
                checkUnlocked.transform.localPosition = new Vector3(-125, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.FOOD:
                checkUnlocked.transform.localPosition = new Vector3(-18, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.LOGO:
                checkUnlocked.transform.localPosition = new Vector3(90, 45, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.PETS:
                checkUnlocked.transform.localPosition = new Vector3(-340, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.PURDUE_PETE:
                checkUnlocked.transform.localPosition = new Vector3(-235, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.PURDUE:
                checkUnlocked.transform.localPosition = new Vector3(-125, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.RICK_ROLL:
                checkUnlocked.transform.localPosition = new Vector3(-18, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.TURKSTRA:
                checkUnlocked.transform.localPosition = new Vector3(90, -85, -10);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(true);
                checkUploadOne.SetActive(false);
                checkUploadTwo.SetActive(false);
                break;
            case Card.customDesign.UPLOAD:
                checkUnlocked.transform.localPosition = new Vector3(90, -85, -10);
                checkUploadOne.SetActive(true);
                checkUploadTwo.SetActive(true);
                checkDefault.SetActive(false);
                checkUnlocked.SetActive(false);
                break;
        }


    }

    private async void SetCustomDesignVariable(string setToString)
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
            case "upload":
                customDesign = Card.customDesign.UPLOAD;
                break;
        }

        Debug.Log("customDesign set to = " + customDesign);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("customization/selected_design").SetValueAsync(setToString);
        PositionCheck();
    }
}
