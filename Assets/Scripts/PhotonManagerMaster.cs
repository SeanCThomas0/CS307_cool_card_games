using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


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
    }

    public override void OnDisable()
    {
        //PhotonNetwork.NetworkingClient.EventReceived -= OnSignalSent;
    }

    public void OnEvent()
    {
        Debug.Log("THIS SHOULD NOT PRINT , MESSAGE SEAN IF IT DOES");
    }




    private void OnDestroy()
    {
        Debug.LogWarning("Network Controller was destoryed: CRITICAL PHOTON ERROR!");
    }


}
