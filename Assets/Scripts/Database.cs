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
    
    public Text username, password;

    private Player data;

    private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData()
    {
        if(username.text.Equals("") && password.text.Equals(""))
        {
            return;
        }

        data = new Player(username.text, password.text);

        string jsonData = JsonUtility.ToJson(data);

        databaseReference.Child("Users" + Random.Range(0, 1000000000)).SetRawJsonValueAsync(jsonData);
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
