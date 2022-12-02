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
    public GameObject PotText;
    public GameObject CardDealer;
    public GameObject QuitButton;
    public GameObject ExperiencedButton;
    public GameObject EasyButton;
    public GameObject Player1Input;
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

    public Transform canvas;

    /*Temporary card class for testing to be replaced with CardDealer script*/
    public static float userXPos = 0f;
    public static float userYPos = 0f;
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
        private string position;
        private int chipAmount;
        private int currentBet = 0;
        private bool isHuman;
        public GameObject playerText;
        public GameObject betText;

        public CardPlayer(string userID, int chipAmount, bool isHuman, GameObject playerText, GameObject betText)
        {
            hand = new List<GameObject>();
            this.userID = userID;
            this.position = userID;
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

        public bool addToBetAmount(int currentBet)
        {
            if(chipAmount - currentBet < 0) {
                return false;
            } else {
                this.currentBet += currentBet;
                this.chipAmount -= currentBet;
                betText.GetComponent<TMPro.TextMeshProUGUI>().text = "Bet: " + this.currentBet; 
                playerText.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + chipAmount;
            }
            return true;
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

        public void addToChipAmount(int chipAmount)
        {
            this.chipAmount += chipAmount;
            playerText.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + this.chipAmount;
        }

        public void printHand()
        {
            int count = 1;
            
            if(position.Equals("1")) {
                DisplayOneHand();
            }
            if(position.Equals("2")) {
                DisplayTwoHand();
            }
            if(position.Equals("3")) {
                DisplayThreeHand();
            }
            if(position.Equals("4")) {
                DisplayFourHand();
            }
            count++;
            
        }

        public void addToHand(GameObject cardToAdd)
        {
            hand.Add(cardToAdd);
        }

        public void clearHand()
        {
            foreach(GameObject currCard in hand) {
                flipCard(currCard);
            }
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

         //visually show cards 
        private void DisplayOneHand()
        {
            float x = -8;
            float z = 0;

            Debug.Log("\n" + userID + "Current hand:");
            for (int i = 0; i < getHandList().Count; i++)
            {
                peekAtCard(i).transform.position = new Vector3(x, -0.5f, z);
            
                Debug.Log((i+1) + ": " + peekAtCard(i).GetComponent<Card>().faceValue + " of " + peekAtCard(i).GetComponent<Card>().suitValueString);
                peekAtCard(i).SetActive(true);

                x = x + 1f;
                z = z - 0.1f;
            }
        }

        private void DisplayTwoHand()
        {
            float x = -3;
            float z = 0;

            for (int i = 0; i < getHandList().Count; i++)
            {
                peekAtCard(i).transform.position = new Vector3(x, -0.5f, z);

                peekAtCard(i).SetActive(true);

                x = x + 1f;
                z = z - 0.1f;
            }

        }

        private void DisplayThreeHand()
        {
            float x = 3;
            float z = 0;

            for (int i = 0; i < getHandList().Count; i++)
            {
                peekAtCard(i).transform.position = new Vector3(x, -0.5f, z);

                peekAtCard(i).SetActive(true);

                x = x + 1f;
                z = z - 0.1f;
            }
        }

        private void DisplayFourHand()
        {
            float x = 7;
            float z = 0;

            for (int i = 0; i < getHandList().Count; i++)
            {
                peekAtCard(i).transform.position = new Vector3(x, -0.5f, z);

                peekAtCard(i).SetActive(true);

                x = x + 1f;
                z = z - 0.1f;
            }
        }

        public string moveDecision() {
            System.Random rand = new System.Random();
            int randIndex = rand.Next(0, 100);
            Debug.Log("random index: " + randIndex);
            if(experienced) {
                if (randIndex < 5) {
                    return "fold";
                } else if(randIndex > 65) {
                    return "raise";
                } else {
                    return "match";
                }
            } else {
                if (randIndex < 10) {
                    return "fold";
                } else if(randIndex > 85) {
                    return "raise";
                } else {
                    return "match";
                }
            }

            return "match";
        }
    }

    //display flop, turn, or river
    private void displayDeckCard(GameObject currentCard) {
        float x = -4 + (displayCount);
        float z = 0;
        displayCount += 2;

        
        currentCard.transform.position = new Vector3(x, 2, z);

        currentCard.SetActive(true);
    }

    private static void flipCard(GameObject currentCard)
    {
        currentCard.SetActive(false);
    }



    public CardPlayer determineRoundWinner(Queue<CardPlayer> tempplayerQueue, List<GameObject> tempplayedCards) {
        CardPlayer currentPlayerCheck = null;
        CardPlayer currentTopPlayer = null;
        List<GameObject> combindedCards = new List<GameObject>();
        List<GameObject> topFiveCards = new List<GameObject>();
        int highestScore = 0;
        int currentScore = 0;
        int winningCard = 0;
        int tempWinningCard = 0;
        int highestCard = 0;
        
        int hasPair(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            foreach (GameObject card in tempCombindedCards)
            {
                numOccurances[card.GetComponent<Card>().numValue - 2]++;
            }
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] >= 2) {
                    tempWinningCard = j + 2;
                    return j + 2 + 20;
                }
            }
            return -1;
        } 
        int hasTwoPair(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            foreach (GameObject card in tempCombindedCards)
            {
                numOccurances[card.GetComponent<Card>().numValue - 2]++;
            }

            int doubleCount = 0;
            int tempScore = 0;
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] >= 2) {
                    tempWinningCard = j + 2;
                    tempScore += j + 2;
                    doubleCount++;
                }
                if(doubleCount == 2) {
                    return tempScore + 40;
                }
            }
            return -1;
        } 
        int hasThree(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            foreach (GameObject card in tempCombindedCards)
            {
                numOccurances[card.GetComponent<Card>().numValue - 2]++;
            }
            
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] >= 3) {
                    tempWinningCard = j + 2;
                    return j + 2 + 120;
                    
                }
            }
            return -1;
        } 
        int hasStraight(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            foreach (GameObject card in tempCombindedCards)
            {
                numOccurances[card.GetComponent<Card>().numValue - 2]++;
            }
            
            int consec = 0;
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] == 0) {
                    consec = 0;
                } else {
                    consec++;
                }
                if(consec == 5) {
                    return j+2+200;
                }
            }
            return -1; 
        } 
        int hasFlush(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[4];
            foreach (GameObject card in tempCombindedCards)
            {
                if(card.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                    numOccurances[0]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                    numOccurances[1]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                    numOccurances[2]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Spades")) {
                    numOccurances[3]++;
                }
            }
            
            for(int j = 3; j >= 0; j--) {
                if(numOccurances[j] >= 5) {
                    return 300;
                } 
            }
            return -1;  
        } 
        int hasFullHouse(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            foreach (GameObject card in tempCombindedCards)
            {
                numOccurances[card.GetComponent<Card>().numValue - 2]++;
            }

            int doubleCount = 0;
            int tripleCount = 0; 
            int tempScore = 0;
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] >= 2) {
                    tempWinningCard = j + 2;
                    tempScore += j + 2;
                    doubleCount++;
                }
                if(numOccurances[j] >= 3) {
                    tempWinningCard = j + 2;
                    tempScore += j + 2;
                    tripleCount++;
                }
                if(doubleCount >= 2 && tripleCount >= 1) {
                    return tempScore + 400;
                }
            }
            return -1;  
        } 
        int hasFour(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            foreach (GameObject card in tempCombindedCards)
            {
                numOccurances[card.GetComponent<Card>().numValue - 2]++;
            }
            
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] >= 3) {
                    tempWinningCard = j + 2;
                    return j + 2 + 500;
                }
            }
            return -1;
        } 
        int hasStraightFlush(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            int[] numSuitOccurances = new int[4];
            foreach (GameObject card in tempCombindedCards)
            {
                if(card.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                    numSuitOccurances[0]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                    numSuitOccurances[1]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                    numSuitOccurances[2]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Spades")) {
                    numSuitOccurances[3]++;
                }
            }

            string flushsuit = "";
            for(int j = 3; j >= 0; j--) {
                if(numSuitOccurances[j] >= 5) {
                    if(j == 0) {
                        flushsuit = "Clubs";
                    } else if(j == 1) {
                        flushsuit = "Hearts";
                    } else if(j == 2) {
                        flushsuit = "Diamonds";
                    } else if(j == 3) {
                        flushsuit = "Spades";
                    }
                } 
            }
            
            foreach (GameObject card in tempCombindedCards)
            {
                if(card.GetComponent<Card>().suitValueString.Equals(flushsuit)) {
                    numOccurances[card.GetComponent<Card>().numValue - 2]++;
                }
            }
            
            int consec = 0;
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] == 0) {
                    consec = 0;
                } else {
                    consec++;
                }
                if(consec == 5) {
                    return j+2+600;
                }
            }

            return -1;
        } 
        int hasRoyalFlush(List<GameObject> tempCombindedCards) {
            int[] numOccurances = new int[13];
            int[] numSuitOccurances = new int[4];
            foreach (GameObject card in tempCombindedCards)
            {
                if(card.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                    numSuitOccurances[0]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                    numSuitOccurances[1]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                    numSuitOccurances[2]++;
                } else if(card.GetComponent<Card>().suitValueString.Equals("Spades")) {
                    numSuitOccurances[3]++;
                }
            }

            string flushsuit = "";
            for(int j = 3; j >= 0; j--) {
                if(numSuitOccurances[j] >= 5) {
                    if(j == 0) {
                        flushsuit = "Clubs";
                    } else if(j == 1) {
                        flushsuit = "Hearts";
                    } else if(j == 2) {
                        flushsuit = "Diamonds";
                    } else if(j == 3) {
                        flushsuit = "Spades";
                    }
                } 
            }
            
            foreach (GameObject card in tempCombindedCards)
            {
                if(card.GetComponent<Card>().suitValueString.Equals(flushsuit)) {
                    numOccurances[card.GetComponent<Card>().numValue - 2]++;
                }
            }
            
            int consec = 0;
            for(int j = 12; j >= 0; j--) {
                if(numOccurances[j] == 0) {
                    consec = 0;
                } else {
                    consec++;
                }
                if(consec == 5 && j == 8) {
                    return j+2+600;
                }
            }

            return -1;
        } 

        for(int i = 0; i < tempplayerQueue.Count; i++) {
            currentPlayerCheck = tempplayerQueue.Dequeue();
            currentScore = 0;

            for(int j = 0; j < tempplayedCards.Count; j++) {
                combindedCards.Add(tempplayedCards[j]);
            }
            for(int j = 0; j < 2; j++) {
                combindedCards.Add(currentPlayerCheck.peekAtCard(j));
                if(j == 0) {
                    currentScore = currentPlayerCheck.peekAtCard(j).GetComponent<Card>().numValue;
                } else {
                    if(currentPlayerCheck.peekAtCard(j).GetComponent<Card>().numValue > currentScore) {
                        currentScore = currentPlayerCheck.peekAtCard(j).GetComponent<Card>().numValue;
                    }
                }
            }
            int tempHighestCard = currentScore;

            int check = hasPair(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasTwoPair(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasThree(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasStraight(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasFlush(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasFullHouse(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasFour(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasStraightFlush(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            check = hasRoyalFlush(combindedCards);
            if(check != -1) {
                currentScore = check;
            }
            
            //check to see if user score is > or equal to top score 
            if(i == 0) {
                currentTopPlayer = currentPlayerCheck;
                highestScore = currentScore;
                winningCard = tempWinningCard;
                highestCard = tempHighestCard;
            } else {
                if(currentScore > highestScore) {
                    highestScore = currentScore;
                    currentTopPlayer = currentPlayerCheck;
                    winningCard = tempWinningCard;
                    highestCard = tempHighestCard;
                } else if (currentScore == highestScore) {
                    if(highestCard < tempHighestCard)
                    {
                        highestScore = currentScore;
                        currentTopPlayer = currentPlayerCheck;
                        winningCard = tempWinningCard;
                        highestCard = tempHighestCard;
                    }
                }
            }
            tempplayerQueue.Enqueue(currentPlayerCheck);
            combindedCards.Clear();
        }
        Debug.Log("Winnig card is: " + winningCard);
        Debug.Log("round winner is User " + currentTopPlayer.getUserID() + " with Score " + highestScore);
        return currentTopPlayer;
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
    public int numbMovesMade = 0;
    public static int deckCount = 0;
    public static int displayCount = 0;
    public int minMoves = 4;
    public int maxNumbRounds = 2;
    public int roundsCompleted = 0;
    /*
        0=Initial,
        1=pick up top card
        2 = choose trump
        3 = play card
    */

    public bool sleeping = false;
    public bool sleepRunning = false;
    public static bool gameOver = false;
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
            cards.transform.SetParent(canvas);

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
        dealQueue.Enqueue(playerOne);
        player1Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerOne.getChipAmount(); 
        playerOne.setCurrentBet(0);
        
        playerTwo = new CardPlayer("" + 2, 200, false, player2Text, player2Bet);
        dealQueue.Enqueue(playerTwo);
        player2Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerTwo.getChipAmount(); 
        playerTwo.setCurrentBet(0);

        playerThree = new CardPlayer("" + 3, 200, false, player3Text, player3Bet);
        dealQueue.Enqueue(playerThree);
        player3Text.GetComponent<TMPro.TextMeshProUGUI>().text = "Chips: " + playerThree.getChipAmount(); 
        playerThree.setCurrentBet(0);

        playerFour = new CardPlayer("" + 4, 200, false, player4Text, player4Bet);
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
                minMoves = dealQueue.Count;
                PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                //Select dealer and big/small blind
                //small blind is 2 chips big blind is 4 chips
                currentDealer = dealQueue.Dequeue();
                dealQueue.Enqueue(currentDealer);

                smallBlind = dealQueue.Dequeue();
                dealQueue.Enqueue(smallBlind);

                if(smallBlind.getChipAmount() < 2) {
                    potValue += smallBlind.getChipAmount();
                    smallBlind.addToBetAmount(smallBlind.getChipAmount());
                } else {
                    smallBlind.setChipAmount(smallBlind.getChipAmount() - 2);
                    smallBlind.setCurrentBet(2);
                    potValue += 2;
                }
                
                PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;

                bigBlind = dealQueue.Dequeue();
                dealQueue.Enqueue(bigBlind);
            
                if(bigBlind.getChipAmount() < 4) {
                    potValue += bigBlind.getChipAmount();
                    bigBlind.addToBetAmount(bigBlind.getChipAmount());
                } else {
                    bigBlind.setChipAmount(bigBlind.getChipAmount() - 4);
                    bigBlind.setCurrentBet(4);
                    potValue += 4;
                }
                PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                matchBet = 4;

                currentPlayer = dealQueue.Peek();

                for(int i = 0; i < dealQueue.Count; i++) {
                    currentPlayer = dealQueue.Dequeue();
                    playerQueue.Enqueue(currentPlayer);
                    dealQueue.Enqueue(currentPlayer);
                }

                for(int i = 0; i < 3; i++) {
                    currentPlayer = playerQueue.Dequeue();
                    playerQueue.Enqueue(currentPlayer);
                }
                currentPlayer = playerQueue.Dequeue();

                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealer is: " + currentDealer.getUserID();
                
                pool = cardDealer.ShuffleCards(pool);

                deckCount = 0;
                Debug.Log("deal queue size: " + dealQueue.Count);
                for(int i = 0; i < dealQueue.Count * 2; i++) {
                    currentPlayer.addToHand(pool[deckCount]);
                    deckCount++;
                    playerQueue.Enqueue(currentPlayer);
                    currentPlayer = playerQueue.Dequeue();
                }
                for(int i = 0; i < dealQueue.Count; i++) {
                    currentPlayer.printHand();
                    playerQueue.Enqueue(currentPlayer);
                    currentPlayer = playerQueue.Dequeue();
                }
                
                currentState = 1;
            } 
            else if (currentState == 1)
            {
                bool folded = false;
                bool validMove = false;
                if(currentPlayer.getIsHuman()) {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = currentPlayer.getUserID() + " match the highest bet, raise, or fold";
                    if(!currentInput.Equals("empty")) {
                        if(currentInput.Equals("fold")) {
                            currentPlayer.clearHand();
                            validMove = true;
                            numbMovesMade++;
                            folded = true;
                            currentInput = "empty";
                        } else {
                            //start of check if number
                            int checkNumber;
                            bool isNumb = int.TryParse(currentInput, out checkNumber);
                            //end of check number    

                            bool validBet = false;

                            if(isNumb == true) {
                                int intInput = int.Parse(currentInput);
                                if(intInput + currentPlayer.getCurrentBet() == matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else if(intInput + currentPlayer.getCurrentBet() > matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput); 
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        matchBet = currentPlayer.getCurrentBet();
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                    
                                } else if(intInput == currentPlayer.getChipAmount()) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else {
                                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: if you are entering a bet it must be within your chip amount";
                                }
                            } else {
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: fold or match/raise the highest bet by entering number";
                            }

                            if(validBet == true) {
                                playerQueue.Enqueue(currentPlayer);
                                numbMovesMade++;
                            }
                            currentInput = "empty";
                            validMove = validBet;
                        }
                    }
                    
                } else {
                    validMove = true;
                    Debug.Log(currentPlayer.getUserID() + " choosing");
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pre Flop betting";
                    string compAction = currentPlayer.moveDecision();
                    
                    if(currentPlayer.getCurrentBet() + currentPlayer.getChipAmount() < matchBet) {
                        compAction = "fold";
                    }
                    //only have coputer match bet for now
                    if(compAction.Equals("match")) {
                        Debug.Log("matched");
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            if(matchBet - currentPlayer.getCurrentBet() == 0) {
                                compAction = "check";
                            }
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                    }

                    if(compAction.Equals("raise")) {
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                        if(currentPlayer.getChipAmount() == 0) {
                            compAction = "match";
                        }
                        System.Random rand = new System.Random();
                        int randIndex = rand.Next(0, currentPlayer.getChipAmount());
                        currentPlayer.addToBetAmount(randIndex);
                        potValue += randIndex;
                        matchBet = currentPlayer.getCurrentBet();
                    }

                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction;
                    Debug.Log("Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction);

                    if(compAction.Equals("fold")) {
                        currentPlayer.clearHand();
                        folded = true;
                    }

                    if(!compAction.Equals("fold")) {
                        playerQueue.Enqueue(currentPlayer);
                    }

                    //get next player and add player back if they didnt fold
                    numbMovesMade++;
                }
                
                bool equalBets = true;
                int firstBet = 0;
                if(folded == true) {
                    for(int i = 0; i < playerQueue.Count; i++) {
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                }

                //if there is only one player remaining
                if(validMove == true && playerQueue.Count == 1) {
                    currentState = 8;
                    numbMovesMade = 0;
                }

                //if playerqueue count is 1 then make that user the winner
                if(validMove == true) {
                    Debug.Log("Player count: " + playerQueue.Count);
                    for(int i = 0; i < playerQueue.Count; i++) {
                        if(i == 0) {
                            firstBet = currentPlayer.getCurrentBet(); 
                        } else {
                            if(firstBet != currentPlayer.getCurrentBet() && currentPlayer.getChipAmount() != 0) {
                                equalBets = false;
                            }
                        }
                        Debug.Log("check player id: " + currentPlayer.getUserID() + " has bet " + currentPlayer.getCurrentBet());
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                    currentPlayer = playerQueue.Dequeue();
                } else {
                    equalBets = false;
                }
                
                if(equalBets == true && numbMovesMade >= minMoves) {
                    currentState = 2;
                    minMoves = playerQueue.Count + 1;
                    numbMovesMade = 0;
                }
                
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
                bool folded = false;
                bool validMove = false;
                if(currentPlayer.getIsHuman()) {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = currentPlayer.getUserID() + " match the highest bet, raise, or fold";
                    if(!currentInput.Equals("empty")) {
                        if(currentInput.Equals("fold")) {
                            currentPlayer.clearHand();
                            validMove = true;
                            numbMovesMade++;
                            folded = true;
                            currentInput = "empty";
                        } else {
                            //start of check if number
                            int checkNumber;
                            bool isNumb = int.TryParse(currentInput, out checkNumber);
                            //end of check number    

                            bool validBet = false;

                            if(isNumb == true) {
                                int intInput = int.Parse(currentInput);
                                if(intInput + currentPlayer.getCurrentBet() == matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else if(intInput + currentPlayer.getCurrentBet() > matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput); 
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        matchBet = currentPlayer.getCurrentBet();
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                    
                                } else if(intInput == currentPlayer.getChipAmount()) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else {
                                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: if you are entering a bet it must be within your chip amount";
                                }
                            } else {
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: fold or match/raise the highest bet by entering number";
                            }

                            if(validBet == true) {
                                playerQueue.Enqueue(currentPlayer);
                                numbMovesMade++;
                            }
                            currentInput = "empty";
                            validMove = validBet;
                        }
                    }
                    
                } else {
                    validMove = true;
                    Debug.Log(currentPlayer.getUserID() + " choosing");
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pre Flop betting";
                    string compAction = currentPlayer.moveDecision();
                    
                    if(currentPlayer.getCurrentBet() + currentPlayer.getChipAmount() < matchBet) {
                        compAction = "fold";
                    }
                    //only have coputer match bet for now
                    if(compAction.Equals("match")) {
                        Debug.Log("matched");
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            if(matchBet - currentPlayer.getCurrentBet() == 0) {
                                compAction = "check";
                            }
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                    }

                    if(compAction.Equals("raise")) {
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                        if(currentPlayer.getChipAmount() == 0) {
                            compAction = "match";
                        }
                        System.Random rand = new System.Random();
                        int randIndex = rand.Next(0, currentPlayer.getChipAmount());
                        currentPlayer.addToBetAmount(randIndex);
                        potValue += randIndex;
                        matchBet = currentPlayer.getCurrentBet();
                    }

                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction;
                    Debug.Log("Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction);

                    if(compAction.Equals("fold")) {
                        currentPlayer.clearHand();
                        folded = true;
                    }

                    if(!compAction.Equals("fold")) {
                        playerQueue.Enqueue(currentPlayer);
                    }

                    //get next player and add player back if they didnt fold
                    numbMovesMade++;
                }
                
                bool equalBets = true;
                int firstBet = 0;
                if(folded == true) {
                    for(int i = 0; i < playerQueue.Count; i++) {
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                }

                //if there is only one player remaining
                if(validMove == true && playerQueue.Count == 1) {
                    currentState = 8;
                    numbMovesMade = 0;
                }

                //if playerqueue count is 1 then make that user the winner
                if(validMove == true) {
                    Debug.Log("Player count: " + playerQueue.Count);
                    for(int i = 0; i < playerQueue.Count; i++) {
                        if(i == 0) {
                            firstBet = currentPlayer.getCurrentBet(); 
                        } else {
                            if(firstBet != currentPlayer.getCurrentBet() && currentPlayer.getChipAmount() != 0) {
                                equalBets = false;
                            }
                        }
                        Debug.Log("check player id: " + currentPlayer.getUserID() + " has bet " + currentPlayer.getCurrentBet());
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                    currentPlayer = playerQueue.Dequeue();
                } else {
                    equalBets = false;
                }
                
                if(equalBets == true && numbMovesMade >= minMoves) {
                    currentState = 4;
                    minMoves = playerQueue.Count + 1;
                    numbMovesMade = 0;
                }
                
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
                bool folded = false;
                bool validMove = false;
                if(currentPlayer.getIsHuman()) {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = currentPlayer.getUserID() + " match the highest bet, raise, or fold";
                    if(!currentInput.Equals("empty")) {
                        if(currentInput.Equals("fold")) {
                            currentPlayer.clearHand();
                            validMove = true;
                            numbMovesMade++;
                            folded = true;
                            currentInput = "empty";
                        } else {
                            //start of check if number
                            int checkNumber;
                            bool isNumb = int.TryParse(currentInput, out checkNumber);
                            //end of check number    

                            bool validBet = false;

                            if(isNumb == true) {
                                int intInput = int.Parse(currentInput);
                                if(intInput + currentPlayer.getCurrentBet() == matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else if(intInput + currentPlayer.getCurrentBet() > matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput); 
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        matchBet = currentPlayer.getCurrentBet();
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                    
                                } else if(intInput == currentPlayer.getChipAmount()) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else {
                                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: if you are entering a bet it must be within your chip amount";
                                }
                            } else {
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: fold or match/raise the highest bet by entering number";
                            }

                            if(validBet == true) {
                                playerQueue.Enqueue(currentPlayer);
                                numbMovesMade++;
                            }
                            currentInput = "empty";
                            validMove = validBet;
                        }
                    }
                    
                } else {
                    validMove = true;
                    Debug.Log(currentPlayer.getUserID() + " choosing");
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pre Flop betting";
                    string compAction = currentPlayer.moveDecision();
                    
                    if(currentPlayer.getCurrentBet() + currentPlayer.getChipAmount() < matchBet) {
                        compAction = "fold";
                    }
                    //only have coputer match bet for now
                    if(compAction.Equals("match")) {
                        Debug.Log("matched");
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            if(matchBet - currentPlayer.getCurrentBet() == 0) {
                                compAction = "check";
                            }
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                    }

                    if(compAction.Equals("raise")) {
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                        if(currentPlayer.getChipAmount() == 0) {
                            compAction = "match";
                        }
                        System.Random rand = new System.Random();
                        int randIndex = rand.Next(0, currentPlayer.getChipAmount());
                        currentPlayer.addToBetAmount(randIndex);
                        potValue += randIndex;
                        matchBet = currentPlayer.getCurrentBet();
                    }

                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction;
                    Debug.Log("Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction);

                    if(compAction.Equals("fold")) {
                        currentPlayer.clearHand();
                        folded = true;
                    }

                    if(!compAction.Equals("fold")) {
                        playerQueue.Enqueue(currentPlayer);
                    }

                    //get next player and add player back if they didnt fold
                    numbMovesMade++;
                }
                
                bool equalBets = true;
                int firstBet = 0;
                if(folded == true) {
                    for(int i = 0; i < playerQueue.Count; i++) {
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                }

                //if there is only one player remaining
                if(validMove == true && playerQueue.Count == 1) {
                    currentState = 8;
                    numbMovesMade = 0;
                }

                //if playerqueue count is 1 then make that user the winner
                if(validMove == true) {
                    Debug.Log("Player count: " + playerQueue.Count);
                    for(int i = 0; i < playerQueue.Count; i++) {
                        if(i == 0) {
                            firstBet = currentPlayer.getCurrentBet(); 
                        } else {
                            if(firstBet != currentPlayer.getCurrentBet() && currentPlayer.getChipAmount() != 0) {
                                equalBets = false;
                            }
                        }
                        Debug.Log("check player id: " + currentPlayer.getUserID() + " has bet " + currentPlayer.getCurrentBet());
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                    currentPlayer = playerQueue.Dequeue();
                } else {
                    equalBets = false;
                }
                
                if(equalBets == true && numbMovesMade >= minMoves) {
                    currentState = 6;
                    minMoves = playerQueue.Count + 1;
                    numbMovesMade = 0;
                }
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
                bool folded = false;
                bool validMove = false;
                if(currentPlayer.getIsHuman()) {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = currentPlayer.getUserID() + " match the highest bet, raise, or fold";
                    if(!currentInput.Equals("empty")) {
                        if(currentInput.Equals("fold")) {
                            currentPlayer.clearHand();
                            validMove = true;
                            numbMovesMade++;
                            folded = true;
                            currentInput = "empty";
                        } else {
                            //start of check if number
                            int checkNumber;
                            bool isNumb = int.TryParse(currentInput, out checkNumber);
                            //end of check number    

                            bool validBet = false;

                            if(isNumb == true) {
                                int intInput = int.Parse(currentInput);
                                if(intInput + currentPlayer.getCurrentBet() == matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else if(intInput + currentPlayer.getCurrentBet() > matchBet) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput); 
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        matchBet = currentPlayer.getCurrentBet();
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                    
                                } else if(intInput == currentPlayer.getChipAmount()) {
                                    bool checkInput = currentPlayer.addToBetAmount(intInput);
                                    if(checkInput == true) {
                                        potValue += intInput;
                                        PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                                        validBet = true;
                                    } else {
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: cannot bet more than you have";
                                    }
                                } else {
                                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: if you are entering a bet it must be within your chip amount";
                                }
                            } else {
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Invalid input: fold or match/raise the highest bet by entering number";
                            }

                            if(validBet == true) {
                                playerQueue.Enqueue(currentPlayer);
                                numbMovesMade++;
                            }
                            currentInput = "empty";
                            validMove = validBet;
                        }
                    }
                    
                } else {
                    validMove = true;
                    Debug.Log(currentPlayer.getUserID() + " choosing");
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pre Flop betting";
                    string compAction = currentPlayer.moveDecision();
                    
                    if(currentPlayer.getCurrentBet() + currentPlayer.getChipAmount() < matchBet) {
                        compAction = "fold";
                    }
                    //only have coputer match bet for now
                    if(compAction.Equals("match")) {
                        Debug.Log("matched");
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            if(matchBet - currentPlayer.getCurrentBet() == 0) {
                                compAction = "check";
                            }
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                    }

                    if(compAction.Equals("raise")) {
                        if(currentPlayer.getCurrentBet() < matchBet) {
                            potValue += matchBet - currentPlayer.getCurrentBet();
                            currentPlayer.addToBetAmount(matchBet - currentPlayer.getCurrentBet());
                            Debug.Log(potValue);
                            PotText.GetComponent<TMPro.TextMeshProUGUI>().text = "Pot Value: " + potValue;
                        }
                        if(currentPlayer.getChipAmount() == 0) {
                            compAction = "match";
                        }
                        System.Random rand = new System.Random();
                        int randIndex = rand.Next(0, currentPlayer.getChipAmount());
                        currentPlayer.addToBetAmount(randIndex);
                        potValue += randIndex;
                        matchBet = currentPlayer.getCurrentBet();
                    }

                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction;
                    Debug.Log("Computer opponent " + currentPlayer.getUserID() + " chose to " + compAction);

                    if(compAction.Equals("fold")) {
                        currentPlayer.clearHand();
                        folded = true;
                    }

                    if(!compAction.Equals("fold")) {
                        playerQueue.Enqueue(currentPlayer);
                    }

                    //get next player and add player back if they didnt fold
                    numbMovesMade++;
                }
                
                bool equalBets = true;
                int firstBet = 0;
                if(folded == true) {
                    for(int i = 0; i < playerQueue.Count; i++) {
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                }

                //if there is only one player remaining
                if(validMove == true && playerQueue.Count == 1) {
                    currentState = 8;
                    numbMovesMade = 0;
                }

                //if playerqueue count is 1 then make that user the winner
                if(validMove == true) {
                    Debug.Log("Player count: " + playerQueue.Count);
                    for(int i = 0; i < playerQueue.Count; i++) {
                        if(i == 0) {
                            firstBet = currentPlayer.getCurrentBet(); 
                        } else {
                            if(firstBet != currentPlayer.getCurrentBet() && currentPlayer.getChipAmount() != 0) {
                                equalBets = false;
                            }
                        }
                        Debug.Log("check player id: " + currentPlayer.getUserID() + " has bet " + currentPlayer.getCurrentBet());
                        currentPlayer = playerQueue.Dequeue();
                        playerQueue.Enqueue(currentPlayer);
                    }
                    currentPlayer = playerQueue.Dequeue();
                } else {
                    equalBets = false;
                }
                
                if(equalBets == true && numbMovesMade >= minMoves) {
                    currentState = 8;
                    minMoves = playerQueue.Count + 1;
                    numbMovesMade = 0;
                }
            }
            else if (currentState == 8) //show player cards and determine winner
            {
                playerQueue.Enqueue(currentPlayer);
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Checking winner";
                //if there is only one player remaining
                CardPlayer roundWinner; 
                Debug.Log(playerQueue.Count);
                if(playerQueue.Count == 1) {
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Player " + currentPlayer.getUserID() + " wins the hand and a pot of " + potValue;
                    currentPlayer.addToChipAmount(potValue);
                } else {
                    roundWinner = determineRoundWinner(playerQueue, playedCards);
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Player " + roundWinner.getUserID() + " wins the hand and a pot of " + potValue;
                    roundWinner.addToChipAmount(potValue);
                }

                Debug.Log("Show winner");
                currentState = 9;
            }
            else if (currentState == 9) //reset pool and betting values and move to next dealer
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Preparing for next hand";
                Debug.Log("reset values");
                displayCount = 0;
                for(int i = 0; i < playedCards.Count; i++) {
                    flipCard(playedCards[i]);
                }
                playedCards.Clear();
                Debug.Log("cleared");
                deckCount = 0;
                
                int queueLength = dealQueue.Count;
                for(int i = 0; i < queueLength; i++) {
                    currentPlayer = dealQueue.Dequeue();
                    Debug.Log(currentPlayer.getHandList().Count);
                    currentPlayer.clearHand();
                    currentPlayer.setCurrentBet(0);
                    if(currentPlayer.getChipAmount() > 0) {
                        dealQueue.Enqueue(currentPlayer);
                    }
                }
                playerQueue.Clear();
                roundsCompleted++;
                currentState = 0;
            }
            
        }

    }

    IEnumerator sleepFunction()
    {
        sleeping = true;
        yield return new WaitForSeconds(2);
        sleeping = false;
        sleepRunning = false;
    }

    public bool checkForWinner()
    {
        maxNumbRounds = 3;
        Debug.Log("rounds completed: " + roundsCompleted);
        Debug.Log("maxNumbRounds: " + maxNumbRounds);
        if(dealQueue.Count == 1) {
            currentPlayer = dealQueue.Dequeue();
            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Poker game winner is player " + currentPlayer.getUserID();
            gameOver = true;
        } else if(roundsCompleted >= maxNumbRounds) {
            CardPlayer currHighestPlayer = null;
    
            for(int i = 0; i < dealQueue.Count; i++) {
                currentPlayer = dealQueue.Dequeue();
                if(i == 0) {
                    currHighestPlayer = currentPlayer;
                } else {
                    if(currentPlayer.getChipAmount() > currHighestPlayer.getChipAmount()) {
                        currHighestPlayer = currentPlayer;
                    }
                }
                dealQueue.Enqueue(currentPlayer);
            }
            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Poker game winner is player " + currHighestPlayer.getUserID() + " reached max number of rounds " + maxNumbRounds;
            gameOver = true;
        }
        return gameOver;
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
