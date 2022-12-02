using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using Firebase.Auth;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Database : MonoBehaviour
{
    //private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    /*
     * Function : Start
     * 
     * Description : This function provides starting values for the scenes
     */
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /*
     * Function : CreateAccountData
     * 
     * Parameter : user //the user object we want to represent in the database
     * 
     * Description : This function creates the starting user data in the firebase database for a new user.
     */
    public void CreateAccountData(Firebase.Auth.FirebaseUser user)
    {
        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);

        //user_data
        Debug.Log("Database");
        userRef.Child("user_data").Child("email").SetValueAsync(user.Email);
        userRef.Child("user_data").Child("username").SetValueAsync(user.Email.Substring(0, user.Email.IndexOf("@")));
        Debug.Log("logged event: " + user.UserId + " " + user.Email + " " + user.Email.Substring(0, user.Email.IndexOf("@")));

        /* Instead of initializing all of these values to zero at the start, we are just not going to initialize them and when we first need them, we will initialize them then
        //game_statistics
        
        //solitaire
        userRef.Child("game_statistics").Child("solitaire").Child("win_count").SetValueAsync(0);
        */
    }

    /*
     * Function : SetUserScore
     * 
     * Parameter : user //the current user we want to get the data from
     * Parameter : game //the game we want to the get the score from
     * Parameter : statistic //the specific statistic we want
     * Parameter : value //the value we want to set the statistic to
     * 
     * Description : This function sets the specific statistic to the given value.
     */
    public void SetUserScore(Firebase.Auth.FirebaseUser user, string game, string statistic, int value)
    {
        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);

        userRef.Child("game_statistics").Child(game).Child(statistic).SetValueAsync(value);
    }

    /*
     * Function : GetUserScore
     * 
     * Parameter : user //the current user we want to get the data from
     * Parameter : game //the game we want to the get the score from
     * Parameter : statistic //the specific statistic we want
     * 
     * Return : the desired game statistic from the database
     * 
     * Description : This function searches through the firebase database for the desired game statistic and returns it.
     */
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

    /*
     * Function : DeleteCurrentUser
     * 
     * Description : Deletes the current user and all of the user's data from firebase systems.
     */
    public void DeleteCurrentUser()
    {
        string userID = auth.CurrentUser.UserId;

        databaseReference.Child("users").Child(userID).RemoveValueAsync();

        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            user.DeleteAsync().ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("User deleted successfully.");
            });
        }
        else
        {
            Debug.LogError("DeleteCurrentUser: current user is null");
        }
    }

    public void ResetStatistics()
    {
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;

        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);

        userRef.Child("game_statistics").RemoveValueAsync();
    }
}
