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

    public int playerChoiceIndex = 0;
    public int roundsPlayed = 0;
    public static int playerColorConsecCount = 0;
    public static int playerNumberConsecCount = 0;
    public static int playerGreaterConsecCount = 0;
    public static int playerLowerConsecCount = 0;
    public static int playerBruhConsecCount = 0;
    public static int playerMovesConsecCount = 0; //for playing card >3 times
    public int turnsSinceCompHelp = 3;
    public float userXPos = 300f;
    public float userYPos = 200f;
    public string currentInput = "empty";

    public int currentState = 0;

    public CardPlayer userPlayer;
    public static List<GameObject> pool;
    public static GameObject lastCardPlayed;
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
            pool.Add(returnCard);
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
            OppText.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opponent says: " + recommendationOptions();
            turnsCount++;
        }

        private string recommendationOptions() {
            System.Random rand = new System.Random();
            int easyIndex = rand.Next(0, 10);
            string goodMessage = "Play any card";
            string badMessage = "Good Luck";
            //easy companion reccomendations

            //start of good advice
            if(playerColorConsecCount == 1) {
                goodMessage = "Don't play another " + lastCardPlayed.GetComponent<Card>().frontColor;
            } 
            
            if(playerNumberConsecCount == 1) {
                goodMessage = "Don't play another " + lastCardPlayed.GetComponent<Card>().faceValue;
            } 

            if(playerGreaterConsecCount == 1) {
                goodMessage = "Don't play a card greater than " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(playerLowerConsecCount == 1) {
                goodMessage = "Don't play a card less than " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(playerMovesConsecCount == 5) {
                goodMessage = "Draw a card " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            //start of bad advice
            if(playerColorConsecCount == 1) {
                goodMessage = "play another " + lastCardPlayed.GetComponent<Card>().frontColor;
            } 
            
            if(playerNumberConsecCount == 1) {
                goodMessage = "play another " + lastCardPlayed.GetComponent<Card>().faceValue;
            } 

            if(playerGreaterConsecCount == 1) {
                goodMessage = "play a card greater than " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(playerLowerConsecCount == 1) {
                goodMessage = "play a card less than " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(playerMovesConsecCount == 5) {
                goodMessage = "Draw a card " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(easyIndex <= 3) {
                return goodMessage;
            } else {
                return badMessage;
            }
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
            CompanionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer companion says: " + recommendationOptions();
            recs++;
        }

        private string recommendationOptions() {
            System.Random rand = new System.Random();
            int easyIndex = rand.Next(0, 10);
            string recMessage = "Play any card";
            //easy companion reccomendations
            if(playerColorConsecCount == 1) {
                recMessage = "Don't play another " + lastCardPlayed.GetComponent<Card>().frontColor;
            } 
            
            if(playerNumberConsecCount == 1) {
                recMessage = "Don't play another " + lastCardPlayed.GetComponent<Card>().faceValue;
            } 

            if(playerGreaterConsecCount == 1) {
                recMessage = "Don't play a card greater than " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(playerLowerConsecCount == 1) {
                recMessage = "Don't play a card less than " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            if(playerMovesConsecCount == 5) {
                recMessage = "Draw a card " + lastCardPlayed.GetComponent<Card>().faceValue;
            }

            return recMessage;
        } 
    }

    private int bruhSequenceChecker(int bruhIndex, GameObject currCard) {
        if(bruhIndex == 0 && currCard.GetComponent<Card>().frontColor.Equals("Black")) {
            return 1;
        } else if(bruhIndex == 1 && currCard.GetComponent<Card>().frontColor.Equals("Red")) {
            return 2;
        } else if(bruhIndex == 2 && currCard.GetComponent<Card>().numValue < lastCardPlayed.GetComponent<Card>().numValue) {
            return 3;
        } else if(bruhIndex == 3 && currCard.GetComponent<Card>().numValue > lastCardPlayed.GetComponent<Card>().numValue) {
            return 4;
        }
        return 0;
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

    private void flipCard(GameObject currentCard) {
        currentCard.SetActive(false);
    }

    /* abstracted methods */
    public bool checkForWinner() { 
        //Debug.Log("Check winner");
        if(userPlayer.getHandList().Count >= 14) {
            gameEnded = true;
            playerWin = false;
            return true;
        } else if((userPlayer.getHandList().Count <= 0 || playerBruhConsecCount == 4) && currentState != 0) {
            gameEnded = true;
            playerWin = true;
            Debug.Log("Player wins");
            GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "You Win!!!";
            if(playerBruhConsecCount == 4) {
                Debug.Log("You found the secret ending!");
                GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "You Win!!! You found the secret ending!";
            }
            return true;
        }
        return false;
    } 
    

    public void gameLoop() {
        bool finished = checkForWinner();
        if(finished == false) {
            if(currentState == 0) {
                Debug.Log("state 0 starting");
                //deal user their cards
                Debug.Log("Player dealt cards");
                GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "Player cards dealt";
                for(int i = 0; i < 7; i++) {
                    userPlayer.addToHand(pool[i]);
                    pool.RemoveAt(i);
                }

                DisplayOneHand(userPlayer);
                currentInput = "empty";
                currentState = 1;
            } else if(currentState == 1) {
                Debug.Log("state 1 starting");
                //have computer opponent give their message
                opponent.displayOppMessage();
                currentState = 2;
            } else if(currentState == 2) {
                GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "Enter the index of the card you'd like to play (1 is leftmost card)";
                Debug.Log("state 2 starting");
                //wait for user input
                if(!currentInput.Equals("empty")) {
                    int intInput = -1; //= int.Parse(currentInput);

                    if(currentInput.Equals("1") || currentInput.Equals("2") || currentInput.Equals("3") || currentInput.Equals("4") || currentInput.Equals("5") 
                    || currentInput.Equals("6") || currentInput.Equals("7") || currentInput.Equals("8") || currentInput.Equals("9") || currentInput.Equals("10") 
                    || currentInput.Equals("11") || currentInput.Equals("12") || currentInput.Equals("13") || currentInput.Equals("14")) {
                        intInput = int.Parse(currentInput);
                    }

                    if(currentInput.Equals("deck") || currentInput.Equals("help") || (intInput > 0 && intInput <= userPlayer.getHandList().Count)) {
                        string playerHand = "";
                        for (int i = 0; i < userPlayer.getHandList().Count; i++)
                        {
                            playerHand += userPlayer.peekAtCard(i).GetComponent<Card>().suitValueString;
                            playerHand += " ";
                            playerHand += userPlayer.peekAtCard(i).GetComponent<Card>().numValue;
                            playerHand += ", ";
                        }
                        Debug.Log(playerHand);
                        Debug.Log("recieved user input: " + currentInput);
                        if(currentInput.Equals("help")) {
                            if(turnsSinceCompHelp >= 3) {
                                currentState = 3;
                                turnsSinceCompHelp = 0;
                            } else {
                                Debug.Log("Companion is on cooldown");
                                CompanionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Companion is on cooldown";
                            }
                        } else if(currentInput.Equals("deck")) {
                            userPlayer.addToHand(pool[0]);
                            pool.RemoveAt(0);
                            //pool[0].SetActive(true);
                            Debug.Log("deck draw");
                            playerChoiceIndex = -1;
                            playerMovesConsecCount = 0;
                            currentState = 4;
                        } else {
                            playerChoiceIndex  = intInput;
                            currentState = 4;
                        }

                    } else {
                        Debug.Log("Invalid input");
                        GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid Input, choose to \"help\", click the deck button on the right, or enter a valid hand index for a card in your hand";
                    }
                    currentInput = "empty";
                }
            } else if(currentState == 3) {
                Debug.Log("state 3 starting");
                //if input asks for companion advice:
                //give companion advice then reprompt the user to make a move
                companion.displayCompMessage();
                currentState = 2;
            } else if(currentState == 4) {
                Debug.Log("state 4 starting");
                //evaluate user input 
                //if user input is valid accept or reject move and update counters
                //once finished go back to state 1
                if(playerChoiceIndex != -1) {
                    if(roundsPlayed == 0) {
                        lastCardPlayed = userPlayer.playCard(playerChoiceIndex - 1);
                        flipCard(lastCardPlayed);
                        DisplayOneHand(userPlayer);
                        if(lastCardPlayed.GetComponent<Card>().frontColor.Equals("Black")) {
                            playerBruhConsecCount = 1;
                        }
                        GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your move to play " + lastCardPlayed.GetComponent<Card>().faceValue + " of " + lastCardPlayed.GetComponent<Card>().suitValueString + " was accepted";

                        playerMovesConsecCount++;
                    } else {
                        bool invalid = false;
                        //add adjustments to see what counts would be with peek
                        if(playerColorConsecCount == 1) {
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().frontColor.Equals("Black") && 
                            lastCardPlayed.GetComponent<Card>().frontColor.Equals("Black")) {
                                invalid = true;
                                userPlayer.addToHand(pool[0]);
                                pool.RemoveAt(0);
                                Debug.Log("consec color broken");
                            }

                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().frontColor.Equals("Red") && 
                            lastCardPlayed.GetComponent<Card>().frontColor.Equals("Red")) {
                                invalid = true;
                                userPlayer.addToHand(pool[0]);
                                pool.RemoveAt(0);
                                Debug.Log("consec color broken");
                            }
                            
                        } else if(playerNumberConsecCount == 1) {
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().numValue == lastCardPlayed.GetComponent<Card>().numValue) {
                                invalid = true;
                                userPlayer.addToHand(pool[0]);
                                pool.RemoveAt(0);
                                Debug.Log("consec number broken");
                            }
                        } else if(playerGreaterConsecCount == 1) {
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().numValue > lastCardPlayed.GetComponent<Card>().numValue) {
                                invalid = true;
                                userPlayer.addToHand(pool[0]);
                                pool.RemoveAt(0);
                                Debug.Log("consec greater broken");
                            }
                        } else if(playerLowerConsecCount == 1) {
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().numValue < lastCardPlayed.GetComponent<Card>().numValue) {
                                invalid = true;
                                userPlayer.addToHand(pool[0]);
                                pool.RemoveAt(0);
                                Debug.Log("consec lower broken");
                            }
                        } else if(playerMovesConsecCount == 5) { //needs to be 5
                            invalid = true;
                            userPlayer.addToHand(pool[0]);
                            pool.RemoveAt(0);
                            playerMovesConsecCount = 0;
                            Debug.Log("consec moves w/out draw broken");
                        }  

                        if(invalid == false) {
                            //first
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().frontColor.Equals("Black") && 
                            lastCardPlayed.GetComponent<Card>().frontColor.Equals("Black")) {
                                playerColorConsecCount++;
                            } else if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().frontColor.Equals("Red") && 
                            lastCardPlayed.GetComponent<Card>().frontColor.Equals("Red")) {
                                playerColorConsecCount++;
                            } else {
                                playerColorConsecCount = 0;
                            }
                            //second
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().numValue == lastCardPlayed.GetComponent<Card>().numValue) {
                                playerNumberConsecCount++;
                            } else {
                                playerNumberConsecCount = 0;
                            }

                            //third
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().numValue > lastCardPlayed.GetComponent<Card>().numValue) {
                                playerGreaterConsecCount++;
                            } else {
                                playerGreaterConsecCount = 0;
                            }

                            //fourth
                            if(userPlayer.peekAtCard(playerChoiceIndex - 1).GetComponent<Card>().numValue < lastCardPlayed.GetComponent<Card>().numValue) {
                                playerLowerConsecCount++;
                            } else {
                                playerLowerConsecCount = 0;
                            }

                            //play card
                            playerBruhConsecCount = bruhSequenceChecker(playerBruhConsecCount, userPlayer.peekAtCard(playerChoiceIndex - 1));
                            Debug.Log("Secret count is: " + playerBruhConsecCount);
                            lastCardPlayed = userPlayer.playCard(playerChoiceIndex - 1);
                            flipCard(lastCardPlayed);
                            GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "Your move to play " + lastCardPlayed.GetComponent<Card>().faceValue + " of " + lastCardPlayed.GetComponent<Card>().suitValueString + " was accepted";
                            playerMovesConsecCount++;
                        } else {
                            GameText.GetComponent<TMPro.TextMeshProUGUI>().text = "That move did not follow the rules of Bruh! Adding a card to your hand as a penalty";
                        }
                    }
                    roundsPlayed++;
                }
                DisplayOneHand(userPlayer);
                turnsSinceCompHelp++;

                currentState = 1;
            }
        }
        
    }

    IEnumerator  sleepFunction() {
        sleeping = true;
        yield return new WaitForSeconds(2);
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
                cards.GetComponent<Card>().frontColor = "Black";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.HEARTS) {
                cards.GetComponent<Card>().suitValueString = "Hearts";
                cards.GetComponent<Card>().frontColor = "Red";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.DIAMONDS) {
                cards.GetComponent<Card>().suitValueString = "Diamonds";
                cards.GetComponent<Card>().frontColor = "Red";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.SPADES) {
                cards.GetComponent<Card>().suitValueString = "Spades";
                cards.GetComponent<Card>().frontColor = "Black";
            }

            if(cards.GetComponent<Card>().numValue == 1) {
                cards.GetComponent<Card>().faceValue = "Ace";
            } else if(cards.GetComponent<Card>().numValue == 11) {
                cards.GetComponent<Card>().faceValue = "Jack";
            } else if(cards.GetComponent<Card>().numValue == 12) {
                cards.GetComponent<Card>().faceValue = "Queen";
            } else if(cards.GetComponent<Card>().numValue == 13) {
                cards.GetComponent<Card>().faceValue = "King";
            } else {
                cards.GetComponent<Card>().faceValue = cards.GetComponent<Card>().numValue.ToString();
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
        if(currentState == 2) {
            Debug.Log("User entered " + userInput); 
            currentInput = userInput;
        }
    }

    public void DeckClick() {
        if(currentState == 2) {
            Debug.Log("deck draw"); 
            currentInput = "deck";
        }
    }

    public void DifficultMode() {
        diff = true;
        currentInput = "Button";
    }
}
