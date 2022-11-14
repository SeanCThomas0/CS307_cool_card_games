using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;

public class DailyGoals : MonoBehaviour
{
    public enum goalTypes
    {
        WINS,
        GO_FISH_SETS,
        EUCHRE_TRICKS,
        ERROR
    }

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    public GameObject goFishText;
    public GameObject solitaireText;
    public GameObject euchreText;
    public GameObject bruhText;
    public GameObject pokerText;

    private goalTypes type;
    private int value;
    private string completed;

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        GatherGoals();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void GatherGoals()
    {
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

                    type = (goalTypes)Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    type = goalTypes.ERROR;
                }
            }
        });

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
                    value = Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    value = -1;
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/go_fish/completed").GetValueAsync().ContinueWith(work =>
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
                    completed = work.Result.Value.ToString();
                }
                else
                {
                    value = -1;
                }
            }
        });

        SetText(goFishText, type, value, completed);

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
                    type = (goalTypes)Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    type = goalTypes.ERROR;
                }
            }
        });

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
                    value = Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    value = -1;
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/solitaire/completed").GetValueAsync().ContinueWith(work =>
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
                    completed = work.Result.Value.ToString();
                }
                else
                {
                    value = -1;
                }
            }
        });

        SetText(solitaireText, type, value, completed);

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

                    type = (goalTypes)Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    type = goalTypes.ERROR;
                }
            }
        });

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
                    value = Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    value = -1;
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/euchre/completed").GetValueAsync().ContinueWith(work =>
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
                    completed = work.Result.Value.ToString();
                }
                else
                {
                    value = -1;
                }
            }
        });

        SetText(euchreText, type, value, completed);

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

                    type = (goalTypes)Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    type = goalTypes.ERROR;
                }
            }
        });

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
                    value = Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    value = -1;
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/bruh/completed").GetValueAsync().ContinueWith(work =>
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
                    completed = work.Result.Value.ToString();
                }
                else
                {
                    value = -1;
                }
            }
        });

        SetText(bruhText, type, value, completed);

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

                    type = (goalTypes)Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    type = goalTypes.ERROR;
                }
            }
        });

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
                    value = Int32.Parse(work.Result.Value.ToString());
                }
                else
                {
                    value = -1;
                }
            }
        });

        await databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("daily_goals/poker/completed").GetValueAsync().ContinueWith(work =>
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
                    completed = work.Result.Value.ToString();
                }
                else
                {
                    value = -1;
                }
            }
        });

        SetText(pokerText, type, value, completed);
    }

    private void SetText(GameObject gameObject, goalTypes type, int value, string completed)
    {
        switch (type)
        {
            case goalTypes.WINS:
                if (completed.Equals("True"))
                {
                    gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Win " + value + " games!";
                }
                else
                {
                    gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Win " + value + " games!";
                }
                break;
            case goalTypes.GO_FISH_SETS:
                if (completed.Equals("True"))
                {
                    gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Achieve a set count of " + value + "!";
                }
                else
                {
                    gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Achieve a set count of " + value + "!";
                }
                break;
            case goalTypes.EUCHRE_TRICKS:
                if (completed.Equals("True"))
                {
                    gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "COMPLETED! Achieve a trick count of " + value + "!";
                }
                else
                {
                    gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Incomplete. Achieve a trick count of " + value + "!";
                }
                break;
            case goalTypes.ERROR:
                gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Goal not available.";
                break;
        }


    }
}