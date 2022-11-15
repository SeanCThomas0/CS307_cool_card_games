using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

public class PokerScript : MonoBehaviour
{
    public GameObject GameMessages;
    public GameObject CardDealer;
    public GameObject QuitButton;
    public GameObject ExperiencedButton;
    public GameObject EasyButton;
    public GameObject player1Text;
    public GameObject player2Text;
    public GameObject player3Text;
    public GameObject player4Text;
    public GameObject player1Bet;
    public GameObject player2Bet;
    public GameObject player3Bet;
    public GameObject player4Bet;
    private CardDealer cardDealer;
    public bool game_won = false;

    /*Temporary card class for testing to be replaced with CardDealer script*/
    public float userXPos = 0f;
    public float userYPos = 0f;
    public static bool experienced = false;

    private UserPreferences.backgroundColor backgroundColor;
    public GameObject mainCam;

    /*
    [SerializeField] public AudioSource ClickSound;
    [SerializeField] public AudioSource WinSound;
    [SerializeField] public AudioSource CardSound;
    [SerializeField] public AudioSource Music;
    */
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


    /*CardPlayer class for individual game players*/
    public class CardPlayer
    {
        private List<GameObject> hand;
        private string userID;
        private int chipAmount;
        private int currentBet = 0;
        private bool isHuman;
        public GameObject playerText;
        public GameObject betText;

        public CardPlayer(string userID, int chipAmount, bool isHuman, GameObject playerText, GameObject betText)
        {
            hand = new List<GameObject>();
            this.userID = userID;
            this.chipAmount = chipAmount;
            this.isHuman = isHuman;
            this.playerText = playerText;
            this.betText = betText;
        }

        public List<GameObject> getHandList()
        {
            return hand;
        }

        public string getUserID()
        {
            return userID;
        }

        public bool getIsHuman()
        {
            return isHuman;
        }

        public int getCurrentBet() {
            return currentBet;
        }

        public void setCurrentBet(int currentBet) {
            this.currentBet = currentBet;
            Debug.Log("bet is " + currentBet);
            betText.GetComponent<TMPro.TextMeshProUGUI>().text = "Bet: " + currentBet; 
        }

        public int getChipAmount()
        {
            return chipAmount;
        }

        public void setChipAmount(int chipAmount)
        {
            this.chipAmount = chipAmount;
            playerText.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + chipAmount;
        }

        public void printHand()
        {
            int count = 1;
            Debug.Log("\n" + userID + "Current hand:");
            foreach (GameObject currentCard in hand)
            {
                Debug.Log(count + ": " + currentCard.GetComponent<Card>().faceValue + " of " + currentCard.GetComponent<Card>().suitValueString);
                count++;
            }
        }

        public void addToHand(GameObject cardToAdd)
        {
            hand.Add(cardToAdd);
        }

        public void clearHand()
        {
            hand.Clear();
        }

        public GameObject removeFromHand(int indexInHand)
        {
            GameObject returnCard = (GameObject)hand[indexInHand];
            hand.RemoveAt(indexInHand);
            return returnCard;
        }

        public GameObject peekAtCard(int indexInHand)
        {
            GameObject returnCard = (GameObject)hand[indexInHand];
            return returnCard;
        }

        /*gets card value and removes it from the User's hand*/
        public GameObject playCard(int indexInHand)
        {
            GameObject returnCard = (GameObject)hand[indexInHand];
            removeFromHand(indexInHand);
            return returnCard;
        }
    }


    //visually show cards 
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

    private void DisplayOnePlay(GameObject currentCard)
    {
        float x = userXPos;
        float z = 0;


        currentCard.transform.position = new Vector3(x + 1, userYPos + 2, z);

        currentCard.SetActive(true);
    }

    private void DisplayTwoHand(GameObject currentCard)
    {
        float x = userXPos;
        float z = 0;


        currentCard.transform.position = new Vector3(x - 5, -4, z);

        currentCard.SetActive(true);

    }

    private void DisplayThreeHand(GameObject currentCard)
    {
        float x = userXPos - 5;
        float z = 0;

        currentCard.transform.position = new Vector3(x + 6, -2, z);

        currentCard.SetActive(true);
    }

    private void DisplayFourHand(GameObject currentCard)
    {
        float x = userXPos;
        float z = 0;

        currentCard.transform.position = new Vector3(x + 7, -4, z);

        currentCard.SetActive(true);
    }

    //display flop, turn, or river
    private void displayDeckCard(GameObject currentCard) {
        float x = -4 + (displayCount);
        float z = 0;
        displayCount += 2;

        
        currentCard.transform.position = new Vector3(x, 2, z);

        currentCard.SetActive(true);
    }

    private void flipCard(GameObject currentCard)
    {
        currentCard.SetActive(false);
    }

    public string currentInput = "empty";
   
    public Queue<CardPlayer> playerQueue = new Queue<CardPlayer>();
    public Queue<CardPlayer> dealQueue = new Queue<CardPlayer>();
    public CardPlayer playerOne;
    public CardPlayer playerTwo;
    public CardPlayer playerThree;
    public CardPlayer playerFour;
    public CardPlayer dealer = null;
    public CardPlayer currentPlayer = null;
    public CardPlayer currentDealer = null;
    public CardPlayer smallBlind = null;
    public CardPlayer bigBlind = null;
    
    public static int potValue = 0;
    public int currentState = 0;
    public int matchBet = 0;
    public static int deckCount = 0;
    public static int displayCount = 0;
    /*
        0=Initial,
        1=pick up top card
        2 = choose trump
        3 = play card
    */

    public bool sleeping = false;
    public bool sleepRunning = false;
    public List<GameObject> pool;
    public List<GameObject> playedCards;

    public bool databaseUpdated; //Whether or not the database has been updated after the game ends
    //private DatabaseReference databaseReference;
    //private FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        
        /*
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        ClickSound.volume = volumeValue;
        WinSound.volume = volumeValue;
        CardSound.volume = volumeValue / 3;
        */

        cardDealer = CardDealer.GetComponent<CardDealer>();

        Debug.Log("Poker Game Starting");
        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Poker Game Starting";
        StartCoroutine(sleepFunction());
        pool = cardDealer.RandomCards(52, 1, 13, true, true, true, true);
        
        foreach (GameObject cards in pool)
        {
            //Debug.Log(cards.GetComponent<Card>().numValue + " of " + cards.GetComponent<Card>().suitValue);
            if (cards.GetComponent<Card>().numValue == 11)
            {
                cards.GetComponent<Card>().faceValue = "Jack";
            }
            else if (cards.GetComponent<Card>().numValue == 12)
            {
                cards.GetComponent<Card>().faceValue = "Queen";
            }
            else if (cards.GetComponent<Card>().numValue == 13)
            {
                cards.GetComponent<Card>().faceValue = "King";
            }
            else if (cards.GetComponent<Card>().numValue == 1)
            {
                cards.GetComponent<Card>().numValue = 14;
                cards.GetComponent<Card>().faceValue = "Ace";
            } 
            else
            {
                cards.GetComponent<Card>().faceValue = cards.GetComponent<Card>().numValue.ToString();
            }

            if (cards.GetComponent<Card>().suitValue == Card.suit.CLUBS)
            {
                cards.GetComponent<Card>().suitValueString = "Clubs";
            }
            else if (cards.GetComponent<Card>().suitValue == Card.suit.HEARTS)
            {
                cards.GetComponent<Card>().suitValueString = "Hearts";
            }
            else if (cards.GetComponent<Card>().suitValue == Card.suit.DIAMONDS)
            {
                cards.GetComponent<Card>().suitValueString = "Diamonds";
            }
            else if (cards.GetComponent<Card>().suitValue == Card.suit.SPADES)
            {
                cards.GetComponent<Card>().suitValueString = "Spades";
            }
        }
        
        playerOne = new CardPlayer("" + 1, 200, true, player1Text, player1Bet);
        playerQueue.Enqueue(playerOne);
        dealQueue.Enqueue(playerOne);
        player1Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerOne.getChipAmount(); 
        playerOne.setCurrentBet(0);
        
        playerTwo = new CardPlayer("" + 2, 200, false, player2Text, player2Bet);
        playerQueue.Enqueue(playerTwo);
        dealQueue.Enqueue(playerTwo);
        player2Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerTwo.getChipAmount(); 
        playerTwo.setCurrentBet(0);

        playerThree = new CardPlayer("" + 3, 200, false, player3Text, player3Bet);
        playerQueue.Enqueue(playerThree);
        dealQueue.Enqueue(playerThree);
        player3Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerThree.getChipAmount(); 
        playerThree.setCurrentBet(0);

        playerFour = new CardPlayer("" + 4, 200, false, player4Text, player4Bet);
        playerQueue.Enqueue(playerFour);
        dealQueue.Enqueue(playerFour);
        player4Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerFour.getChipAmount(); 
        playerFour.setCurrentBet(0);

        pool = cardDealer.ShuffleCards(pool);

        currentState = -1;
        
    }

    void gameDecision()
    {
        bool finished = checkForWinner();
        if (!finished)
        {
            //if the game does not have a winner
            if (currentState == -1)
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a difficulty button";
                if (currentInput.Equals("Button"))
                {
                    EasyButton.SetActive(false);
                    ExperiencedButton.SetActive(false);
                    currentState = 0;
                    currentInput = "empty";
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Game Starting";
                }

            } 
            else if (currentState == 0)
            {
                potValue = 0;
                //Select dealer and big/small blind
                //small blind is 2 chips big blind is 4 chips
                currentDealer = dealQueue.Dequeue();
                dealQueue.Enqueue(currentDealer);

                smallBlind = dealQueue.Dequeue();
                dealQueue.Enqueue(smallBlind);
                smallBlind.setChipAmount(smallBlind.getChipAmount() - 2);
                smallBlind.setCurrentBet(2);
                potValue += 2;

                bigBlind = dealQueue.Dequeue();
                dealQueue.Enqueue(bigBlind);
                bigBlind.setChipAmount(smallBlind.getChipAmount() - 4);
                bigBlind.setCurrentBet(4);
                potValue += 4;
                matchBet = 4;

                for(int i = 0; i < 3; i++) {
                    currentPlayer = playerQueue.Dequeue();
                    playerQueue.Enqueue(currentPlayer);
                }
                currentPlayer = playerQueue.Dequeue();

                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealer is: " + currentDealer.getUserID();
                
                pool = cardDealer.ShuffleCards(pool);

                deckCount = 0;
                for(int i = 0; i < 8; i++) {
                    currentPlayer.addToHand(pool[deckCount]);
                    deckCount++;
                    playerQueue.Enqueue(currentPlayer);
                    currentPlayer = playerQueue.Dequeue();
                }
                
                currentState = 1;
            } 
            else if (currentState == 1)
            {
                if(currentPlayer.getIsHuman()) {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = currentPlayer.getUserID() + " match the highest bet, raise, or fold";
                    if(!currentInput.Equals("empty")) {
                        if(currentInput.Equals("fold")) {
                            currentPlayer = playerQueue.Dequeue();
                        } else {
                            bool validBet = false;
                            int intInput = int.Parse(currentInput);
                            if(intInput + currentPlayer.getCurrentBet() == matchBet) {
                                potValue += matchBet - intInput;
                                currentPlayer.setCurrentBet(matchBet);
                                validBet = true;
                            } else if(intInput + currentPlayer.getCurrentBet() > matchBet) {
                                potValue += intInput - matchBet;
                                currentPlayer.setCurrentBet(intInput); 
                                validBet = true;
                            } else {
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: fold or match/raise the highest bet";
                            }
                            if(validBet == true) {
                                currentPlayer = playerQueue.Dequeue();
                                playerQueue.Enqueue(currentPlayer);
                            }
                            currentInput = "empty";
                        }
                    }
                } else {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pre Flop betting";
                    string compAction = "";
                    //only have coputer match bet for now
                    potValue += matchBet - currentPlayer.getCurrentBet();
                    currentPlayer.setChipAmount(currentPlayer.getChipAmount() - (matchBet - currentPlayer.getCurrentBet()));
                    currentPlayer.setCurrentBet(matchBet); 
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opponent " + currentPlayer.getUserID() + " chose to "; //add in action

                    //get next player and add player back if they didnt fold
                    playerQueue.Enqueue(currentPlayer);
                    currentPlayer = playerQueue.Dequeue();
                }
                Debug.Log("Pre flop betting");
                currentState = 2;
                //have for loop that sees if all player bets are equal then move to next state
            }
            else if (currentState == 2) //show first set of three cards
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Flop";
                Debug.Log("deal flop");
                deckCount++;
                for(int i = 0; i < 3; i++) {
                    playedCards.Add(pool[deckCount]);
                    displayDeckCard(pool[deckCount]);
                    deckCount++;
                }
                currentState = 3;
            }
            else if (currentState == 3) //let players bet again
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "post flop betting";
                Debug.Log("bet after flop");
                currentState = 4;
            }
            else if (currentState == 4) //show fourth card
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing turn card";
                Debug.Log("deal turn");
                deckCount++;
                playedCards.Add(pool[deckCount]);
                displayDeckCard(pool[deckCount]);
                deckCount++;
                currentState = 5;
            }
            else if (currentState == 5) //let players bet again
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("bet after turn");
                currentState = 6;
            }
            else if (currentState == 6) //show fifth and final card
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing river card";
                Debug.Log("deal river");
                deckCount++;
                playedCards.Add(pool[deckCount]);
                displayDeckCard(pool[deckCount]);
                deckCount++;
                currentState = 7;
            }
            else if (currentState == 7) //let players bet again
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("bet after river");
                currentState = 8;
            }
            else if (currentState == 8) //show player cards and determine winner
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("Show winner");
                currentState = 9;
            }
            else if (currentState == 9) //reset pool and betting values and move to next dealer
            {
                playerQueue.Enqueue(currentPlayer);
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("reset values");
                displayCount = 0;
                for(int i = 0; i < playedCards.Count; i++) {
                    flipCard(playedCards[i]);
                }
                playedCards.Clear();
                Debug.Log("cleared");
                deckCount = 0;
                for(int i = 0; i < dealQueue.Count; i++) {
                    currentPlayer = dealQueue.Dequeue();
                    Debug.Log(currentPlayer.getHandList().Count);
                    currentPlayer.clearHand();
                    currentPlayer.setCurrentBet(0);
                    dealQueue.Enqueue(currentPlayer);
                    Debug.Log("ran " + i);
                }

                currentState = 0;
            }
            
        }

    }

    IEnumerator sleepFunction()
    {
        sleeping = true;
        yield return new WaitForSeconds(1);
        sleeping = false;
        sleepRunning = false;
    }

    public bool checkForWinner()
    {
        
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("sleep value: " + sleeping);
        if (sleeping == false)
        {
            //Debug.Log("Game decision run");
            gameDecision();
        }
        if (sleepRunning == false)
        {
            sleepRunning = true;
            StartCoroutine(sleepFunction());
            //Debug.Log("sleep running set to true");
        }
    }

    
    public void UseExitButton()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
    

    public void OnEnter(string userInput)
    {
        Debug.Log("User entered " + userInput);
        currentInput = userInput;
    }

    public void ExperiencedMode()
    {
        experienced = true;
        currentInput = "Button";
    }

    public void EasyMode()
    {
        experienced = false;
        currentInput = "Button";
    }

}
