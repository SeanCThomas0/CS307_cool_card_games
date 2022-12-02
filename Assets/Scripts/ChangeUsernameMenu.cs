using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class ChangeUsernameMenu : MonoBehaviour
{
    FirebaseUser user;
    DatabaseReference databaseReference;
    public GameObject oldUsername;
    TextMeshProUGUI textMeshPro_oldUsername;
    string usernameText;


    // Start is called before the first frame update
    void Start()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        textMeshPro_oldUsername = oldUsername.GetComponent<TextMeshProUGUI>();
        usernameText = "";
    }

    public void UpdateUsernameText(string input)
    {
        usernameText = input;
    }

    public void SaveButton()
    {
        StartCoroutine(UpdateUsername());
    }

    private IEnumerator UpdateUsername()
    {
        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
        {
            DisplayName = usernameText,
        };

        yield return user.UpdateUserProfileAsync(profile).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("UpdateUserProfileAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);
                ErrorMessage(task.Exception);
                return;
            }

            Debug.Log("User profile display name updated successfully.");

            databaseReference.Child("users").Child(user.UserId).Child("user_data/username").SetValueAsync(usernameText).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("Database Username Update Canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.Log("Database Username Update Faulted: " + task.Exception);
                    return;
                }
                Debug.Log("Database Username Update Successful");
            });
        });
    }

    private void ErrorMessage(System.AggregateException e)
    {
        Debug.Log("Exception: " + e);
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro_oldUsername.text = user.DisplayName;
    }
}
