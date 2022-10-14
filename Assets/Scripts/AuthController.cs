using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Photon.Pun;
using Photon.Realtime;


using TMPro;

public class AuthController : MonoBehaviourPun
{
    public GameObject loginMessage;
    public GameObject PhotonServerObj;
    PhotonServerMaster temp;
    TextMeshProUGUI textMeshPro_LoginMessage;
    private string loginMessageString;

    /*
    / SEAN SERVER STUFF I HOPE IT DONT MESS THIS UP
    */





 


    private void Start()
    {
        textMeshPro_LoginMessage = loginMessage.GetComponent<TextMeshProUGUI>();
        temp= PhotonServerObj.GetComponent<PhotonServerMaster>();



    }
    

    private void Update()
    {
        textMeshPro_LoginMessage.text = loginMessageString;

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene("Scenes/MainMenu");
            temp.Connect();
        }
    }

    private string email, password;

    /*
     * Function : ReadEmailString
     * 
     * Parameter : input //The string to be read from the text box
     * 
     * Description : This function reads the email string and assigns it to the email variable
     */
    public void ReadEmailString(string input)
    {
        email = input;
        Debug.Log("Email: " + email);
    }

    /*
     * Function : ReadPasswordString
     * 
     * Parameter : input //The string to be read from the text box
     * 
     * Description : This function reads the password string and assigns it to the password variable
     */
    public void ReadPasswordString(string input)
    {
        password = input;
        Debug.Log("Password: " + password);
    }

    /*
     * Function : Login
     * 
     * Description : This function attempts to log a user into their cool card games account. If their credentials
     *               are valid, it will log them in and direct them to the main menu. If invalid, it will give the
     *               appropriate error response
     */
    public void Login()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }
            
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });

        print(FirebaseAuth.DefaultInstance.CurrentUser);
    }


    /*
     * Function : Login
     * 
     * Description : This function creates an account and logs the user in given a valid email and password. If
     *               invalid, the function will give the appropriate error response.
     */
    public void CreateAccount()
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });

        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
        });

        print(FirebaseAuth.DefaultInstance.CurrentUser);
    }

    /*
     * Function : GetErrorMessage
     * 
     * Parameter : errorCode //The error code given by the Firebase Authenticator class
     * 
     * Description : This function gives the appropriate UI response to the user, given an error
     */
    void GetErrorMessage(AuthError errorCode)
    {
        string msg = errorCode.ToString(); //Error Code

        switch(errorCode)
        {
            case AuthError.EmailAlreadyInUse:
                loginMessageString = "This email is already in use. Please use a different email";
                break;
            case AuthError.MissingPassword:
                loginMessageString = "Please enter in a valid password";
                break;
            case AuthError.InvalidEmail:
                loginMessageString = "Please enter in a valid email address";
                break;
            case AuthError.WrongPassword:
                loginMessageString = "Incorrect email or password. Please enter a valid email and password";
                break;
            case AuthError.WeakPassword:
                loginMessageString = "Password is too weak. Please enter a stronger password";
                break;
            case AuthError.MissingEmail:
                loginMessageString = "Please enter in a valid email address";
                break;
        }

        print(loginMessageString);
        print("Error: " + msg);
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();

    }
}
