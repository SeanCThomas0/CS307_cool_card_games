using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;

public class ScoreKeeper : MonoBehaviour
{
    public Selectable[] topStacks;
    public GameObject highScorePanel;
    private bool scoreUpdated = false;
    Database database;
    Firebase.Auth.FirebaseUser user;
    int win_count;
    // Start is called before the first frame update
    void Start()
    {
        database = new Database();
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(user.UserId);

        userRef.Child("game_statistics").Child("solitaire").Child("win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Value == null)
                {
                    Debug.Log("GetUserScore Null");
                    win_count = 0;
                }
                else
                {
                    Debug.Log("GetUserScore Success");
                    Debug.Log(task.Result.Value.ToString());
                    win_count = Int32.Parse(task.Result.Value.ToString());
                }

            }
            else
            {
                Debug.Log("GetUserScore Fail");
                win_count = 0;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (HasWon() && !scoreUpdated) {
            Win();
        }
    }

    public bool HasWon() {
        int i = 0;
        foreach (Selectable topstack in topStacks) {
            i += topstack.value;
        }
        if (i >= 52) {
            return true;
        } else {
            return false;
        }
    }

    void Win() {
        scoreUpdated = true;
        highScorePanel.SetActive(true);
        print("You have won!");
        win_count++;
        database.SetUserScore(user, "solitaire", "win_count", win_count);
        Debug.Log("New win_count: " + win_count);
    }
}
