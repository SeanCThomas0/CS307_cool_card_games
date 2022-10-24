using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using TMPro;
using Firebase.Database;

public class AuthController : MonoBehaviour
{
    public GameObject loginMessage;
    public GameObject forgottenPasswordMessage;
    TextMeshProUGUI textMeshPro_LoginMessage;
    TextMeshProUGUI textMeshPro_ForgottenPasswordMessage;
    public GameObject passwordText;
    TextMeshProUGUI textMeshPro_password;

    private string loginMessageString;
    private string email, password;
    private Color32 msgColor;

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    /*
     * Function : Start
     * 
     * Description : This function starts up the unity login page
     */
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        textMeshPro_LoginMessage = loginMessage.GetComponent<TextMeshProUGUI>();
        textMeshPro_ForgottenPasswordMessage = forgottenPasswordMessage.GetComponent<TextMeshProUGUI>();
        textMeshPro_password = passwordText.GetComponent<TextMeshProUGUI>();
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser);
        Debug.Log(FirebaseAuth.DefaultInstance);
        msgColor = new Color32(255, 0, 0, 255); //Set message color to red for error messages
    }

    /*
     * Function : Update
     * 
     * Description : This function updates the login page continuously
     */
    private void Update()
    {
        textMeshPro_LoginMessage.text = loginMessageString;
        textMeshPro_ForgottenPasswordMessage.text = loginMessageString;

        //Changes text to currently selected color
        if (textMeshPro_ForgottenPasswordMessage.color != msgColor)
        {
            textMeshPro_ForgottenPasswordMessage.color = msgColor;
        }

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser);

            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }

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

            //Add account to database
            CreateAccountData(newUser);

            //FirebaseDatabase.DefaultInstance.RootReference.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });

        //Login the user after successful account creation
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
        Debug.Log("Error: " + errorCode.ToString());

        switch (errorCode)
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
            case AuthError.UserNotFound:
                loginMessageString = "Account does not exist. Please enter a valid email address";
                break;
            case AuthError.TooManyRequests:
                loginMessageString = "Too many failed login attempts. Please try again later";
                break;
        }
    }

    /*
     * Function : ExitGame
     * 
     * Description : This function exits out of the card game application
     */
    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    /*
     * Function : ForgottenPassword
     * 
     * Parameter : email //This is the email that we will send the verification code to
     * 
     * Description : This function sends an email to a user to reset their password
     */
    public void ForgottenPassword()
    {
        Debug.Log("Forgotten Password");
        FirebaseAuth.DefaultInstance.SendPasswordResetEmailAsync(email).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("ForgottenPassword Canceled");
                return;
            }
            if (task.IsFaulted)
            {
                msgColor = new Color32(255, 0, 0, 255);
                Debug.Log("ForgottenPassword Error: " + task.Exception);
                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }

            msgColor = new Color32(255, 255, 255, 255); //Sets text to white
            loginMessageString = "Email Sent to the following email address: " + email;
        });
    }


    /*
     * Function : ChangePanel
     * 
     * Description : This function preps the next panel so it is fresh
     */
    public void ChangePanel()
    {
        loginMessageString = "";
        textMeshPro_password.text = "";
        msgColor = new Color32(255, 0, 0, 255); //Sets text to red
    }

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
}