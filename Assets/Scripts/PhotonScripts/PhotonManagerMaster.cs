using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;



namespace PhotonScripts
{
    public class PhotonManagerMaster : MonoBehaviourPunCallbacks,       
        IMatchmakingCallbacks, IInRoomCallbacks, ILobbyCallbacks, IErrorInfoCallback, IConnectionCallbacks

    {
        // Start is called before the first frame update
        void Start()
        {
            //PhotonNetwork.AddCallbackTarget(this);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Photon connected to Mater server");
            

        }

        public static void setNickname(string nickname) {
            PhotonNetwork.NickName= nickname;
        }

        public override void OnEnable()
        {
            //PhotonNetwork.NetworkingClient.EventReceived += OnSignalSent;
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            //PhotonNetwork.NetworkingClient.EventReceived -= OnSignalSent;
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            
            //Debug.Log("THIS SHOULD NOT PRINT , MESSAGE SEAN IF IT DOES");
            if (photonEvent.Code == (int)PhotonEventCodes.HostToClientData){
                object[] data = (object[]) photonEvent.CustomData;
                string test = (string) data[0];
                //bool test = (bool) data[1];
                //int players =(int) data[2];
                Debug.Log(test);

            }
            else if (photonEvent.Code == (int)PhotonEventCodes.HostSendRoomInfo){
                object[] data = (object[]) photonEvent.CustomData;
                string roomName = (string) data[0];
                int numPlayersInRoom = (int) data[1];
                //Debug.Log(test);

            }
            else if (photonEvent.Code == (int)PhotonEventCodes.StartGame) {
                object[] data = (object[]) photonEvent.CustomData;
                string SceneName = (string) data[0];
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                    SceneManager.LoadScene("GoFishMultiplayer");
                    
                }
            }

        }

        public static void SendCardsToPlayer(string username){
            object[] content = new object[]
            {
                username
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            PhotonNetwork.RaiseEvent((int) PhotonEventCodes.HostToClientData,content,raiseEventOptions, SendOptions.SendUnreliable);
            Debug.Log(username);
            Debug.Log("Tried to use raise event - Master");
        }
        public static void UpdateRoomInfo(string username){
            object[] content = new object[]
            {
                username
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            PhotonNetwork.RaiseEvent((int) PhotonEventCodes.HostToClientData,content,raiseEventOptions, SendOptions.SendUnreliable);
            Debug.Log(username);
            Debug.Log("Tried to use raise event - Master");
        }

        public static void StartGame(string gamename){
            object[] content = new object[]
            {
                gamename
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {Receivers = ReceiverGroup.All};
            PhotonNetwork.RaiseEvent((int) PhotonEventCodes.StartGame,content,raiseEventOptions, SendOptions.SendUnreliable);
            Debug.Log(gamename);
            Debug.Log("Tried to use raise event - Master: StartGame");
        }

        public static void CreateJoinRoom(){
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log("make room pog!");
                PhotonNetwork.JoinOrCreateRoom("GoFish",options, null);
                //SceneManager.LoadScene("GoFishMultiplayer");
            }
        }

        public override void OnJoinedRoom(){
            Debug.Log("joined room:" + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Players in " +PhotonNetwork.CurrentRoom.Name +":" + PhotonNetwork.CurrentRoom.PlayerCount);
        }




        private void OnDestroy()
        {
            Debug.LogWarning("Network Controller was destoryed: could be bad? ");
        }


    }
}
