using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BruhManager : MonoBehaviour
{
    public GameObject CardDealer;
    public GameObject DealButton;
    
    /* Global variables */ 
    //private
    private CardDealer cardDealer;

    //public
    public bool sleeping = false;
    public bool sleepRunning = false;
    public bool playerWin = false;
    public bool gameEnded = false;

    public int playerColorConsecCount = 0;
    public int playerNumberConsecCount = 0;
    public int playerGreaterConsecCount = 0;
    public int playerLowerConsecCount = 0;
    public int playerBruhConsecCount = 0;
    public int playerMovesConsecCount = 0; //for playing card >3 times
    public float userXPos = -7;
    public float userYPos = -124;

    public int currentState = 0;

    public CardPlayer userPlayer;
    public List<GameObject> pool;

    /* User player class */
    public class CardPlayer {
        private List<GameObject> hand;
        private string userID;

        public CardPlayer(string userID) {
            hand = new List<GameObject>();
            this.userID = userID;
        }

        public List<GameObject> getHandList() {
            return hand;
        }
        
        public string getUserID() {
            return userID;
        }

        public void printHand() {
            int count = 1;
            Debug.Log("\n" + userID + "Current hand:");
            foreach (GameObject currentCard in hand) {
                Debug.Log(count + ": " + currentCard.GetComponent<Card>().faceValue + " of " + currentCard.GetComponent<Card>().suitValueString);
                count++;
            }
        }

        public void addToHand(GameObject cardToAdd) {
            hand.Add(cardToAdd);
        }

        public GameObject removeFromHand(int indexInHand) {
            GameObject returnCard = (GameObject) hand[indexInHand];
            hand.RemoveAt(indexInHand);
            return returnCard;
        }

        public GameObject peekAtCard(int indexInHand) {
            GameObject returnCard = (GameObject) hand[indexInHand];
            return returnCard;
        }

        /*gets card value and removes it from the User's hand*/
        public GameObject playCard(int indexInHand) {
            GameObject returnCard = removeFromHand(indexInHand);
            return returnCard;
        }
    }

    private void DisplayOneHand(CardPlayer currentPlayer)
    {
        float x = userXPos;
        float z = 0;

        for (int i = 0; i < currentPlayer.getHandList().Count; i++)
        {
            currentPlayer.peekAtCard(i).transform.position = new Vector3(x, userYPos, z);

            currentPlayer.peekAtCard(i).SetActive(true);

            x = x + 0.45f;
            z = z - 0.1f;
        }
    }

    /* abstracted methods */
    public bool checkForWinner() { 
        Debug.Log("Check winner");
        if(userPlayer.getHandList().Count >= 20) {
            gameEnded = true;
            playerWin = false;
            return true;
        } else if((userPlayer.getHandList().Count <= 0 || playerBruhConsecCount == 4) && currentState != 0) {
            gameEnded = true;
            playerWin = true;
            if(playerBruhConsecCount == 4) {
                Debug.Log("You found the secret ending!");
            }
            return true;
        }
        return false;
    } 
    

    public void gameLoop() {
        bool finished = checkForWinner();
        if(finished == false) {
            if(currentState == 0) {
                //deal user their cards
                Debug.Log("Player dealt cards");
                for(int i = 0; i < 7; i++) {
                    userPlayer.addToHand(pool[i]);
                }
                DisplayOneHand(userPlayer);
            } else if(currentState == 1) {
                
            } else if(currentState == 2) {
                
            } else if(currentState == 3) {
                
            } else if(currentState == 4) {
                
            }
        }
        
    }

    IEnumerator  sleepFunction() {
        sleeping = true;
        yield return new WaitForSeconds(3);
        sleeping = false;
        sleepRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Bruh game Starting");
        cardDealer = CardDealer.GetComponent<CardDealer>();
        pool = cardDealer.RandomCards(52, 1, 13, true, true, true, true);
        
        foreach(GameObject cards in pool) {
            if(cards.GetComponent<Card>().suitValue == Card.suit.CLUBS) {
                cards.GetComponent<Card>().suitValueString = "Clubs";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.HEARTS) {
                cards.GetComponent<Card>().suitValueString = "Hearts";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.DIAMONDS) {
                cards.GetComponent<Card>().suitValueString = "Diamonds";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.SPADES) {
                cards.GetComponent<Card>().suitValueString = "Spades";
            }
        }

        userPlayer = new CardPlayer("1");
        currentState = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("sleep value: " + sleeping);
        if(sleeping == false) {
            gameLoop();
        } 
        if(sleepRunning == false){
            sleepRunning = true;
            StartCoroutine(sleepFunction());
        }
    }
}
