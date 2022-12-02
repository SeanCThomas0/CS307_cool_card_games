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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 5) 
                    {
                        tempSetTo = "Get 5 wins for Go Fish: " + task.Result.Value.ToString() + "/5 Incomplete";
                    } 
                    else
                    {
                        tempSetTo = "Get 5 wins for Go Fish: 5/5 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 5 wins for Go Fish: 0/5 Incomplete";
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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 20)
                    {
                        tempSetTo = "Get 20 sets in Go Fish: " + task.Result.Value.ToString() + "/20 Incomplete";
                    } 
                    else 
                    {
                        tempSetTo = "Get 20 sets in Go Fish: 20/20 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 20 sets in Go Fish: 0/20 Incomplete";
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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 5)
                    {
                        tempSetTo = "Get 5 wins in Solitaire: " + task.Result.Value.ToString() + "/5 Incomplete";
                    }
                    else
                    {
                        tempSetTo = "Get 5 wins in Solitaire: 5/5 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 5 wins in Solitaire: 0/5 Incomplete";
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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 5)
                    {
                        tempSetTo = "Get 5 wins in Euchre: " + task.Result.Value.ToString() + "/5 Incomplete";
                    }
                    else
                    {
                        tempSetTo = "Get 5 wins in Euchre: 5/5 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 5 wins in Euchre: 0/5 Incomplete";
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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 20) {
                        tempSetTo = "Get 20 tricks in Euchre: " + task.Result.Value.ToString() + "/20 Incomplete";
                    } else {
                        tempSetTo = "Get 20 tricks in Euchre: 20/20 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 20 tricks in Euchre: 0/20 Incomplete";
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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 10)
                    {
                        tempSetTo = "Get 10 wins in Bruh: " + task.Result.Value.ToString() + "/10 Incomplete";
                    }
                    else
                    {
                        tempSetTo = "Get 10 wins in Bruh: 10/10 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 10 wins in Bruh: 0/10 Incomplete";
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
                    int tempVal = Int32.Parse(task.Result.Value.ToString());
                    if (tempVal < 10)
                    {
                        tempSetTo = "Get 10 wins in Poker: " + task.Result.Value.ToString() + "/10 Incomplete";
                    }
                    else
                    {
                        tempSetTo = "Get 10 wins in Poker: 10/10 Completed";
                    }
                }
                else
                {
                    tempSetTo = "Get 10 wins in Poker: 0/10 Incomplete";
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
