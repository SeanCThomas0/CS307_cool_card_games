using ExitGames.Client.Photon;
using Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChatManage : MonoBehaviour, IChatClientListener
{

    public void sendMessage() {
        chatClient.PublishMessage("Hello", ChatInput.text);
    } 

    public void DebugReturn(DebugLevel level, string message) {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state) {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected() {
        throw new System.NotImplementedException();
    }

    public void OnConnected() {
        JoinChatButton.setActive(false);
        UsernameInput.setActive(false);
        ChatRoomImage.setActive(true);
        chatClient.Subscribe(new string[] {"Hello"});
        chatClient.SetOnlineStatus(ChatUserStatus.Online);

    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages) {
        for (int i = 0; i < senders.Length; i++)
        {
            ScrollText.text += senders[i] + ": " + messages[i] + ", ";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName) {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results) {
        foreach (var channel in channels)
        {
            this.chatClient.PublishMessage(channel, "joined");

        }
        
    }

    public void OnUnsubscribed(string[] channels) {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user) {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user) {
        throw new System.NotImplementedException();  
    }

    ChatClient chatClient;

    [SerializeField] string userID;

    // Start is called before the first frame update
    void Start()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userID));
    }


    // Update is called once per frame
    void Update()
    {
        chatClient.Service();
    }
}
