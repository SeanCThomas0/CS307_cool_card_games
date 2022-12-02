using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;


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

    [SerializeField]
    private GameObject friendSceneButton;

    [SerializeField]
    private GameObject inviteInformation;

    public static string username;
    //public static Action<PhotonStatus> OnStatusUpdated = delegate { };

    private UserPreferences.backgroundColor backgroundColor;
    public GameObject mainCam;

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;

    void OnEnable()
    {
        backgroundColor = (UserPreferences.backgroundColor)PlayerPrefs.GetInt("backgroundColor");

        switch (backgroundColor)
        {
            case UserPreferences.backgroundColor.GREEN:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(49, 121, 58, 255);
                break;
            case UserPreferences.backgroundColor.BLUE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(43, 100, 159, 255);
                break;
            case UserPreferences.backgroundColor.RED:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(222, 50, 73, 255);
                break;
            case UserPreferences.backgroundColor.ORANGE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(226, 119, 28, 255);
                break;
            case UserPreferences.backgroundColor.PURPLE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(120, 37, 217, 255);
                break;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.LogError("No AppID Provided");
            return;
        }

        //if in new scene, reconnect the chat client
        if (chatClient == null && !string.IsNullOrEmpty(username))
        {
            ConnectToServer();
            //OnConnected();
            //OnSubscribed(new string[] {"Chatroom"}, new bool[] {true});
        }

        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;



    }

    // Update is called once per frame
    void Update()
    {
        //must be called repeatedly to make chatClient function
        if (chatClient != null)
        {
            //chatClient.Service();
        }
    }

    public void ConnectToServer()
    {
        Debug.Log("Connecting");
        chatClient = new ChatClient(this);

        //Connect to Photon Server and set username to input field
        if (string.IsNullOrEmpty(username))
        {
            username = usernameInputField.text;
        }
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new Photon.Chat.AuthenticationValues(username));

        //call infinite loop to update last online value for current user
        StartCoroutine(updateLastOnline());


    }

    public void DisconnectFromServer()
    {
        Debug.Log("Leaving");
        chatClient.Disconnect(ChatDisconnectCause.None);
    }

    public void SendMsg()
    {

        //check to send message publicly or privately
        if (string.IsNullOrEmpty(toInput.text))
        {
            chatClient.PublishMessage("Chatroom", "<color=black>" + username + ": " + msgInput.text + "</color>");

        }
        else
        {
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
        chatClient.Unsubscribe(new string[] { PhotonNetwork.CurrentRoom.Name });
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
        
        Debug.Log("Leave");
    }

    public void DebugReturn(DebugLevel level, string message)
    {

    }

    public void OnDisconnected()
    {
        chatClient.Unsubscribe(new string[] { PhotonNetwork.CurrentRoom.Name });
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
        Debug.Log("Disconnected");
    }

    //when Connect is called, subscribe user to the chatroom
    public void OnConnected()
    {
        Debug.Log("Connected and Subscribed");
        chatClient.Subscribe(new string[] { "Chatroom" });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
        chatClient.PublishMessage("Chatroom", "<color=black>" + username + ": " + "joined" + "</color>");
    }

    //when button is pressed, join chatroom
    public void JoinChatRoom()
    {
        joinChatButton.SetActive(false);
        usernameInput.SetActive(false);
        chatRoomImage.SetActive(true);
        friendSceneButton.SetActive(true);
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

        //invite message is received
        if (message.ToString().Contains("has invited you to join ") && !message.ToString().Contains(username)) {
            inviteInformation.SetActive(true);
            return;
        }

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
        Debug.Log("OnStatusUpdate: " + user + "changed to " + status);

        GameObject[] friends = GameObject.FindGameObjectsWithTag("Friend");
        GameObject friendUpdate = friends[0];
        foreach (GameObject friend in friends)
        {
            if (user.Equals(friend.GetComponentInChildren<TMP_Text>().text))
            {
                friendUpdate = friend;
                break;
            }
        }
        if (status == 0)
        {
            friendUpdate.GetComponent<Image>().color = Color.red;
        }
        else
        {
            friendUpdate.GetComponent<Image>().color = Color.green;
        }
        //PhotonStatus newStatus = new PhotonStatus(user, status, (string) message);
        //OnStatusUpdated?.Invoke(newStatus);
    }

    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {

    }

    //check if message contains inappropriate words and replace them with asterisks
    public void checkMessage(TMP_InputField inputField)
    {
        string message = inputField.text;
        string[] bannedWords = new string[] { "ass", "bitch", "fuck", "hell", "sex", "shit" };

        foreach (string word in bannedWords)
        {
            string asterisk = "";


            if (message.Contains(word))
            {
                //determine length of asterisk string
                for (int i = 0; i < word.Length; i++)
                {
                    asterisk += "*";
                }
                message = message.Replace(word.Replace(" ", "").Replace(".", ""), asterisk);
            }
        }

        //set input field text to new message with asterisks
        inputField.text = message;

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
    bool removeFriendbool = false;

    [SerializeField]
    private TMP_Text lastOnlineInfoText;
    [SerializeField]
    private TMP_Text currentGameText;
    [SerializeField]
    private GameObject friendInformation;
    

    public void addPreviousFriends(string[] friends)
    {

    }

    //add friend by userID and put into friend list while sending friend request
    public void addFriend()
    {
        //check if friend is already added
        if (friendList.Contains(addFriendInput.text))
        {
            chatClient.SendPrivateMessage(username, addFriendInput.text + " is already a friend!</color>");
            return;
        }
        friendList.Add(addFriendInput.text);
        chatClient.AddFriends(new[] { addFriendInput.text });
        GameObject friend = Instantiate(friendObjectButton);
        friend.transform.SetParent(contentContainer);
        friend.SetActive(true);
        friend.transform.localScale = Vector2.one;
        Debug.Log(addFriendInput.text);
        friend.GetComponentInChildren<TMP_Text>().text = addFriendInput.text;
        chatClient.SendPrivateMessage(addFriendInput.text, username + " has sent a friend request! Add them back"
        + " by typing the username and pressing add friend in the friend list menu.</color>");

    }

    public void removeFriend(GameObject friendButton)
    {
        if (!removeFriendbool)
        {
            return;
        }

        Destroy(friendButton);
        friendList.Remove(friendButton.GetComponentInChildren<TMP_Text>().text);
        chatClient.AddFriends(new[] { friendButton.GetComponentInChildren<TMP_Text>().text });
        removeFriendbool = false;
    }

    public void removeFriendButtonClicked()
    {
        chatClient.SendPrivateMessage(username, "Select a friend to remove.");
        removeFriendbool = true;
    }

    //clicking on friend allows you to message the friend
    public void sendFriendMessage(GameObject button)
    {
        Debug.Log(button.GetComponentInChildren<TMP_Text>().text);
        toInput.text = button.GetComponentInChildren<TMP_Text>().text;
    }

    //change to main menu scene
    public void changeToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //display information like last online and what game a friend
    //is playing when clicking on a friend object
    public void displayFriendInformation(GameObject friend) {
        if(removeFriendbool) {
            return;
        }
        string username = friend.GetComponentInChildren<TMP_Text>().text;
        //if friend is offline change current game to N/A
        if (friend.GetComponentInChildren<Image>().color == Color.red) {
            databaseReference.Child("friend_info").Child(username).Child("current_game").SetValueAsync("N/A");
        }

        getFriendInfo(username);

        friendInformation.SetActive(true);
    
    }

    public async void getFriendInfo(string username) {
        
        string lastOnline  = "";
        string currentGame = "";

        //get last online value from database
        await databaseReference.Child("friend_info").Child(username).Child("last_online").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("get last online cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("get last online faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    lastOnline = work.Result.Value.ToString();
                    Debug.Log("from firebase (last_online) =" + lastOnline);
                }
                else
                {
                    lastOnline = "N/A";
                    Debug.Log("nothing in firebase (last_online), set to = N/A");
                }
            }
        });

        //get current game value from database
        await databaseReference.Child("friend_info").Child(username).Child("current_game").GetValueAsync().ContinueWith(work =>
        {
            if (work.IsCanceled)
            {
                Debug.Log("get current game cancelled");
            }
            if (work.IsFaulted)
            {
                Debug.Log("get current game faulted");
            }
            else
            {
                if (work.Result.Value != null)
                {
                    currentGame = work.Result.Value.ToString();
                    Debug.Log("from firebase (current_game) =" + currentGame);
                }
                else
                {
                    currentGame = "N/A";
                    Debug.Log("nothing in firebase (current_game), set to = N/A");
                }
            }
        });

        lastOnlineInfoText.text = lastOnline;
        currentGameText.text = currentGame;

        Debug.Log("Last Online: " + lastOnline);
        Debug.Log("Current Game: " + currentGame);

    }

    //close the friend information page
    public void closeButton() {
        friendInformation.SetActive(false);
    }

    //invite friend to join game lobby
    public void inviteFriend(GameObject friend) {
        chatClient.SendPrivateMessage(friend.GetComponentInChildren<TMP_Text>().text, username + " has invited you to join (game)</color>");
    }

    //accept invite
    public void acceptButton() {
        inviteInformation.SetActive(false);
    }

    //decline invite
    public void declineButton() {
        inviteInformation.SetActive(false);
    }

    IEnumerator updateLastOnline() {

        Debug.Log("update");

        //put last online info in database
        DateTime utc = System.DateTime.UtcNow;
        utc = utc.AddHours(-5);
        string lastOnline = utc.ToString("HH:mm dd MMMM, yyyy");
        databaseReference.Child("friend_info").Child(username).Child("last_online").SetValueAsync(lastOnline);

        yield return new WaitForSeconds(60);

        StartCoroutine(updateLastOnline());
        
    }

    //put current game playing in database
    public void updateCurrentGame(GameObject gameButton) {
        Debug.Log("update current Game");

        //put name of game scene into database
        string gameName = gameButton.GetComponentInChildren<TMP_Text>().text;
        
        if (gameName.Equals("Exit")) {
            gameName = "N/A";
        }

        databaseReference.Child("friend_info").Child(username).Child("current_game").SetValueAsync(gameName);

    }


}