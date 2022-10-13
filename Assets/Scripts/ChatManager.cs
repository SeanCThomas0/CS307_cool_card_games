using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;


/*
Chat Manager script manages the chat fucntions including joining rooms
and sending/receiving messages. It implements the IChatClientListener
that responds to the different event of the chatClient.
*/
public class ChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    
    public TMP_InputField msgInput;
    public TMP_Text msgArea;

    public TMP_InputField toInput;
    
    public TMP_InputField usernameInputField;
    
    [SerializeField]
    private GameObject joinChatButton;


    [SerializeField]
    private GameObject usernameInput;

    [SerializeField]
    private GameObject chatRoomImage;

    public string username;

    

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        if(string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.LogError("No AppID Provided");
            return;
        }
        ConnectToServer();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //must be called repeatedly to make chatClient function
        if (chatClient != null)
        {
            chatClient.Service();
        }
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting");
        chatClient = new ChatClient(this);
        
        //Connect to Photon Server and set username to input field
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new Photon.Chat.AuthenticationValues(usernameInputField.text));
        
    }

    public void DisconnectFromServer()
    {
        Debug.Log("Leaving");
        chatClient.Disconnect(ChatDisconnectCause.None);
    }

    public void SendMsg()
    {
        //check to send message publicly or privately
        if(string.IsNullOrEmpty(toInput.text)) {
            chatClient.PublishMessage("Chatroom", "<color=black>" + username + ": " + msgInput.text + "</color>");
            
        }
        else {
            chatClient.SendPrivateMessage(toInput.text, username + ": " + msgInput.text + "</color>");
            Debug.Log("Target: " + toInput.text);
        }
        
        
        //reset message input field
        msgInput.text = "";
        
    }

    public void Join()
    {
       
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

    //when Connect is called, subscribe user to the chatroom
    public void OnConnected()
    {
        Debug.Log("Connected and Subscribed");
        chatClient.Subscribe(new string[] {"Chatroom"});
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    //when button is pressed, join chatroom
    public void JoinChatRoom() {
        username = usernameInputField.text;
        joinChatButton.SetActive(false);
        usernameInput.SetActive(false);
        chatRoomImage.SetActive(true);
        msgArea.text += "\r\n" + "<color=black>" + username + ": " + "joined" + "</color>";
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }
    //when publish message is called and messages are received, display them in chatroom
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log("Received");
        for (int i = 0; i < senders.Length; i++)
        {
            Debug.Log(senders[i]);
            if (string.IsNullOrEmpty(msgArea.text))
            {
                msgArea.text += messages[i];
            }
            else
            {
                msgArea.text += "\r\n" + messages[i];
            }
        }
    }

    //when private message is received, display in chatroom
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("Received Private");
        if (string.IsNullOrEmpty(msgArea.text))
            {
                msgArea.text += "<color=black>(Private) " + message;
            }
            else
            {
                msgArea.text += "\r\n" + "<color=black>(Private) " + message;
            }
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subcribed");
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        msgArea.text = "";
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



    //friend list implementation

    ArrayList friendList = new ArrayList();
    //string[] requestList = new string[100];
    //int friends = 0;
    public TMP_InputField addFriendInput;
    [SerializeField]
    private GameObject addFriendButton;
    [SerializeField]
    private GameObject friendObjectButton;
    [SerializeField]
    private Transform contentContainer;

    public void AddFriends(string[] friends) {

    }

    //add friend by userID and put into friend list while sending friend request
    public void addFriend() {
        GameObject friend = Instantiate(friendObjectButton);
        friend.transform.SetParent(contentContainer);
        friend.transform.localScale = Vector2.one;
        Debug.Log(addFriendInput.text);
        //friend.GetComponentInChildren<Text>().text = addFriendInput.text;
        chatClient.SendPrivateMessage(addFriendInput.text, username + " has sent a friend request! Add them back"
        + " by typing the username and pressing add friend in the friend list menu.</color>");

    }

    //clicking on friend allows you to message the friend
    public void sendFriendMessage(GameObject button) {
        Debug.Log(button.GetComponentInChildren<Text>().text);
        toInput.text = button.GetComponentInChildren<Text>().text;
    }


}