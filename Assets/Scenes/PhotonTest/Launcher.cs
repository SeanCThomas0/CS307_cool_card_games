using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace com.CS307.CoolCardGames.Launcher
{

    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion

        #region Private Fields

        ///<summary>
        /// This client's version number. Group users by version number
        /// </summary>
        string gameVersion = "1";

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        [Tooltip("The UI for game stuff")]
        [SerializeField]
        private GameObject GameController;

        #endregion

        #region MonoBehavior Callbacks

        ///<summary>
        /// MonoBehavior methed called on GameObject by Unity during early initialization phase
        /// </summary>
        void Awake()
        {
            //#Critical
            //this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;


        }

        ///<summary>
        /// MonoBheavior method called on GameObject by Unity during initialzation phase
        /// </summary>

        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }


        #endregion


        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// If already connected, we attempt joining random room
        /// if not connected connect this application to photon cloud network
        /// </summary>

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);



            //we check if we are connected or not, if we are then join, if not we make the connection
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need this point to attempt joining a random rooms If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }    
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Bascis Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            GameController.SetActive(false);



            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            progressLabel.SetActive(false);
            GameController.SetActive(true);

        }











    }
}
