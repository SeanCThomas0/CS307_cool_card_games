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

        // distribute cards for beginning of game
        DistributeCards(playerBot, 5);
        DistributeCards(playerSingle, 5);
    }

    // Update is called once per frame
    void Update()
    {
        // checks for clicking, detects what is clicked
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider)
                {
                    mostRecentNumValue = hit.collider.gameObject.GetComponent<Card>().numValue;
                    mostRecentSuitValue = hit.collider.gameObject.GetComponent<Card>().suitValue;

                    Debug.Log("newCollided: " + hit.collider.gameObject.GetComponent<Card>().numValue + ", " + hit.collider.gameObject.GetComponent<Card>().suitValue);
                }
            }
        }
    }

    public class Player {
        private string userID;
        private List<GameObject> hand;

        private int[] numOfValues;
        // private List<GameObject> cardsOfFour;

        public Player(string userID) {
            this.userID = userID;
            this.hand = new List<GameObject>();
            this.numOfValues = new int[13];
            // this.cardsOfFour = new List<GameObject>();
        }

        public string GetUserID() {
            return userID;
        }

        public void AddToHand(GameObject card) {
            hand.Add(card);
            // numOfValues[card.getNumValue()]++;
        }

        public void RemoveFromHand(GameObject card) {
            hand.Remove(card);
            // numOfValues[card.getNumValue()]--;
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
            // numOfValues[numValue] = numOfValues[numValue] - numRemoved;
            return numRemoved;
        }

        // public int[] getNumOfValues() {
        //     return numOfValues;
        // }

        public List<GameObject> GetHand() {
            return hand;
        }
    }

    // add cards to hand and remove from pool
    public void DistributeCards(Player player, int count) {
        if (pool.Count < count) {
            Debug.Log("Cannot distribute more cards than are available in the pool.");
            return;
        }

        for (int i = 0; i < count; i++) {        
            player.AddToHand(pool[i]);
            cardDealer.ToggleView(pool[i], true);
            pool.RemoveAt(i);
        }
    }

    // remove cards from one player and give to another player
    // returns true if the player had the cards
    // false if the player did not have any cards
    public bool RequestCards(Player from, Player to, GameObject card) {
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
}
