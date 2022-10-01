using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.CS307.CoolCardGames.Launcher
{

    public class Launcher : MonoBehaviour
    {
        #region Private Serializable Fields


        #endregion

        #region Private Fields

        ///<summary>
        /// This client's version number. Group users by version number
        /// </summary>
        string gameVersion = "1";

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
            Connect();
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
















    }
}
