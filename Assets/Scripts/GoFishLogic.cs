using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFishLogic : MonoBehaviour
{
    public GameObject Controller;

    private CardDealer cardDealer;
    private List<Card> pool;
    private Player playerBot;
    private Player playerSingle;

    // Start is called before the first frame update
    void Start()
    {
        // set up deck of cards
        cardDealer = Controller.GetComponent<CardDealer>();

        pool = cardDealer.RandomCards(52);

        // create players
        playerBot = new Player("0");
        playerSingle = new Player("1");

        // distribute cards for beginning of game
        distributeCards(playerBot, 5);
        distributeCards(playerSingle, 5);

        for (int i = 0; i < playerBot.getHand().Count; i++) {
            Debug.Log("Bot: " + playerBot.getHand()[i].getGameObject() + ", " + playerBot.getHand()[i].getNumValue() + ", " + playerBot.getHand()[i].getSuitValue());
        }

        for (int i = 0; i < playerSingle.getHand().Count; i++) {
            Debug.Log("Player: " + playerSingle.getHand()[i].getGameObject() + ", " + playerSingle.getHand()[i].getNumValue() + ", " + playerSingle.getHand()[i].getSuitValue());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Player {
        private string userID;
        private List<Card> hand;

        private int[] numOfValues;
        private List<Card> cardsOfFour;

        public Player(string userID) {
            this.userID = userID;
            this.hand = new List<Card>();
            this.numOfValues = new int[13];
            this.cardsOfFour = new List<Card>();
        }

        public string getUserID() {
            return userID;
        }

        public void addToHand(Card card) {
            hand.Add(card);
            // numOfValues[card.getNumValue()]++;
        }

        public void removeFromHand(Card card) {
            hand.Remove(card);
            // numOfValues[card.getNumValue()]--;
        }

        public int removeAllFromHand(int numValue) {
            int numRemoved = 0;

            for (int i = 0; i < hand.Count; i++) {
                if (hand[i].getNumValue() == numValue) {
                    hand.RemoveAt(i);
                    numRemoved++;
                    i--;
                }
            }
            // numOfValues[numValue] = numOfValues[numValue] - numRemoved;
            return numRemoved;
        }

        public int[] getNumOfValues() {
            return numOfValues;
        }

        public List<Card> getHand() {
            return hand;
        }
    }

    // add cards to hand and remove from pool
    public void distributeCards(Player player, int count) {
        if (pool.Count < count) {
            Debug.Log("Cannot distribute more cards than are available in the pool.");
            return;
        }

        for (int i = 0; i < count; i++) {        
            player.addToHand(pool[i]);
            pool.RemoveAt(i);
        }
    }

    // remove cards from one player and give to another player
    // returns true if the player had the cards
    // false if the player did not have any cards
    public bool requestCards(Player from, Player to, Card card) {
        int numRemoved = from.removeAllFromHand(card.getNumValue());
        for (int i = 0; i < numRemoved; i++) {
            to.addToHand(card);
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
