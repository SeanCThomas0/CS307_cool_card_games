using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class ChangeEmailMenu : MonoBehaviour
{
    FirebaseUser user;
    DatabaseReference databaseReference;
    public GameObject oldEmail;
    TextMeshProUGUI textMeshPro_oldEmail;
    string emailText;
    string passwordText;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ChangeEmailMenu Start");
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        textMeshPro_oldEmail = oldEmail.GetComponent<TextMeshProUGUI>();
        emailText = "";
        passwordText = "";
    }

    public void UpdateEmailText(string input)
    {
        emailText = input;
        Debug.Log("EmailText Updated");
    }

    public void UpdatePasswordText(string input)
    {
        passwordText = input;
        Debug.Log("PasswordText Updated");
    }

    public void SaveButton()
    {
        StartCoroutine(UpdateEmail());
    }

    private IEnumerator UpdateEmail()
    {
        Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(user.Email, passwordText);

        yield return user.ReauthenticateAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("ReauthenticateAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("ReauthenticateAsync encountered an error: " + task.Exception);
                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                ErrorMessage((AuthError)e.ErrorCode);
                return;
            }

            Debug.Log("User reauthenticated successfully.");

            user.UpdateEmailAsync(emailText).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("UpdateEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.Log("UpdateEmailAsync encountered an error: " + task.Exception);
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                    ErrorMessage((AuthError)e.ErrorCode);
                    return;
                }

                Debug.Log("User profile display name updated successfully.");

                databaseReference.Child("users").Child(user.UserId).Child("user_data/email").SetValueAsync(emailText).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("Database Email Update Canceled");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.Log("Database Email Update Faulted: " + task.Exception);
                        return;
                    }
                    Debug.Log("Database Email Update Successful");
                });
            });
        });
    }

    private void ErrorMessage(AuthError e)
    {
        Debug.Log("Exception: " + e);
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro_oldEmail.text = user.Email;
    }
}