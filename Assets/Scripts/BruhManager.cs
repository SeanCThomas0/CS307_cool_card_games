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
    public GameObject UserInput;
    public GameObject CompanionText;
    public GameObject OppText;
    public GameObject GameText;
    
    /* Global variables */ 
    //private
    private CardDealer cardDealer;

    //public
    public bool sleeping = false;
    public bool sleepRunning = false;
    public bool playerWin = false;
    public bool gameEnded = false;
    public static bool diff = false;

    public int playerColorConsecCount = 0;
    public int playerNumberConsecCount = 0;
    public int playerGreaterConsecCount = 0;
    public int playerLowerConsecCount = 0;
    public int playerBruhConsecCount = 0;
    public int playerMovesConsecCount = 0; //for playing card >3 times
    public int turnsSinceCompHelp = 3;
    public float userXPos = 300f;
    public float userYPos = 200f;
    public string currentInput;

    public int currentState = 0;

    public CardPlayer userPlayer;
    public List<GameObject> pool;
    public GameObject lastCardPlayed;
    public ComputerOpp opponent;
    public ComputerComp companion;

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

    public class ComputerOpp{
        private bool difficult;
        private int turnsCount;
        public GameObject OppText;

        public ComputerOpp(bool difficult, GameObject OppText) {
            this.difficult = diff;
            this.turnsCount = 0;
            this.OppText = OppText;
        }

        public void displayOppMessage() {
            OppText.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opp moves count: " + turnsCount;
            turnsCount++;
        }
    }

    public class ComputerComp{
        private int recs;
        public GameObject CompanionText;

        public ComputerComp(GameObject CompanionText) {
            this.recs = 0;
            this.CompanionText = CompanionText;
        }

        public void displayCompMessage() {
            CompanionText.GetComponent<TMPro.TextMeshProUGUI>().text = "companion recommendations count: " + recs;
            recs++;
        }
    }

    private void DisplayOneHand(CardPlayer currentPlayer)
    {
        float x = userXPos;
        float z = 100f;

        for (int i = 0; i < currentPlayer.getHandList().Count; i++)
        {
            currentPlayer.peekAtCard(i).transform.position = new Vector3(x + 190f, userYPos + 240f, z);

            currentPlayer.peekAtCard(i).SetActive(true);

            x = x + 30f;
            z = z - 5f;
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
                GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player cards dealt";
                for(int i = 0; i < 7; i++) {
                    userPlayer.addToHand(pool[i]);
                }
                DisplayOneHand(userPlayer);
                currentState = 1;
            } else if(currentState == 1) {
                //have computer opponent give their message
                opponent.displayOppMessage();
                currentState = 2;
            } else if(currentState == 2) {
                //wait for user input
                if(!currentInput.Equals("empty")) {
                    Debug.Log("recieved user input" + currentInput);
                    if(currentInput.Equals("help")) {
                        if(turnsSinceCompHelp >= 3) {
                            currentState = 3;
                        } else {
                            Debug.Log("Companion is on cooldown");
                        }
                    } else {
                        currentState = 4;
                    }
                }
            } else if(currentState == 3) {
                //if input asks for companion advice:
                //give companion advice then reprompt the user to make a move
                companion.displayCompMessage();
                currentState = 2;
            } else if(currentState == 4) {
                //evaluate user input 
                //if user input is valid accept or reject move and update counters
                //once finished go back to state 1
                currentState = 1;
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
        opponent = new ComputerOpp(false, OppText);
        companion = new ComputerComp(CompanionText);
        cardDealer.cardSize = Card.cardSize.HUGE;
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

    public void OnEnter(string userInput) {
        Debug.Log("User entered " + userInput); 
        currentInput = userInput;
    }

    public void DeckClick() {
        Debug.Log("deck draw"); 
        currentInput = "deck draw";
    }

    public void DifficultMode() {
        diff = true;
        currentInput = "Button";
    }
}
