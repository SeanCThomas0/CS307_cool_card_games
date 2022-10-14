using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviour

{

    public GameObject PhotonServerMainMenu;
    PhotonServerMaster temp;

    void Awake() {
        temp= PhotonServerMainMenu.GetComponent<PhotonServerMaster>();

    }



    public void Logout()
    {
        Debug.Log("Logout");
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("Scenes/LoginPage");        
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        print(FirebaseAuth.DefaultInstance.CurrentUser.Email);
        FirebaseAuth.DefaultInstance.SignOut();
        print(FirebaseAuth.DefaultInstance.CurrentUser == null);
        Application.Quit();
    }

    public void ChangeScenese(string scene)
    {
        Debug.Log("Change to " + scene);
        //SceneManager.LoadScene(scene);
        // THIS HAS TO BE A BYTE FOR SOME REASON???????????????
        byte max =  0;
        if (scene == "GoFish") max =temp.maxPlayersFish;
        if (scene == "Solitaire") max =temp.maxPlayersSolitare;
        if (scene == "EuchreTestScene") max =temp.maxPlayersEucher;
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.CreateRoom(scene,new RoomOptions { MaxPlayers = max},null);
        //PhotonNetwork.JoinOrCreateRoom(scene,new RoomOptions { MaxPlayers = max},TypedLobby.Default);
        //PhotonNetwork.JoinRoom(scene);
        //PhotonNetwork.JoinOrCreateRoom("Game", roomOptions, TypedLobby.Default);
        //PhotonNetwork.JoinRandomRoom();
       // PhotonNetwork.JoinRandomRoom();
       //PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(scene);


        Debug.Log("JOINED GAME");
        

    }
}

