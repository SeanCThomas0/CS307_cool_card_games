using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Photon.Pun;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class MainMenu : MonoBehaviour
{
    /*
     * Function Start
     *called at very start
     *
     */
     private void Start(){
        if (!PhotonNetwork.IsConnected) {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log("Connected to photon : MainMenu.cs");
        }
     }



    /*
     * Function : Update
     * 
     * Description : This function updates the LoginPage scene continuously
     */
    private void Update()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            SceneManager.LoadScene("Scenes/LoginPage");
        }
    }

    /*
     * Function : Logout
     * 
     * Description : This function logs out the user and returns them to the LoginPage scene
     */
    public void Logout()
    {
        Debug.Log("Logout");
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.Email);
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser == null);
        SceneManager.LoadScene("Scenes/LoginPage");
    }

    /*
     * Function : ExitGame
     * 
     * Description : This function logs the user out and exits the application
     */
    public void ExitGame()
    {
        Debug.Log("QUIT");
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.Email);
        FirebaseAuth.DefaultInstance.SignOut();
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser == null);
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

