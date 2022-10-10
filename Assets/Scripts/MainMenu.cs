using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class MainMenu : MonoBehaviour
{
    public void Logout ()
    {
        Debug.Log("Logout");
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        print(FirebaseAuth.DefaultInstance.CurrentUser.Email);
        FirebaseAuth.DefaultInstance.SignOut();
        print(FirebaseAuth.DefaultInstance.CurrentUser == null);
        Application.Quit();
    }
}

