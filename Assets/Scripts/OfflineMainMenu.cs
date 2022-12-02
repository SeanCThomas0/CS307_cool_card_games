using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineMainMenu : MonoBehaviour
{
    /*
     * Function : Logout
     * 
     * Description : This function logs out the user and returns them to the LoginPage scene
     */
    public void Logout()
    {
        SceneManager.LoadScene("Scenes/LoginPage");
    }

    /*
     * Function : ExitGame
     * 
     * Description : This function logs the user out and exits the application
     */
    public void ExitGame()
    {
        Application.Quit();
    }

    /*
     * Function : ChangeScenes
     * 
     * Parameter : scene //The path of the scene we are changing to
     * 
     * Description : This function changes to the scene provided in the string
     */
    public void ChangeScenes(string scene)
    {
        Debug.Log("Change to " + scene);
        SceneManager.LoadScene(scene);
    }
}
