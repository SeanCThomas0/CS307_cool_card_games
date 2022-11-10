using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    public GameObject goFishWinsText;
    public GameObject goFishSetsText;

    public GameObject solitaireWinsText;

    public GameObject euchreWinsText;
    public GameObject euchreTricksText;

    public GameObject bruhWinsText;

    public GameObject pokerWinsText;

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        RetrieveStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string tempSetTo;

    private async void RetrieveStats()
    {
        // GO FISH WINS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("go fish wins cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("go fish wins faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Wins: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Wins: Not Available";
                }
            }
        });

        SetText(goFishWinsText, tempSetTo);

        // GO FISH WINS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/set_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("go fish sets cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("go fish sets faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Sets: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Sets: Not Available";
                }
            }
        });

        SetText(goFishSetsText, tempSetTo);

        // SOLITAIRE WINS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/solitaire/win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("solitaire wins cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("solitaire wins faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Wins: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Wins: Not Available";
                }
            }
        });

        SetText(solitaireWinsText, tempSetTo);

        // EUCHRE WINS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/euchre/win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("euchre wins cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("euchre wins faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Wins: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Wins: Not Available";
                }
            }
        });

        SetText(euchreWinsText, tempSetTo);

        // EUCHRE TRICKS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/euchre/trick_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("euchre tricks cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("euchre tricks faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Tricks: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Tricks: Not Available";
                }
            }
        });

        SetText(euchreTricksText, tempSetTo);

        // BRUH WINS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/bruh/win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("bruh wins cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("bruh wins faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Wins: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Wins: Not Available";
                }
            }
        });

        SetText(bruhWinsText, tempSetTo);

        // POKER WINS
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/poker/win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("poker wins cancelled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("poker wins faulted");
            }
            else
            {
                if (task.Result.Value != null)
                {
                    tempSetTo = "Wins: " + task.Result.Value.ToString();
                }
                else
                {
                    tempSetTo = "Wins: Not Available";
                }
            }
        });

        SetText(pokerWinsText, tempSetTo);
    }

    private void SetText(GameObject toSet, string setTo)
    {
        toSet.GetComponent<TMPro.TextMeshProUGUI>().text = setTo;
    }
}
