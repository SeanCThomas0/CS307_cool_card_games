using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            SceneManager.LoadScene("Scenes/LoginPage");
        }
    }

    public void Logout()
    {
        Debug.Log("Logout");
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.Email);
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser == null);
        SceneManager.LoadScene("Scenes/LoginPage");
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.Email);
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser == null);
        Application.Quit();
    }

    public void ChangeScenes(string scene)
    {
        Debug.Log("Change to " + scene);
        SceneManager.LoadScene(scene);
    }
}

