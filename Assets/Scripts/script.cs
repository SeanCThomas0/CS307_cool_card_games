using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class script : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    
    public TMP_InputField msgInput;
    public TMP_Text msgArea;
    //public Button joinChatButton;
   // public TMP_InputField usernameInput;
    //hello
    //public PhotonManager photonManager;
    [SerializeField]
    private GameObject joinChatButton;


    [SerializeField]
    private GameObject usernameInput;

    [SerializeField]
    private GameObject chatRoomImage;

    

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        if(string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.LogError("No AppID Provided");
            return;
        }
        Debug.Log("Connecting");
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
            new Photon.Chat.AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
    }

    // Update is called once per frame
    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void ConnectToServer()
    {
        
    }

    public void DisconnectFromServer()
    {
        Debug.Log("Leaving");
        chatClient.Disconnect(ChatDisconnectCause.None);
    }

    public void SendMsg()
    {
        
        chatClient.PublishMessage("Chatroom 1", "Bob" + ": " + "<color=black>" + msgInput.text + "</color>");
        if (string.IsNullOrEmpty(msgArea.text))
            {
                msgArea.text += "<color=black>" + msgInput.text + "</color>" + ", ";
            }
        else
        {
            msgArea.text += "\r\n" + "<color=black>" + msgInput.text + "</color>" + ", ";
        }
        msgInput.text = "";
        
    }

    public void Join()
    {
        chatClient.Subscribe(new string[] {"Chatroom 1"});
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void Leave()
    {
        chatClient.Unsubscribe(new string[] {PhotonNetwork.CurrentRoom.Name});
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }
    
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnDisconnected()
    {
        
    }

    public void OnConnected()
    {
        joinChatButton.SetActive(false);
        usernameInput.SetActive(false);
        chatRoomImage.SetActive(true);
        
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log(senders[i]);
            if (string.IsNullOrEmpty(msgArea.text))
            {
                msgArea.text += messages[i] + ", ";
            }
            else
            {
                msgArea.text += "\r\n" + messages[i] + ", ";
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        msgArea.text = "";
        //photonManager.Leave();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnUserSubscribed(string channel, string user)
    {
        
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        
    }
}