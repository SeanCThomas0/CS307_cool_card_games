using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;

public class DailyGoals : MonoBehaviour
{
    public enum games
    {
        GO_FISH,
        SOLITAIRE,
        EUCHRE,
        BRUH,
        POKER
    }

    public enum goFishTypes
    {
        WINS,
        SETS
    }

    public enum solitaireTypes
    {
        WINS
    }

    public enum euchreTypes
    {
        WINS,
        TRICKS
    }

    public enum bruhTypes
    {
        WINS
    }

    public enum pokerTypes
    {
        WINS
    }

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    public GameObject goFishText;
    public GameObject solitaireText;
    public GameObject euchreText;
    public GameObject bruhText;
    public GameObject pokerText;

    private int type;
    private int value;
    private string completed;

    private DateTime today;
    private bool generateNewGoals;

    private int tempValue;

    public GameObject goFishCheck;
    public GameObject solitaireCheck;
    public GameObject euchreCheck;
    public GameObject bruhCheck;
    public GameObject pokerCheck;

    private bool updating = false;

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        today = DateTime.Today;
        generateNewGoals = false;

        Debug.Log("hi");
        EvaluateDate();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private async void EvaluateDate()
    {
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/last_updated").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    if (!work.Result.Value.ToString().Equals(today.ToString()))
                    {
                        generateNewGoals = true;
                    }
                }
                else
                {
                    generateNewGoals = true;
                }
            }
        });

        if (generateNewGoals)
        {
            generateNewGoals = false;
            GenerateNewGoals();
        }
        else
        {
            DetermineComplete();
        }
    }

    private async void GenerateNewGoals()
    {
        // SET DATE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/last_updated").SetValueAsync(today.ToString());

        // RANDOM GO FISH
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/type").SetValueAsync(UnityEngine.Random.Range(0, Enum.GetValues(typeof(goFishTypes)).Length));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/value").SetValueAsync(UnityEngine.Random.Range(1, 10));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/set_count").SetValueAsync(0);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/win_count").SetValueAsync(0);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/completed").SetValueAsync(false);

        // RANDOM SOLITAIRE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/type").SetValueAsync(UnityEngine.Random.Range(0, Enum.GetValues(typeof(solitaireTypes)).Length));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/value").SetValueAsync(UnityEngine.Random.Range(1, 10));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/win_count").SetValueAsync(0);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(false);

        // RANDOM EUCHRE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/type").SetValueAsync(UnityEngine.Random.Range(0, Enum.GetValues(typeof(euchreTypes)).Length));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/value").SetValueAsync(UnityEngine.Random.Range(1, 10));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/win_count").SetValueAsync(0);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/completed").SetValueAsync(false);

        // RANDOM BRUH
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/type").SetValueAsync(UnityEngine.Random.Range(0, Enum.GetValues(typeof(bruhTypes)).Length));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/value").SetValueAsync(UnityEngine.Random.Range(1, 10));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/win_count").SetValueAsync(0);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/completed").SetValueAsync(false);

        // RANDOM POKER
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/type").SetValueAsync(UnityEngine.Random.Range(0, Enum.GetValues(typeof(pokerTypes)).Length));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/value").SetValueAsync(UnityEngine.Random.Range(1, 10));
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/win_count").SetValueAsync(0);
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/completed").SetValueAsync(false);

        DetermineComplete();
    }

    private void SetText(GameObject gameObject, games game, int type, int value, string completed)
    {
        if (type == -1)
        {
            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Goal not available.";
            return;
        }

        switch (game)
        {
            case games.GO_FISH:
            Debug.Log("state: " + completed);
                switch ((goFishTypes)type)
                {
                    case goFishTypes.WINS:
                        if (completed.Equals("True"))
                        {
                            goFishCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Win " + value + " games!";
                        }
                        else
                        {
                            goFishCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Win " + value + " games!";
                        }
                        break;
                    case goFishTypes.SETS:
                        if (completed.Equals("True"))
                        {
                            Debug.Log("hmm");
                            goFishCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Achieve a set count of " + value + "!";
                        }
                        else
                        {
                            Debug.Log("hola");
                            goFishCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Achieve a set count of " + value + "!";
                        }
                        break;
                }
                break;

            case games.SOLITAIRE:
                switch ((solitaireTypes)type)
                {
                    case solitaireTypes.WINS:
                        if (completed.Equals("True"))
                        {
                            solitaireCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Win " + value + " games!";
                        }
                        else
                        {
                            solitaireCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Win " + value + " games!";
                        }
                        break;
                }
                break;

            case games.EUCHRE:
                switch ((euchreTypes)type)
                {
                    case euchreTypes.WINS:
                        if (completed.Equals("True"))
                        {
                            euchreCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Win " + value + " games!";
                        }
                        else
                        {
                            euchreCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Win " + value + " games!";
                        }
                        break;
                    case euchreTypes.TRICKS:
                        if (completed.Equals("True"))
                        {
                            euchreCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Achieve a trick count of " + value + "!";
                        }
                        else
                        {
                            euchreCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Achieve a trick count of " + value + "!";
                        }
                        break;
                }
                break;

            case games.BRUH:
                switch ((bruhTypes)type)
                {
                    case bruhTypes.WINS:
                        if (completed.Equals("True"))
                        {
                            bruhCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Win " + value + " games!";
                        }
                        else
                        {
                            bruhCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Win " + value + " games!";
                        }
                        break;
                }
                break;

            case games.POKER:
                switch ((pokerTypes)type)
                {
                    case pokerTypes.WINS:
                        if (completed.Equals("True"))
                        {
                            pokerCheck.SetActive(true);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Win " + value + " games!";
                        }
                        else
                        {
                            pokerCheck.SetActive(false);
                            gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Win " + value + " games!";
                        }
                        break;
                }
                break;
        }
    }

    private async void DetermineComplete()
    {
        updating = true;

        int tempType = -1;
        string tempComplete = "null";

        // GO FISH
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/value").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempValue = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/type").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempType = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        switch ((goFishTypes)tempType)
        {
            case goFishTypes.WINS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/win_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            Debug.Log("here: " + tempValue);
                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;
            case goFishTypes.SETS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/set_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            Debug.Log("set_count: " + work.Result.Value);
                            Debug.Log("tempValue: " + tempValue);

                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {

                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;
        }

        SetText(goFishText, games.GO_FISH, tempType, tempValue, tempComplete);

        tempType = -1;
        tempComplete = "null";

        // SOLITAIRE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/value").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempValue = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/type").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempType = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        switch ((solitaireTypes)tempType)
        {
            case solitaireTypes.WINS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/win_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;
        }

        SetText(solitaireText, games.SOLITAIRE, tempType, tempValue, tempComplete);

        tempType = -1;
        tempComplete = "null";

        // EUCHRE
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/value").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempValue = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/type").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempType = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        switch ((euchreTypes)tempType)
        {
            case euchreTypes.WINS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/win_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;

            case euchreTypes.TRICKS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/trick_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;
        }

        SetText(euchreText, games.EUCHRE, tempType, tempValue, tempComplete);

        tempType = -1;
        tempComplete = "null";

        // BRUH
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/value").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempValue = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/type").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempType = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        switch ((bruhTypes)tempType)
        {
            case bruhTypes.WINS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/win_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;
        }

        SetText(bruhText, games.BRUH, tempType, tempValue, tempComplete);

        tempType = -1;
        tempComplete = "null";

        // POKER
        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/value").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempValue = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/type").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    tempType = Int32.Parse(work.Result.Value.ToString());
                }
            }
        });

        switch ((pokerTypes)tempType)
        {
            case pokerTypes.WINS:
                await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/win_count").GetValueAsync().ContinueWith(work =>
                {
                    if (work.IsCanceled)
                    {
                        Debug.Log("cancelled");
                    }
                    if (work.IsFaulted)
                    {
                        Debug.Log("faulted");
                    }
                    else
                    {
                        if (work.Result.Value != null)
                        {
                            if (Int32.Parse(work.Result.Value.ToString()) >= tempValue)
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/completed").SetValueAsync(true);
                                tempComplete = "True";
                            }
                            else
                            {
                                databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/completed").SetValueAsync(false);
                                tempComplete = "False";
                            }
                        }
                    }
                });
                break;
        }

        SetText(pokerText, games.POKER, tempType, tempValue, tempComplete);

        updating = false;
    }

    public void Refresh() {
        if (!updating) {
            DetermineComplete();
        }
    }
}