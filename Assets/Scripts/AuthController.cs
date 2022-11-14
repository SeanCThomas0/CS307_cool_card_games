using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using TMPro;
using Firebase.Database;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
    private bool loggedIn = false;

    public static string username;
    public static string userId;
    public static string userEmail;

    /*
     * Function : Start
     * 
     * Description : This function starts up the unity login page
     */
    private void Start()
    {
        loggedIn = false;

        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        loginMessageString = "";
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

        if (loggedIn == true)
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
     * Function : LoginButton
     * 
     * Description : This function attempts to is activated by the Login Button and calls the Login function
     */
    public void LoginButton()
    {
        Debug.Log("LoginButton");
        StartCoroutine(Login());
    }

    /*
     * Function : Login
     * 
     * Description : This function attempts to log a user into their cool card games account. If their credentials
     *               are valid, it will log them in and direct them to the main menu. If invalid, it will give the
     *               appropriate error response
     */
    public IEnumerator Login()
    {
        yield return FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }
            
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

            Debug.Log("Current PhotoUrl: " + FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl == null);

            if (FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl == null)
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    PhotoUrl = new System.Uri("https://firebasestorage.googleapis.com/v0/b/cool-card-games.appspot.com/o/profile_icons%2FLogo.png?alt=media&token=f8c07c18-c585-471b-a8a4-030f7349dc23"),
                };
                newUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.Log("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log("User profile picture updated successfully.");
                });
            }

            username = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
            userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            userEmail = FirebaseAuth.DefaultInstance.CurrentUser.Email;
            string photoURL = FirebaseAuth.DefaultInstance.CurrentUser.PhotoUrl.ToString();

            if (FirebaseAuth.DefaultInstance.CurrentUser.DisplayName.Equals(""))
            {
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    DisplayName = newUser.Email.Substring(0, newUser.Email.IndexOf("@")),
                };
                newUser.UpdateUserProfileAsync(profile).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.Log("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Debug.Log("User profile display name updated successfully.");
                });
            }

            Debug.Log("user profile: " + newUser.DisplayName + " " + newUser.PhotoUrl);

            Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser);
            loggedIn = true;
        });
    }

    public void RegisterButton()
    {
        Debug.Log("RegisterButton");
        StartCoroutine(CreateAccount());
    }

    /*
     * Function : Login
     * 
     * Description : This function creates an account and logs the user in given a valid email and password. If
     *               invalid, the function will give the appropriate error response.
     */
    private IEnumerator CreateAccount()
    {
        yield return FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync was canceled.");

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);

                Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);

                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;

            //Add account to database
            StartCoroutine(CreateAccountData(newUser));

            //FirebaseDatabase.DefaultInstance.RootReference.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

        });

        Debug.Log("Account Created");

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

            Debug.Log("User Logged in: " + newUser.UserId + " " + newUser.DisplayName);
            loggedIn = true;
        });
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

    /*
     * Function : CreateAccountData
     * 
     * Parameter : user //The firebase user we are creating account data for
     * 
     * Description : This function preps the next panel so it is fresh
     */
    public IEnumerator CreateAccountData(Firebase.Auth.FirebaseUser user)
    {
        DatabaseReference userRef = databaseReference.Child("users").Child(user.UserId);

        //user_data
        Debug.Log("Database");
        userRef.Child("user_data").Child("email").SetValueAsync(user.Email);
        userRef.Child("user_data").Child("username").SetValueAsync(user.Email.Substring(0, user.Email.IndexOf("@")));

        //update user profile in authenticator
        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
        {
            DisplayName = user.Email.Substring(0, user.Email.IndexOf("@")),
            PhotoUrl = new System.Uri("https://firebasestorage.googleapis.com/v0/b/cool-card-games.appspot.com/o/profile_icons%2FLogo.png?alt=media&token=f8c07c18-c585-471b-a8a4-030f7349dc23"),
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
                return;
            }

            Debug.Log("User profile updated successfully.");
        });

        Debug.Log("logged event: " + user.UserId + " " + user.Email + " " + user.DisplayName + " " + user.PhotoUrl);
    }
}