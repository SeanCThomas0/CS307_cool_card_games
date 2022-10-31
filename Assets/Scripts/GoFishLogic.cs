using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class GoFishLogic : MonoBehaviourPun
{
    public GameObject cardDealerController; // to get CardDealer
    private CardDealer cardDealer;

    public List<GameObject> pool;
    private Queue<GameObject> queue;

    int numOfPlayers;
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;

    public bool playerTwoIsBot;
    public bool playerThreeIsBot;
    public bool playerFourIsBot;

    public GameObject guideText;
    public GameObject quitButton;
    public GameObject exitButton;
    public GameObject botDiffButtonText;
    

    private enum gameStates
    {
        PICK_FROM_POOL,
        PICK_NUM_TO_REQEUST,
        OUT_OF_CARDS,
        PICK_PLAYER_TO_REQUEST,
        BOT_PLAYING,
        BOT_REQUESTING,
        END_GAME,
        DEMO_POOL_RAPID_FIRE
    }

    private enum gameAlerts
    {
        NONE,
        PICK_PLAYER,
        PICK_NUM,
        PICK_POOL,
        SATISFY_REQUEST,
    }

    private gameStates gameState;
    private gameAlerts gameAlert;
    private GameObject turn;

    private GameObject requestingFrom;
    private int requestingNumValue;

    public bool diffiBot;
    private bool gaveToBot;

    private int indexInHand;

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;
    private int curUserWinCount = 0;
    private int curUserSetCount = 0;
    private bool updatedDatabase;

    // SEAN MULTIPLAYER VARS




    // Start is called before the first frame update
    void Start()
    {
        PhotonView photonView = PhotonView.Get(this);


        // get a randomized standard deck of cards
        cardDealer = cardDealerController.GetComponent<CardDealer>();
        pool = cardDealer.RandomCards(52);

        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].GetComponent<Card>().inPool = true;
        }

        // create queue of players and distribute cards
        numOfPlayers = 4;
        queue = new Queue<GameObject>();
        switch (numOfPlayers)
        {
            case 2:
                queue.Enqueue(playerOne);
                DistributeCards(playerOne, 5);
                DisplayHand(playerOne);

                queue.Enqueue(playerTwo);
                playerTwoIsBot = true;
                DistributeCards(playerTwo, 5);
                DisplayHand(playerTwo);

                playerThree.SetActive(false);
                playerFour.SetActive(false);

                break;
            case 3:
                queue.Enqueue(playerOne);
                DistributeCards(playerOne, 5);
                DisplayHand(playerOne);

                queue.Enqueue(playerTwo);
                playerTwoIsBot = true;
                DistributeCards(playerTwo, 5);
                DisplayHand(playerTwo);

                queue.Enqueue(playerThree);
                playerThreeIsBot = true;
                DistributeCards(playerThree, 5);
                DisplayHand(playerThree);

                playerFour.SetActive(false);

                break;
            case 4:
                queue.Enqueue(playerOne);
                DistributeCards(playerOne, 5);
                DisplayHand(playerOne);

                queue.Enqueue(playerTwo);
                playerTwoIsBot = true;
                DistributeCards(playerTwo, 5);
                DisplayHand(playerTwo);

                queue.Enqueue(playerThree);
                playerThreeIsBot = true;
                DistributeCards(playerThree, 5);
                DisplayHand(playerThree);

                queue.Enqueue(playerFour);
                playerFourIsBot = true;
                DistributeCards(playerFour, 5);
                DisplayHand(playerFour);
                break;
        }

        // set initial states
        turn = queue.Dequeue();
        queue.Enqueue(turn);

        gameState = gameStates.PICK_PLAYER_TO_REQUEST;
         //gameState = gameStates.DEMO_POOL_RAPID_FIRE; // to demonstrate win conditions
        gameAlert = gameAlerts.NONE;

        gaveToBot = false;
        diffiBot = false;

        exitButton.SetActive(false);

        indexInHand = -1;

        // display pool
        photonView.RPC("updatePool",RpcTarget.Others,pool);
        DisplayPool();

        // Set Firebase authenticator and database reference
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Retrieve current user game statistics
        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("win_count = 0");
            }
            if (task.IsFaulted)
            {
                Debug.Log("win_count = 0");
            }
            else
            {
                curUserWinCount = Int32.Parse(task.Result.Value.ToString());
                Debug.Log("win_count = " + curUserWinCount);
            }
        });

        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/set_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("set_count = 0");
            }
            if (task.IsFaulted)
            {
                Debug.Log("set_count = 0");
            }
            else
            {
                curUserSetCount = Int32.Parse(task.Result.Value.ToString());
                Debug.Log("set_count = " + curUserSetCount);
            }
        });

        updatedDatabase = false;
    }

    // Update is called once per frame
    void Update()
    {
        // updates text
        bool containBotRequest = false;

        string requestingNumValueAsString;
        switch (requestingNumValue)
        {
            case 1:
                requestingNumValueAsString = "Ace";
                break;
            case 11:
                requestingNumValueAsString = "Jack";
                break;
            case 12:
                requestingNumValueAsString = "Queen";
                break;
            case 13:
                requestingNumValueAsString = "King";
                break;
            default:
                requestingNumValueAsString = requestingNumValue.ToString();
                break;
        }

        if (turn.GetComponent<Player>().hand.Count == 0)
        {
            gameState = gameStates.OUT_OF_CARDS;
        }

        if (turn.GetComponent<Player>().hand.Count == 0 && pool.Count == 0)
        {
            gameState = gameStates.END_GAME;
        }

        switch (gameState)
        {
            case gameStates.PICK_PLAYER_TO_REQUEST:
                if (gameAlert == gameAlerts.PICK_PLAYER)
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Please select a player other than yourself.";
                }
                else
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a player to request a rank from.";
                }
                break;
            case gameStates.PICK_NUM_TO_REQEUST:
                if (gameAlert == gameAlerts.PICK_NUM)
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Please select a rank from your own deck.";
                }
                else
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a rank from your deck to request from " + requestingFrom.GetComponent<Player>().userID + ".";
                }

                break;
            case gameStates.PICK_FROM_POOL:
                if (pool.Count <= 0)
                {
                    gameState = gameStates.END_GAME;
                    break;
                }

                if (gameAlert == gameAlerts.PICK_POOL)
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Please select a card from the pool.";
                }
                else
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = requestingFrom.GetComponent<Player>().userID + " told you to Go Fish. Pick from the pool.";
                }

                if ((turn == playerTwo && playerTwoIsBot) || (turn == playerThree && playerThreeIsBot) || (turn == playerFour && playerFourIsBot))
                {
                    PickFromPool(pool[UnityEngine.Random.Range(0, pool.Count)]);
                    DetermineNextPlayer();
                }

                break;
            case gameStates.OUT_OF_CARDS:
                if (pool.Count <= 0)
                {
                    gameState = gameStates.END_GAME;
                    break;
                }

                if (gameAlert == gameAlerts.PICK_POOL)
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Please select a card from the pool.";
                }
                else
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "You're out of cards. Pick from the pool.";
                }

                if ((turn == playerTwo && playerTwoIsBot) || (turn == playerThree && playerThreeIsBot) || (turn == playerFour && playerFourIsBot))
                {
                    PickFromPool(pool[UnityEngine.Random.Range(0, pool.Count)]);
                    gameState = gameStates.BOT_PLAYING;
                }

                break;
            case gameStates.BOT_PLAYING:
                guideText.GetComponent<TMPro.TextMeshProUGUI>().text = turn.GetComponent<Player>().userID + " is playing.";
                BotPlay();
                break;
            case gameStates.BOT_REQUESTING:
                if (gameAlert == gameAlerts.SATISFY_REQUEST)
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Please select all cards of rank " + requestingNumValueAsString + " from your own deck or select Go Fish.";
                }
                else
                {
                    guideText.GetComponent<TMPro.TextMeshProUGUI>().text = turn.GetComponent<Player>().userID + " is requesting rank " + requestingNumValueAsString + " from " + requestingFrom.GetComponent<Player>().userID + ".";
                }

                if ((requestingFrom == playerTwo && playerTwoIsBot) || (requestingFrom == playerThree && playerThreeIsBot) || (requestingFrom == playerFour && playerFourIsBot))
                {
                    if (RequestAllCards())
                    {
                        gameState = gameStates.BOT_REQUESTING;
                    }
                    else
                    {
                        gameState = gameStates.PICK_FROM_POOL;
                    }
                }
                else
                {
                    containBotRequest = false;
                    for (int i = 0; i < requestingFrom.GetComponent<Player>().hand.Count; i++)
                    {
                        if (requestingFrom.GetComponent<Player>().hand[i].GetComponent<Card>().numValue == requestingNumValue)
                        {
                            containBotRequest = true;
                        }
                    }

                    if (!containBotRequest && gaveToBot)
                    {
                        gameState = gameStates.BOT_PLAYING;
                        gaveToBot = false;
                    }
                }

                break;
            case gameStates.END_GAME:
                GameObject[] players = queue.ToArray();
                GameObject maxPlayer = players[0];

                List<GameObject> winningPlayers = new List<GameObject>();

                for (int i = 1; i < players.Length; i++)
                {
                    //Updates current user's set count
                    if (players[i].GetComponent<Player>().userID.Equals("1") && !updatedDatabase)
                    {
                        curUserSetCount += players[i].GetComponent<Player>().numOfSetsOfFour;
                        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/set_count").SetValueAsync(curUserSetCount);
                    }

                    if (players[i].GetComponent<Player>().numOfSetsOfFour > maxPlayer.GetComponent<Player>().numOfSetsOfFour)
                    {
                        maxPlayer = players[i];
                    }
                }

                for (int i = 1; i < players.Length; i++)
                {
                    if (players[i].GetComponent<Player>().numOfSetsOfFour >= maxPlayer.GetComponent<Player>().numOfSetsOfFour)
                    {
                        //updates current user's win count
                        if (players[i].GetComponent<Player>().userID.Equals("1") && !updatedDatabase)
                        {
                            databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("game_statistics/go_fish/win_count").SetValueAsync(++curUserWinCount);
                        }
                        winningPlayers.Add(players[i]);
                    }
                }
                //database has been updated and doesn't need to be updated again this game
                updatedDatabase = true;

                switch (winningPlayers.Count)
                {
                    case 0:
                        guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "The pool is empty. Game over. Nobody wins!";
                        break;
                    case 1:
                        guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "The pool is empty. Game over. " + winningPlayers[0].GetComponent<Player>().userID + " wins with " + winningPlayers[0].GetComponent<Player>().numOfSetsOfFour + " sets.";
                        break;
                    case 2:
                        guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "The pool is empty. Game over. " + winningPlayers[0].GetComponent<Player>().userID + " and " + winningPlayers[1].GetComponent<Player>().userID + " tie with " + winningPlayers[0].GetComponent<Player>().numOfSetsOfFour + " sets.";
                        break;
                    case 3:
                        guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "The pool is empty. Game over. " + winningPlayers[0].GetComponent<Player>().userID + ", " + winningPlayers[1].GetComponent<Player>().userID + ", and " + winningPlayers[2].GetComponent<Player>().userID + " tie with " + winningPlayers[0].GetComponent<Player>().numOfSetsOfFour + " sets.";
                        break;
                    case 4:
                        guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "The pool is empty. Game over. " + winningPlayers[0].GetComponent<Player>().userID + ", " + winningPlayers[1].GetComponent<Player>().userID + ", " + winningPlayers[2].GetComponent<Player>().userID + ", and " + winningPlayers[2].GetComponent<Player>().userID + " tie with " + winningPlayers[0].GetComponent<Player>().numOfSetsOfFour + " sets.";
                        break;
                }

                quitButton.SetActive(false);
                exitButton.SetActive(true);

                break;
        }

        // checks for clicking, detects what is clicked
        if (Input.GetKeyDown(KeyCode.RightArrow) && (turn == playerOne || requestingFrom == playerOne))
        {
            if (indexInHand + 1 < playerOne.GetComponent<Player>().hand.Count)
            {
                indexInHand++;
            }
            else
            {
                playerOne.GetComponent<Player>().hand[playerOne.GetComponent<Player>().hand.Count - 1].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.white;
                indexInHand = 0;
            }

            if (indexInHand - 1 >= 0)
            {
                playerOne.GetComponent<Player>().hand[indexInHand - 1].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.white;
            }

            playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && (turn == playerOne || requestingFrom == playerOne))
        {
            if (indexInHand - 1 >= 0)
            {
                indexInHand--;
            }
            else
            {
                playerOne.GetComponent<Player>().hand[0].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.white;
                indexInHand = playerOne.GetComponent<Player>().hand.Count - 1;
            }

            if (indexInHand + 1 < playerOne.GetComponent<Player>().hand.Count)
            {
                playerOne.GetComponent<Player>().hand[indexInHand + 1].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.white;
            }

            playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (turn == playerOne || requestingFrom == playerOne))
        {
            switch (gameState)
            {
                case gameStates.PICK_NUM_TO_REQEUST:
                    if (playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>() != null && cardDealer.ContainsCard(playerOne.GetComponent<Player>().hand, playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().numValue, playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().suitValue))
                    {
                        requestingNumValue = playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().numValue;
                        gameAlert = gameAlerts.NONE;

                        if (RequestAllCards())
                        {
                            gameState = gameStates.PICK_PLAYER_TO_REQUEST;
                        }
                        else
                        {
                            gameState = gameStates.PICK_FROM_POOL;
                        }
                    }
                    else
                    {
                        gameAlert = gameAlerts.PICK_NUM;
                    }
                    break;

                case gameStates.PICK_FROM_POOL:
                    // if (turn.GetComponent<Player>().hand[indexInHand].GetComponent<Card>() != null && turn.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().inPool)
                    // {
                    PickFromPool(pool[UnityEngine.Random.Range(0, pool.Count)]);
                    // PickFromPool(turn.GetComponent<Player>().hand[indexInHand]);
                    DetermineNextPlayer();
                    gameAlert = gameAlerts.NONE;
                    // }
                    // else
                    // {
                    //     gameAlert = gameAlerts.PICK_POOL;
                    // }
                    break;
                case gameStates.BOT_REQUESTING:
                    if (playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>() != null && cardDealer.ContainsCard(requestingFrom.GetComponent<Player>().hand, playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().numValue, playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().suitValue) && playerOne.GetComponent<Player>().hand[indexInHand].GetComponent<Card>().numValue == requestingNumValue && containBotRequest)
                    {
                        RequestCards(playerOne.GetComponent<Player>().hand[indexInHand]);
                        gaveToBot = true;
                        gameAlert = gameAlerts.NONE;
                    }
                    else if (!containBotRequest)
                    {
                        if (pool.Count <= 0)
                        {
                            gameState = gameStates.END_GAME;
                        }

                        PickFromPool(pool[UnityEngine.Random.Range(0, pool.Count)]);
                        DetermineNextPlayer();
                        gameAlert = gameAlerts.NONE;
                    }
                    else
                    {
                        gameAlert = gameAlerts.SATISFY_REQUEST;
                    }

                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider)
            {
                switch (gameState)
                {
                    case gameStates.PICK_PLAYER_TO_REQUEST:
                        if (hit.collider.gameObject.GetComponent<Player>() != null && hit.collider.gameObject.activeSelf && hit.collider.gameObject != turn)
                        {
                            requestingFrom = hit.collider.gameObject;
                            gameState = gameStates.PICK_NUM_TO_REQEUST;
                            gameAlert = gameAlerts.NONE;
                        }
                        else
                        {
                            gameAlert = gameAlerts.PICK_PLAYER;
                        }
                        break;

                    case gameStates.PICK_NUM_TO_REQEUST:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && cardDealer.ContainsCard(turn.GetComponent<Player>().hand, hit.collider.gameObject.GetComponent<Card>().numValue, hit.collider.gameObject.GetComponent<Card>().suitValue))
                        {
                            requestingNumValue = hit.collider.gameObject.GetComponent<Card>().numValue;
                            gameAlert = gameAlerts.NONE;

                            if (RequestAllCards())
                            {
                                gameState = gameStates.PICK_PLAYER_TO_REQUEST;
                            }
                            else
                            {
                                gameState = gameStates.PICK_FROM_POOL;
                            }
                        }
                        else
                        {
                            gameAlert = gameAlerts.PICK_NUM;
                        }
                        break;

                    case gameStates.PICK_FROM_POOL:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && hit.collider.GetComponent<Card>().inPool)
                        {
                            PickFromPool(hit.collider.gameObject);
                            DetermineNextPlayer();
                            gameAlert = gameAlerts.NONE;
                        }
                        else
                        {
                            gameAlert = gameAlerts.PICK_POOL;
                        }
                        break;

                    case gameStates.OUT_OF_CARDS:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && hit.collider.GetComponent<Card>().inPool)
                        {
                            PickFromPool(hit.collider.gameObject);
                            gameState = gameStates.PICK_NUM_TO_REQEUST;
                            gameAlert = gameAlerts.NONE;
                        }
                        else
                        {
                            gameAlert = gameAlerts.PICK_POOL;
                        }
                        break;

                    case gameStates.BOT_REQUESTING:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && cardDealer.ContainsCard(requestingFrom.GetComponent<Player>().hand, hit.collider.gameObject.GetComponent<Card>().numValue, hit.collider.gameObject.GetComponent<Card>().suitValue) && hit.collider.gameObject.GetComponent<Card>().numValue == requestingNumValue && containBotRequest)
                        {
                            RequestCards(hit.collider.gameObject);
                            gaveToBot = true;
                            gameAlert = gameAlerts.NONE;
                        }
                        else if (!containBotRequest && hit.collider.gameObject.name.Equals("Fish"))
                        {
                            if (pool.Count <= 0)
                            {
                                gameState = gameStates.END_GAME;
                            }

                            PickFromPool(pool[UnityEngine.Random.Range(0, pool.Count)]);
                            DetermineNextPlayer();
                            gameAlert = gameAlerts.NONE;
                        }
                        else
                        {
                            gameAlert = gameAlerts.SATISFY_REQUEST;
                        }

                        break;
                    case gameStates.DEMO_POOL_RAPID_FIRE:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && hit.collider.GetComponent<Card>().inPool)
                        {
                            PickFromPool(hit.collider.gameObject);
                        }

                        if (pool.Count <= 0)
                        {
                            gameState = gameStates.END_GAME;
                        }
                        break;
                }

                Debug.Log("Collided: " + hit.collider.name);
            }
        }
    }

    private void BotPlay()
    {
        int newRequestingNumValue = turn.GetComponent<Player>().hand[UnityEngine.Random.Range(0, turn.GetComponent<Player>().hand.Count)].GetComponent<Card>().numValue;

        int playerIndex = UnityEngine.Random.Range(1, numOfPlayers + 1);
        while (playerIndex.ToString().Equals(turn.GetComponent<Player>().userID))
        {
            playerIndex = UnityEngine.Random.Range(1, numOfPlayers + 1);
        }

        switch (playerIndex)
        {
            case 1:
                requestingFrom = playerOne;
                break;
            case 2:
                requestingFrom = playerTwo;
                break;
            case 3:
                requestingFrom = playerThree;
                break;
            case 4:
                requestingFrom = playerFour;
                break;
        }

        if (diffiBot)
        {
            int[] numOfNumValues = new int[13];

            for (int i = 0; i < turn.GetComponent<Player>().hand.Count; i++)
            {
                numOfNumValues[turn.GetComponent<Player>().hand[i].GetComponent<Card>().numValue - 1]++;
            }

            newRequestingNumValue = numOfNumValues[0];
            for (int i = 1; i < numOfNumValues.Length; i++)
            {
                if (numOfNumValues[i] > newRequestingNumValue && numOfNumValues[i] != requestingNumValue)
                {
                    newRequestingNumValue = i + 1;
                }
            }
        }
        else
        {
            newRequestingNumValue = turn.GetComponent<Player>().hand[UnityEngine.Random.Range(0, turn.GetComponent<Player>().hand.Count)].GetComponent<Card>().numValue;
        }

        requestingNumValue = newRequestingNumValue;

        Debug.Log(turn.GetComponent<Player>().userID + " requesting " + requestingNumValue + " from " + requestingFrom.GetComponent<Player>().userID);

        gameState = gameStates.BOT_REQUESTING;
    }

    private void DetermineNextPlayer()
    {
        turn = queue.Dequeue();
        queue.Enqueue(turn);

        requestingFrom = null;
        requestingNumValue = -1;

        if ((turn == playerTwo && playerTwoIsBot) || (turn == playerThree && playerThreeIsBot) || (turn == playerFour && playerFourIsBot))
        {
            gameState = gameStates.BOT_PLAYING;
        }
        else
        {
            gameState = gameStates.PICK_PLAYER_TO_REQUEST;
        }
    }

    // add cards to hand and remove from pool
    private void DistributeCards(GameObject player, int count)
    {
        if (pool.Count < count)
        {
            Debug.Log("Cannot distribute more cards than are available in the pool.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            player.GetComponent<Player>().AddToHand(pool[i]);
            pool[i].GetComponent<Card>().inPool = false;
            pool.RemoveAt(i);
        }
    }

    // remove cards from one player and give to another player
    // returns true if the player had the cards
    // false if the player did not have any cards
    private bool RequestAllCards()
    {
        List<GameObject> removed = requestingFrom.GetComponent<Player>().RemoveAllNumFromHand(requestingNumValue);
        for (int i = 0; i < removed.Count; i++)
        {
            removed[i].GetComponent<Card>().inPool = false;
            turn.GetComponent<Player>().AddToHand(removed[i]);
        }

        if (removed.Count > 0)
        {
            DisplayHand(requestingFrom);
            DisplayHand(turn);
            return true;
        }
        else
        {
            return false;
        }
    }

    // remove cards from one player and give to another player
    // returns true if the player had the cards
    // false if the player did not have any cards
    private bool RequestCards(GameObject card)
    {
        bool removed = requestingFrom.GetComponent<Player>().RemoveFromHand(card);
        turn.GetComponent<Player>().AddToHand(card);

        if (removed)
        {
            card.GetComponent<Card>().inPool = false;
            DisplayHand(requestingFrom);
            DisplayHand(turn);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisplayHand(GameObject player)
    {
        CheckForFour(player);

        cardDealer.SortCards(player.GetComponent<Player>().hand);

        float x = player.GetComponent<Player>().xStartPos;
        float z = 0;

        for (int i = 0; i < player.GetComponent<Player>().hand.Count; i++)
        {
            player.GetComponent<Player>().hand[i].transform.position = new Vector3(x, player.GetComponent<Player>().yStartPos, z);

            if (player == playerOne)
            {
                cardDealer.SetSprite(player.GetComponent<Player>().hand[i]);
            }
            else
            {
                cardDealer.ShowBackKeepValue(player.GetComponent<Player>().hand[i], Card.backColor.BLUE, Card.backDesign.OUTLINE_SIMPLE_PATTERN);
            }

            player.GetComponent<Player>().hand[i].GetComponent<Card>().GetComponent<SpriteRenderer>().color = Color.white;
            player.GetComponent<Player>().hand[i].SetActive(true);

            x = x + 0.35f;
            z = z - 0.1f;
        }
    }

    private void DisplayPool()
    {
        System.Random randomGenerator = new System.Random();

        for (int i = 0; i < pool.Count; i++)
        {
            int negative = 1;

            int randomToNegative = UnityEngine.Random.Range(0, 2);
            if (randomToNegative == 0)
            {
                negative = -1;
            }
            float xOffset = 6 + (float)randomGenerator.NextDouble() * 2 * negative;

            randomToNegative = UnityEngine.Random.Range(0, 2);
            if (randomToNegative == 0)
            {
                negative = -1;
            }
            else
            {
                negative = 1;
            }
            float yOffset = 1 + (float)randomGenerator.NextDouble() * 2 * negative;

            randomToNegative = UnityEngine.Random.Range(0, 2);
            if (randomToNegative == 0)
            {
                negative = -1;
            }
            else
            {
                negative = 1;
            }
            float zOffset = (float)randomGenerator.NextDouble() * negative;

            pool[i].transform.position = new Vector3(0 + xOffset, 0 + yOffset, 0 + zOffset);
            cardDealer.ShowBackKeepValue(pool[i], Card.backColor.BLUE, Card.backDesign.OUTLINE_SIMPLE_PATTERN);
            pool[i].SetActive(true);
        }
    }

    private void PickFromPool(GameObject card)
    {
        cardDealer.SetSprite(card);

        turn.GetComponent<Player>().AddToHand(card);
        DisplayHand(turn);

        card.GetComponent<Card>().inPool = false;
        pool.Remove(card);
    }

    private void CheckForFour(GameObject player)
    {
        int[] numOfNumValues = new int[13];

        for (int i = 0; i < player.GetComponent<Player>().hand.Count; i++)
        {
            numOfNumValues[player.GetComponent<Player>().hand[i].GetComponent<Card>().numValue - 1]++;
        }

        for (int i = 0; i < numOfNumValues.Length; i++)
        {
            if (numOfNumValues[i] >= 4)
            {
                List<GameObject> removed = player.GetComponent<Player>().RemoveAllNumFromHand(i + 1);

                if (removed.Count == 4)
                {
                    for (int j = 0; j < removed.Count; j++)
                    {
                        removed[j].SetActive(false);
                    }
                    player.GetComponent<Player>().numOfSetsOfFour++;

                    if (player.GetComponent<Player>().numOfSetsOfFour == 1)
                    {
                        player.GetComponent<Player>().text.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Player>().numOfSetsOfFour + " set of four";
                    }
                    else
                    {
                        player.GetComponent<Player>().text.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<Player>().numOfSetsOfFour + " sets of four";
                    }
                }
            }
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void ChangeBotDIfficulty()
    {
        diffiBot = !diffiBot;

        if (diffiBot)
        {
            botDiffButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Bot Mode: Hard";
        }
        else
        {
            botDiffButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = "Bot Mode: Easy";
        }
    }
}
