using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFishLogic : MonoBehaviour
{
    public GameObject cardDealerController; // to get CardDealer

    public int mostRecentNumValue;
    public Card.suit mostRecentSuitValue;

    private CardDealer cardDealer;

    private List<GameObject> pool;
    
    // private Queue<Player> queue;
    private Player playerBot;
    private Player playerSingle;

    // Start is called before the first frame update
    void Start()
    {
        // set up deck of cards
        cardDealer = cardDealerController.GetComponent<CardDealer>();

        // get a randomized standard deck of cards
        pool = cardDealer.RandomCards(52);
        
        // create players
        playerBot = new Player("0");
        playerSingle = new Player("1");

        // create queue of players
        // queue = new Queue<Player>();
        // queue.Enqueue(playerSingle);
        // queue.Enqueue(playerBot);

        // distribute cards for beginning of game
        DistributeCards(playerSingle, 5);
        DisplayHand(playerSingle, -8, -3.5f);

        DistributeCards(playerBot, 5);
        DisplayHand(playerBot, -8, 3.5f);
        
        DisplayPool();
    }

    // Update is called once per frame
    void Update()
    {
        // checks for clicking, detects what is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider)
            {
                mostRecentNumValue = hit.collider.gameObject.GetComponent<Card>().numValue;
                mostRecentSuitValue = hit.collider.gameObject.GetComponent<Card>().suitValue;

                TakeTurn(hit.collider.gameObject);

                // Debug.Log("newCollided: " + hit.collider.gameObject.GetComponent<Card>().numValue + ", " + hit.collider.gameObject.GetComponent<Card>().suitValue);
            }
        }
    }

    public class Player {
        private string userID;
        private List<GameObject> hand;

        private int[] numOfValues;

        public Player(string userID) {
            this.userID = userID;
            this.hand = new List<GameObject>();
            this.numOfValues = new int[13];
        }

        public string GetUserID() {
            return userID;
        }

        public void AddToHand(GameObject card) {
            hand.Add(card);
        }

        public void RemoveFromHand(GameObject card) {
            hand.Remove(card);
        }

        public int RemoveAllNumFromHand(int numValue) {
            int numRemoved = 0;

            for (int i = 0; i < hand.Count; i++) {
                if (hand[i].GetComponent<Card>().numValue == numValue) {
                    hand.RemoveAt(i);
                    numRemoved++;
                    i--;
                }
            }
            // numOfValues[numValue] = numOfValues[numValue] - numRemoved;
            return numRemoved;
        }

        public int RemoveAllSuitFromHand(Card.suit suit)
        {
            int numRemoved = 0;

            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].GetComponent<Card>().suitValue == suit)
                {
                    hand.RemoveAt(i);
                    numRemoved++;
                    i--;
                }
            }
            return numRemoved;
        }

        public List<GameObject> GetHand() {
            return hand;
        }
    }

    // add cards to hand and remove from pool
    private void DistributeCards(Player player, int count) {
        if (pool.Count < count) {
            Debug.Log("Cannot distribute more cards than are available in the pool.");
            return;
        }

        for (int i = 0; i < count; i++) {        
            player.AddToHand(pool[i]);
            pool.RemoveAt(i);
        }
    }

    // remove cards from one player and give to another player
    // returns true if the player had the cards
    // false if the player did not have any cards
    private bool RequestCards(Player from, Player to, GameObject card) {
        int numRemoved = from.RemoveAllNumFromHand(card.GetComponent<Card>().numValue);
        for (int i = 0; i < numRemoved; i++) {
            to.AddToHand(card);
        }

        if (numRemoved > 0) {
            return true;
        } else {
            return false;
        }
    }

    // public List<Card> determineFourOfAKind(Player player) {
    //     for (int i = 0; i < player.getNumOfValues().Length; i++) {
    //         if (player.getNumOfValues()[i] >= 4) {
    //             player.addCardOfFour();
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    private void TakeTurn(GameObject card) {
        Debug.Log("numValue: " + card.GetComponent<Card>().numValue + ", suitValue: " + card.GetComponent<Card>().suitValue);

        // Player currentPlayer = queue.Dequeue();
        // queue.Enqueue(currentPlayer);

        RequestCards(playerBot, playerSingle, card);
    }

    private void DisplayHand(Player player, float xStart, float yStart) {
        float x = xStart;
        float y = yStart;
        float z = 0;

        for (int i = 0; i < player.GetHand().Count; i++) {
            player.GetHand()[i].transform.position = new Vector3(x, y, z);
            player.GetHand()[i].SetActive(true);
            
            x = x + 0.35f;
            z = z - 0.1f;
        }
    }

    private void DisplayPool() {
        System.Random randomGenerator = new System.Random();

        for (int i = 0; i < pool.Count; i++) {
                int negative = 1;

                int randomToNegative = UnityEngine.Random.Range(0, 2);
                if (randomToNegative == 0)
                {
                    negative = -1;
                }
                float xOffset = (float)randomGenerator.NextDouble() * 3 * negative;

                randomToNegative = UnityEngine.Random.Range(0, 2);
                if (randomToNegative == 0)
                {
                    negative = -1;
                } else {
                    negative = 1;
                }
                float yOffset = (float)randomGenerator.NextDouble() * negative;

                randomToNegative = UnityEngine.Random.Range(0, 2);
                if (randomToNegative == 0)
                {
                    negative = -1;
                } else
                {
                    negative = 1;
                }
                float zOffset = (float)randomGenerator.NextDouble() * negative;

                pool[i].transform.position = new Vector3(0 + xOffset, 0 + yOffset, 0 + zOffset);
                cardDealer.ShowBacksKeepValues(pool[i], CardDealer.backColor.BLUE, CardDealer.backDesign.OUTLINE_SIMPLE_PATTERN);
                pool[i].SetActive(true);
        }
    }
}
