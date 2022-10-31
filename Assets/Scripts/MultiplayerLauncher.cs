using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerLauncher : MonoBehaviourPunCallbacks
{
        #region Private Serializable Fields


        #endregion


        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";


        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            //PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Connect();
        }


        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinLobby();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                //PhotonNetwork.GameVersion = gameVersion;
            }
        }


    #endregion

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        //PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

    }

    public override void OnJoinedRoom()
    {

    }

    public void joinGoFish() {
               Debug.Log("joinGoFish() called");
               PhotonNetwork.JoinOrCreateRoom("goFish", new RoomOptions { MaxPlayers = 4 },null);


               PhotonNetwork.LoadLevel(7);
               
               if (PhotonNetwork.InRoom) {
                Debug.Log("User is in a room");
               }
               else {
                Debug.Log("User NOT in a room");
               }
    }
    public void joinEuch() {
               PhotonNetwork.JoinOrCreateRoom("euch", new RoomOptions { MaxPlayers = 4 },null);
    }
    public void joinPoker() {
               PhotonNetwork.JoinOrCreateRoom("poker", new RoomOptions { MaxPlayers = 4 },null);
    }
    public void joinBruh() {
               PhotonNetwork.JoinOrCreateRoom("bruh", new RoomOptions { MaxPlayers = 4 },null);
    }

    public override void OnJoinRoomFailed(short returnCode,string message ) {
        Debug.Log("Join room FAILED :(");
    }

    

}

