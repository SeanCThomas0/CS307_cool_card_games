using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using Firebase.Auth;

public class Database : MonoBehaviour
{

    private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;

    public Database()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateAccountData(Firebase.Auth.FirebaseUser user)
    {
        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);

        //user_data
        Debug.Log("Database");
        userRef.Child("user_data").Child("email").SetValueAsync(user.Email);
        userRef.Child("user_data").Child("username").SetValueAsync(user.Email.Substring(0, user.Email.IndexOf("@")));
        Debug.Log("logged event: " + user.UserId + " " + user.Email + " " + user.Email.Substring(0, user.Email.IndexOf("@")));

        //game_statistics

        //solitaire
        userRef.Child("game_statistics").Child("solitaire").Child("win_count").SetValueAsync(0);


    }

    public void SetUserScore(Firebase.Auth.FirebaseUser user, string game, string statistic, int value)
    {
        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);

        userRef.Child("game_statistics").Child(game).Child(statistic).SetValueAsync(value);
    }

    public string GetUserScore(Firebase.Auth.FirebaseUser user, string game, string statistic)
    {
        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);


        userRef.Child("game_statistics").Child(game).Child(statistic).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Value == null)
                {
                    Debug.Log("GetUserScore Null");
                    return null;
                }
                else
                {
                    Debug.Log("GetUserScore Success");
                    Debug.Log(task.Result.Value.ToString());
                    return task.Result.Value.ToString();
                }
                
            }
            else
            {
                Debug.Log("GetUserScore Fail");
                return null;
            }
        });
        return null;
    }

    public void LoadData()
    {
        FirebaseDatabase.DefaultInstance.GetReferenceFromUrl(DATA_URL).GetValueAsync().ContinueWith((task =>
        {
            if (task.IsCanceled)
            {

            }
            if (task.IsFaulted)
            {

            }
            if (task.IsCompleted)
            {
                //Add function
            }
        }));
    }
}
