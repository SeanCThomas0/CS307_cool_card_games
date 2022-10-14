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

    private Player data;

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
                DataSnapshot snapshot = task.Result;

                string playerData = snapshot.GetRawJsonValue();

                Player player = JsonUtility.FromJson<Player>(playerData);

                foreach(var child in snapshot.Children)
                {
                    string t = child.GetRawJsonValue();
                    Player extractedData = JsonUtility.FromJson<Player>(t);

                }

                print("Data is: " + playerData);
            }
        }));
    }
}
