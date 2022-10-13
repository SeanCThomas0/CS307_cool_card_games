using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoFishLogic : MonoBehaviour
{
    public GameObject cardDealerController; // to get CardDealer
    private CardDealer cardDealer;

    private List<GameObject> pool;
    private Queue<GameObject> queue;

    int numOfPlayers;
    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;

    public bool playerTwoIsBot;

    public GameObject guideText;

    private enum gameStates
    {
        PICK_FROM_POOL,
        PICK_NUM_TO_REQEUST,
        PICK_PLAYER_TO_REQUEST,
        BOT_PLAYING,
        BOT_REQUESTING
    }

    private gameStates gameState;
    private GameObject turn;

    private GameObject requestingFrom;
    private int requestingNumValue;

    private bool gaveToBot;

    // Start is called before the first frame update
    void Start()
    {
        // get a randomized standard deck of cards
        cardDealer = cardDealerController.GetComponent<CardDealer>();
        pool = cardDealer.RandomCards(52);

        // create queue of players and distribute cards
        numOfPlayers = 2;
        queue = new Queue<GameObject>();
        switch (numOfPlayers)
        {
            case 2:
                queue.Enqueue(playerOne);
                playerOne.GetComponent<Player>().active = true;
                DistributeCards(playerOne, 5);
                DisplayHand(playerOne, false);

                queue.Enqueue(playerTwo);
                playerTwo.GetComponent<Player>().active = true;
                playerTwoIsBot = true;
                DistributeCards(playerTwo, 5);
                DisplayHand(playerTwo, false);

                playerThree.SetActive(false);
                playerFour.SetActive(false);

                break;
            case 3:
                queue.Enqueue(playerOne);
                playerOne.GetComponent<Player>().active = true;
                DistributeCards(playerOne, 5);
                DisplayHand(playerOne, true);

                queue.Enqueue(playerTwo);
                playerTwo.GetComponent<Player>().active = true;
                DistributeCards(playerTwo, 5);

                queue.Enqueue(playerThree);
                playerThree.GetComponent<Player>().active = true;
                DistributeCards(playerThree, 5);

                playerFour.SetActive(false);

                break;
            case 4:
                queue.Enqueue(playerOne);
                playerOne.GetComponent<Player>().active = true;
                DistributeCards(playerOne, 5);
                DisplayHand(playerOne, true);

                queue.Enqueue(playerTwo);
                playerTwo.GetComponent<Player>().active = true;
                DistributeCards(playerTwo, 5);

                queue.Enqueue(playerThree);
                playerThree.GetComponent<Player>().active = true;
                DistributeCards(playerThree, 5);

                queue.Enqueue(playerFour);
                playerFour.GetComponent<Player>().active = true;
                DistributeCards(playerFour, 5);
                break;
        }

        // set initial states
        turn = queue.Dequeue();
        queue.Enqueue(turn);
        gameState = gameStates.PICK_PLAYER_TO_REQUEST;
        gaveToBot = false;

        // display pool
        DisplayPool();
    }

    // Update is called once per frame
    void Update()
    {
        // updates text
        bool containBotRequest = false;

        switch (gameState)
        {
            case gameStates.PICK_PLAYER_TO_REQUEST:
                guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a player to request a rank from.";
                break;
            case gameStates.PICK_NUM_TO_REQEUST:
                guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a rank from your deck to request from " + requestingFrom.GetComponent<Player>().userID + ".";
                break;
            case gameStates.PICK_FROM_POOL:
                guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player " + requestingFrom.GetComponent<Player>().userID + " told you to Go Fish. Pick from the pool.";
                break;
            case gameStates.BOT_PLAYING:
                guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player 2 (bot) is playing.";
                BotPlay();
                break;
            case gameStates.BOT_REQUESTING:
                guideText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player 2 (bot) is requesting " + requestingNumValue + " from player " + requestingFrom.GetComponent<Player>().userID + ".";

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

                break;
        }

        // checks for clicking, detects what is clicked
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
                        if (hit.collider.gameObject.GetComponent<Player>() != null && hit.collider.gameObject.GetComponent<Player>().active == true && hit.collider.gameObject != turn)
                        {
                            Debug.Log("Player " + hit.collider.gameObject.GetComponent<Player>().userID + " clicked.");
                            requestingFrom = hit.collider.gameObject;
                            gameState = gameStates.PICK_NUM_TO_REQEUST;
                        }
                        break;

                    case gameStates.PICK_NUM_TO_REQEUST:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && cardDealer.ContainsCard(turn.GetComponent<Player>().hand, hit.collider.gameObject.GetComponent<Card>().numValue, hit.collider.gameObject.GetComponent<Card>().suitValue) && hit.collider.gameObject.GetComponent<Card>().showingFront)
                        {
                            Debug.Log("Card: " + hit.collider.gameObject.GetComponent<Card>().numValue + ", " + hit.collider.gameObject.GetComponent<Card>().suitValue + " clicked.");
                            requestingNumValue = hit.collider.gameObject.GetComponent<Card>().numValue;

                            if (RequestAllCards())
                            {
                                Debug.Log("Hooray");
                            }
                            else
                            {
                                gameState = gameStates.PICK_FROM_POOL;
                            }
                        }
                        break;

                    case gameStates.PICK_FROM_POOL:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && !hit.collider.gameObject.GetComponent<Card>().showingFront)
                        {
                            Debug.Log("PoolCard: " + hit.collider.gameObject.GetComponent<Card>().numValue + ", " + hit.collider.gameObject.GetComponent<Card>().suitValue + " clicked.");
                            PickFromPool(hit.collider.gameObject);
                            DetermineNextPlayer();
                        }
                        break;

                    case gameStates.BOT_REQUESTING:
                        if (hit.collider.gameObject.GetComponent<Card>() != null && cardDealer.ContainsCard(requestingFrom.GetComponent<Player>().hand, hit.collider.gameObject.GetComponent<Card>().numValue, hit.collider.gameObject.GetComponent<Card>().suitValue) && hit.collider.gameObject.GetComponent<Card>().showingFront && hit.collider.gameObject.GetComponent<Card>().numValue == requestingNumValue && containBotRequest)
                        {
                            RequestCards(hit.collider.gameObject);
                            gaveToBot = true;
                        }
                        else if (!containBotRequest && hit.collider.gameObject.name == "Fish")
                        {
                            PickFromPool(pool[UnityEngine.Random.Range(0, pool.Count)]);
                            DetermineNextPlayer();
                        }

                        break;
                }

                Debug.Log("Collided: " + hit.collider.name);
            }
        }
    }

    private void BotPlay()
    {
        int playerIndex = UnityEngine.Random.Range(1, numOfPlayers + 1);
        while (playerIndex == 2)
        {
            playerIndex = UnityEngine.Random.Range(1, numOfPlayers + 1);
        }

        switch (playerIndex)
        {
            case 1:
                requestingFrom = playerOne;
                break;
            case 3:
                requestingFrom = playerThree;
                break;
            case 4:
                requestingFrom = playerFour;
                break;
        }

        requestingNumValue = turn.GetComponent<Player>().hand[UnityEngine.Random.Range(0, turn.GetComponent<Player>().hand.Count)].GetComponent<Card>().numValue;

        gameState = gameStates.BOT_REQUESTING;
    }

    private void DetermineNextPlayer()
    {
        turn = queue.Dequeue();
        queue.Enqueue(turn);

        requestingFrom = null;
        requestingNumValue = -1;

        if (turn == playerTwo && playerTwoIsBot)
        {
            gameState = gameStates.BOT_PLAYING;
            Debug.Log(gameState);
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
            turn.GetComponent<Player>().AddToHand(removed[i]);
        }

        if (removed.Count > 0)
        {
            DisplayHand(requestingFrom, false);
            DisplayHand(turn, false);
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
            DisplayHand(requestingFrom, false);
            DisplayHand(turn, false);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DisplayHand(GameObject player, bool back)
    {
        float x = player.GetComponent<Player>().xStartPos;
        float z = 0;

        for (int i = 0; i < player.GetComponent<Player>().hand.Count; i++)
        {
            player.GetComponent<Player>().hand[i].transform.position = new Vector3(x, player.GetComponent<Player>().yPos, z);

            if (back)
            {
                cardDealer.ShowBackKeepValue(player.GetComponent<Player>().hand[i], CardDealer.backColor.BLUE, CardDealer.backDesign.OUTLINE_SIMPLE_PATTERN);
            }

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
            cardDealer.ShowBackKeepValue(pool[i], CardDealer.backColor.BLUE, CardDealer.backDesign.OUTLINE_SIMPLE_PATTERN);
            pool[i].SetActive(true);
        }
    }

    private void PickFromPool(GameObject card)
    {
        cardDealer.SetSprite(card);
        turn.GetComponent<Player>().AddToHand(card);
        DisplayHand(turn, false);

        pool.Remove(card);
        DisplayPool();
    }
}
