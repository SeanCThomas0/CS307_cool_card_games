using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class ChangePasswordMenu : MonoBehaviour
{
    FirebaseUser user;
    DatabaseReference databaseReference;
    string oldPassword;
    string passwordText1;
    string passwordText2;


    // Start is called before the first frame update
    void Start()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        oldPassword = "";
        passwordText1 = "";
        passwordText2 = "";
    }

    public void UpdatePasswordText1(string input)
    {
        passwordText1 = input;
    }

    public void UpdatePasswordText2(string input)
    {
        passwordText2 = input;
    }

    public void UpdateOldPassword(string input)
    {
        oldPassword = input;
    }

    public void SaveButton()
    {
        StartCoroutine(UpdatePassword());
    }

    private IEnumerator UpdatePassword()
    {
        if (!passwordText1.Equals(passwordText2))
        {
            Debug.Log("Passwords do not match");
            yield return new WaitForSeconds(0);
        }

        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(user.Email, oldPassword);

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

                user.UpdatePasswordAsync(passwordText2).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("UpdatePasswordAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.Log("UpdatePasswordAsync encountered an error: " + task.Exception);
                        Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                        ErrorMessage((AuthError)e.ErrorCode);
                        return;
                    }

                    Debug.Log("User password updated successfully.");
                });
            });
        }

    }

    private void ErrorMessage(AuthError e)
    {
        Debug.Log("Exception: " + e);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
