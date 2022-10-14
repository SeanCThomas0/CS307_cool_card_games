using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEuchre : MonoBehaviour
{
    public GameObject UserPlayer;
    public GameObject GameManager;
    public GameObject GameMessages;
    public GameObject CardDealer;
    public GameObject TrumpHolder;
    public GameObject DealerHolder;
    private CardDealer cardDealer;

    /*Temporary card class for testing to be replaced with CardDealer script*/
    public float userXPos = -7;
    public float userYPos = -124;

    public class CardTest {
        private int numbValue;
        private string faceValue;
        private string suit;

        public CardTest(int numbValue, int suit) {
            this.numbValue = numbValue;
            if(numbValue == 11) {
                this.faceValue = "Jack";
            } else if(numbValue == 12) {
                this.faceValue = "Queen";
            } else if(numbValue == 13) {
                this.faceValue = "King";
            } else if(numbValue == 14) {
                this.faceValue = "Ace";
            } else {
                this.faceValue = ("" + numbValue);
            }

            if(suit == 1) {
                this.suit = "Clubs";
            } else if(suit == 2) {
                this.suit = "Diamonds";
            } else if(suit == 3) {
                this.suit = "Hearts";
            } else if(suit == 4) {
                this.suit = "Spades";
            }
        }

        public int numValue() {
            return numbValue;
        }

        public string getFaceValue() {
            return faceValue;
        }
        
        public string suitValue() {
            return suit;
        }

    }

    /*CardPlayer class for individual game players*/
    public class CardPlayer {
        private List<GameObject> hand;
        private string userID;
        private int teamNumber;
        private bool isHuman;

        public CardPlayer(string userID, int teamNumber, bool isHuman) {
            hand = new List<GameObject>();
            this.userID = userID;
            this.teamNumber = teamNumber;
            this.isHuman = isHuman;
        }

        public List<GameObject> getHandList() {
            return hand;
        }
        
        public string getUserID() {
            return userID;
        }

        public int getTeamNumber() {
            return teamNumber; 
        }

        public bool getIsHuman() {
            return isHuman;
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

        private bool getJackPair(string trumpSuit, GameObject possibleJack) {
            if (trumpSuit.Equals("Clubs") && possibleJack.GetComponent<Card>().suitValueString.Equals("Spades")) {
                return true;
            } else if (trumpSuit.Equals("Spades") && possibleJack.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                return true;
            } else if (trumpSuit.Equals("Hearts") && possibleJack.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                return true;
            } else if (trumpSuit.Equals("Diamonds") && possibleJack.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                return true;
            } else {
                return false;
            }
        }
        
        public bool hasLeadSuit(string leadSuit, string trumpSuit) {
            bool leadSuitFound = false;
            for (int i = 0; i < hand.Count; i++) {
                GameObject currentCard = (GameObject) hand[i];
                if (currentCard.GetComponent<Card>().suitValueString.Equals(leadSuit)) {
                    leadSuitFound = true;
                } else if (leadSuit.Equals(trumpSuit) && getJackPair(trumpSuit, currentCard)) {
                    leadSuitFound = true;
                }
            }
            return leadSuitFound;
        }

        public GameObject peekAtCard(int indexInHand) {
            GameObject returnCard = (GameObject) hand[indexInHand];
            return returnCard;
        }

        /*gets card value and removes it from the User's hand*/
        public GameObject playCard(int indexInHand) {
            GameObject returnCard = (GameObject) hand[indexInHand];
            removeFromHand(indexInHand);
            return returnCard;
        }

        private void adjustDecision(int[] suits, int[] totalCardValue, int[] avgCardValue, GameObject currCard, int index) {
            suits[index]++;
            if (currCard.GetComponent<Card>().numValue == 11) {
                totalCardValue[index] += 15;
            } else {
                totalCardValue[index] += currCard.GetComponent<Card>().numValue;
            }
            avgCardValue[index] = totalCardValue[index]/suits[index];
        }

        public string pickUpDecision(GameObject topCard, CardPlayer dealer) {
            string topSuit = topCard.GetComponent<Card>().suitValueString;
            bool dealingTeam;
            double decisionScore = 0;

            if (!(dealer.getTeamNumber() == teamNumber) || !(dealer.getUserID().Equals(userID))) {
                dealingTeam = false;
            } else {
                dealingTeam = true;
            }
            //1 clubs, 2 diamonds, 3 hearts, 4 spades
            int[] suits = new int[4];
            int[] totalCardValue = new int[4];
            int[] avgCardValue = new int[4];

            for (int i = 0; i < 5; i++) {
                GameObject currCard = (GameObject) hand[i];
                if (currCard.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 0);
                } else if (currCard.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 1);
                } else if (currCard.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 2);
                } else if (currCard.GetComponent<Card>().suitValueString.Equals("Spades")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 3);
                }
            }
            int trumpIndex = 0;
            if (topCard.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                decisionScore = avgCardValue[0] + 1;
                trumpIndex = 0;
            } else if (topCard.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                decisionScore = avgCardValue[1] + 1;
                trumpIndex = 1;
            } else if (topCard.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                decisionScore = avgCardValue[2] + 1;
                trumpIndex = 2;
            } else if (topCard.GetComponent<Card>().suitValueString.Equals("Spades")) {
                decisionScore = avgCardValue[3] + 1;
                trumpIndex = 3;
            }
            suits[trumpIndex]++;

            if (topCard.GetComponent<Card>().numValue == 11 || topCard.GetComponent<Card>().numValue == 13 || topCard.GetComponent<Card>().numValue == 14) {
                if (dealingTeam) {
                    decisionScore++;
                } else {
                    decisionScore--;
                }
            }

            if (decisionScore >= 12.5 && suits[trumpIndex] >= 3) {
                return "Pick";
            } else {
                return "Pass";
            }
        }

        private String suitValue(int place) {
            //0 is clubs, 1 diamonds, 2 hearts, 3 spades
            if (place == 0) {
                return "Clubs";
            } else if (place == 1) {
                return "Diamonds";
            } else if (place == 2) {
                return "Hearts";
            } else if (place == 3) {
                return "Spades";
            } else {
                return "error";
            }
        }

        //method to have the user pick up the card if they are a dealer
        public void pickUpCard (GameObject topCard, string trumpSuit) {
            int[] handScores = new int[5];
            int count = 0;
            int currentWorst = 0;
            foreach(GameObject currCard in hand) {
                if (currCard.GetComponent<Card>().suitValueString.Equals(trumpSuit)) {
                    handScores[count] += 15 + currCard.GetComponent<Card>().numValue;
                } else {
                    handScores[count] += currCard.GetComponent<Card>().numValue;
                }
                if (handScores[count] < handScores[currentWorst]) {
                    currentWorst = count;
                }
                count++;
            }
            GameObject cardToHide = playCard(currentWorst);
            cardToHide.SetActive(false);
            addToHand(topCard);
        }

        //method to have a computer choose whether to choose trump or not
        public string suitOrPass(CardPlayer dealer) {
            int decisionScore = 0;
            string wouldBeSuit;

            //0 is clubs, 1 diamonds, 2 hearts, 3 spades
            int[] suits = new int[4];
            int[] totalCardValue = new int[4];
            int[] avgCardValue = new int[4];
            foreach(GameObject currCard in hand) {
                if (currCard.GetComponent<Card>().suitValueString.Equals("Clubs")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 0);
                } else if (currCard.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 1);
                } else if (currCard.GetComponent<Card>().suitValueString.Equals("Hearts")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 2);
                } else if (currCard.GetComponent<Card>().suitValueString.Equals("Spades")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 3);
                }
            }

            int maxTotal = 0;
            int trumpIndex = 0;
            for (int i = 0; i < avgCardValue.Length; i++) {
                if (totalCardValue[i] > maxTotal) {
                    maxTotal = totalCardValue[i];
                    decisionScore = avgCardValue[i];
                    trumpIndex = i;
                }
            }

            wouldBeSuit = suitValue(trumpIndex);
            if (dealer.getUserID().Equals(userID)) {
                return wouldBeSuit;
            } else {
                if (decisionScore >= 12 && suits[trumpIndex] >= 3) {
                    return wouldBeSuit;
                } else {
                    return "Pass";
                }
            }

        }

        //have computer choose to which card to play
        public GameObject chooseCardToPlay(GameObject[] cardsPlayed, string trumpSuit, int numbPlayed) {
            int playedCardIndex = 0;
            int bestCardIndex = 0;
            int worstCardIndex = 0;
            int count = 0;
            int partnerIndex = -1;
            int[] handScores = new int[5];
            string leadSuit = "empty";

            if (numbPlayed == 2) {
                partnerIndex = 0;
            } else if (numbPlayed == 3) {
                partnerIndex = 1;
            }

            if (numbPlayed == 0) {
                foreach(GameObject currCard in hand) {
                    if (currCard.GetComponent<Card>().suitValueString.Equals(trumpSuit)) {
                        handScores[count] += currCard.GetComponent<Card>().numValue;
                    } else {
                        handScores[count] += 4 + currCard.GetComponent<Card>().numValue;
                    }
                    if (handScores[count] > handScores[bestCardIndex]) {
                        bestCardIndex = count;
                    }
                    count++;
                }
                playedCardIndex = bestCardIndex;
            } else {
                int[] playedCardsValue = new int[4];
                int topCard = 0;
                GameObject leadCard = cardsPlayed[0];

                if (leadCard.GetComponent<Card>().faceValue.Equals("Jack")) {
                    if (getJackPair(trumpSuit, leadCard)) {
                        leadSuit = trumpSuit;
                    }
                } else {
                    leadSuit = cardsPlayed[0].GetComponent<Card>().suitValueString;
                }
                bool leadSuitFound = false;

                foreach(GameObject playedCard in cardsPlayed) {
                    if (playedCard == null) {
                        break;
                    }
                    if (playedCard.GetComponent<Card>().suitValueString.Equals(trumpSuit)) {
                        playedCardsValue[count] += 15 + playedCard.GetComponent<Card>().numValue;
                    } else {
                        playedCardsValue[count] += playedCard.GetComponent<Card>().numValue;
                    }
                    if (playedCardsValue[count] > playedCardsValue[topCard]) {
                        topCard = count;
                    }
                    count++;
                }

                count = 0;
                foreach(GameObject currCard in hand) {
                    if (currCard.GetComponent<Card>().suitValueString.Equals(leadSuit)) {
                        handScores[count] += 100;
                        leadSuitFound = true;
                    }
                    if (currCard.GetComponent<Card>().suitValueString.Equals(trumpSuit)) {
                        handScores[count] += 15 + currCard.GetComponent<Card>().numValue;
                    } else {
                        handScores[count] += currCard.GetComponent<Card>().numValue;
                    }

                    if (handScores[count] > handScores[bestCardIndex]) {
                        bestCardIndex = count;
                    } else if(handScores[count] < handScores[bestCardIndex]) {
                        worstCardIndex = count;
                    }
                    count++;
                }
                if (leadSuitFound == true) {
                    playedCardIndex = bestCardIndex;
                } else if (partnerIndex != 0 && topCard == partnerIndex) {
                    playedCardIndex = worstCardIndex;
                } else if (handScores[bestCardIndex] > playedCardsValue[topCard]) {
                    playedCardIndex = bestCardIndex;
                } else {
                    playedCardIndex = worstCardIndex;
                }
            }

            return removeFromHand(playedCardIndex);
        }
    }
    
    private static bool getJackPair(string trumpSuit, GameObject possibleJack) {
        if (trumpSuit.Equals("Clubs") && possibleJack.GetComponent<Card>().suitValueString.Equals("Spades")) {
            return true;
        } else if (trumpSuit.Equals("Spades") && possibleJack.GetComponent<Card>().suitValueString.Equals("Clubs")) {
            return true;
        } else if (trumpSuit.Equals("Hearts") && possibleJack.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
            return true;
        } else if (trumpSuit.Equals("Diamonds") && possibleJack.GetComponent<Card>().suitValueString.Equals("Hearts")) {
            return true;
        } else {
            return false;
        }
    }

    

    public bool followSuitCheck (string leadSuit, GameObject cardPlayed, string trumpSuit) {
        if (getCardSuit(cardPlayed, trumpSuit).Equals(leadSuit)) {
            return true;
        }
        return false;
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

    private void DisplayTopCard(GameObject currentCard)
    {
        float x = userXPos + 2;
        float z = 0;

        currentCard.transform.position = new Vector3(x, -4, z);
        currentCard.SetActive(true);   
    }

    private void flipCard(GameObject currentCard) {
        currentCard.SetActive(false);
    }

    /*
    public List<Card> shuffle(List<Card> deck)
    {
        List<Card> randomCards = new List<Card>();
        System.Random rand = new System.Random();
        while (deck.Count > 0)
        {
            int index = rand.Next(0, deck.Count);
            randomCards.Add(deck[index]); 
            deck.RemoveAt(index);
        }
        return randomCards;
    }
    */

    public string currentInput = "empty";
    //public bool notEmpty = false;
    public List<Card> cardDeck = new List<Card>();
    public Queue<CardPlayer> playerQueue = new Queue<CardPlayer>();
    public CardPlayer playerOne;
    public CardPlayer playerTwo;
    public CardPlayer playerThree;
    public CardPlayer playerFour;
    public int teamOneScore = 0;
    public int teamTwoScore = 0;
    public int oneTrickScore = 0;
    public int twoTrickScore = 0;
    public CardPlayer dealer = null;
    public CardPlayer currentPlayer = null;
    public int gameStep = 0;
    public int currentState = 0;
    /*
        0=Initial,
        1=pick up top card
        2 = choose trump
        3 = play card
    */
    public int pickCount = 0;
    public int trumpCount = 0;
    public string trumpSuit = "";
    public int turnCount = 0; //track how many turns have been taken
    public int trickCount = 0; //count how many tricks out of 5 have been played
    public int trumpChosingTeam = 1; //track who chose trump
    int cardCount = 0; //used to track how many cards have been played in a trick
    public GameObject[] trickCards = new GameObject[4]; //array for cards played in a given trick
    public GameObject topCard = null;
    public int[] teamID = new int[4];
    public bool printed = false;
    public bool sleeping = false;
    public bool sleepRunning = false;
    public List<GameObject> pool;
    public List<GameObject> pool1;
    public List<GameObject> pool2;
    

    // Start is called before the first frame update
    void Start()
    {
        cardDealer = CardDealer.GetComponent<CardDealer>();
        
        Debug.Log("Euchre Game Starting");
        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Euchre Game Starting";
        pool1 = cardDealer.RandomCards(4, 1, 1, true, true, true, true);
        pool2 = cardDealer.RandomCards(20, 9, 13, true, true, true, true);
        foreach(GameObject cards in pool1) {
            cards.GetComponent<Card>().numValue = 14;
            Debug.Log(cards.GetComponent<Card>().numValue + " of " + cards.GetComponent<Card>().suitValue);
            cards.GetComponent<Card>().faceValue = "Ace";
            //convert enums
            Debug.Log(cards.GetComponent<Card>().suitValue);
            Debug.Log(cards.GetComponent<Card>().suitValue == Card.suit.CLUBS);
            if(cards.GetComponent<Card>().suitValue == Card.suit.CLUBS) {
                cards.GetComponent<Card>().suitValueString = "Clubs";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.HEARTS) {
                cards.GetComponent<Card>().suitValueString = "Hearts";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.DIAMONDS) {
                cards.GetComponent<Card>().suitValueString = "Diamonds";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.SPADES) {
                cards.GetComponent<Card>().suitValueString = "Spades";
            }

            pool.Add(cards);
        }
        foreach(GameObject cards in pool2) {
            Debug.Log(cards.GetComponent<Card>().numValue + " of " + cards.GetComponent<Card>().suitValue);
            if(cards.GetComponent<Card>().numValue == 9) {
                cards.GetComponent<Card>().faceValue = "9";
            } else if(cards.GetComponent<Card>().numValue == 10) {
                cards.GetComponent<Card>().faceValue = "10";
            } else if(cards.GetComponent<Card>().numValue == 11) {
                cards.GetComponent<Card>().faceValue = "Jack";
            } else if(cards.GetComponent<Card>().numValue == 12) {
                cards.GetComponent<Card>().faceValue = "Queen";
            } else if(cards.GetComponent<Card>().numValue == 13) {
                cards.GetComponent<Card>().faceValue = "King";
            }

            if(cards.GetComponent<Card>().suitValue == Card.suit.CLUBS) {
                cards.GetComponent<Card>().suitValueString = "Clubs";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.HEARTS) {
                cards.GetComponent<Card>().suitValueString = "Hearts";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.DIAMONDS) {
                cards.GetComponent<Card>().suitValueString = "Diamonds";
            } else if(cards.GetComponent<Card>().suitValue == Card.suit.SPADES) {
                cards.GetComponent<Card>().suitValueString = "Spades";
            }
            pool.Add(cards);
        }

        /*create cards needed for game*/
        /*
        for(int i = 9; i <= 14; i++) {
            for(int j = 1; j <= 4; j++) {
                cardDeck.Add(new Card(i, j));
            }
        }
        
        cardDeck = shuffle(cardDeck);
        foreach(Card addedCard in cardDeck) {
            string tempFace = addedCard.faceValue;
            string tempSuit = addedCard.suitValue;
            string message = "Added card: " + tempFace + " of " + tempSuit + "\n" ; 
            //Debug.Log(message);
        }
        */
        //Debug.Log(cardDeck.Count);
        pool = cardDealer.ShuffleCards(pool);
        
        int oneCount = 0;
        int twoCount = 0;

        /*create card player objects with userID's*/
        for(int i = 1; i <= 4; i++) {
            if(i % 2 == 1) {
                if(i == 1) {
                    playerOne = new CardPlayer("" + i, 1, true);
                    playerQueue.Enqueue(playerOne);
                } else {
                    playerThree = new CardPlayer("" + i, 1, false);
                    playerQueue.Enqueue(playerThree);
                }
                oneCount++;
            } else if(i % 2 == 0) {
                if(i == 2) {
                    playerTwo = new CardPlayer("" + i, 2, false);
                    playerQueue.Enqueue(playerTwo);
                } else {
                    playerFour = new CardPlayer("" + i, 2, false);
                    playerQueue.Enqueue(playerFour);
                }
                twoCount++;
            }
        }
        
    }

    private bool getJackValue(string trumpSuit, GameObject jack) {
        if (trumpSuit.Equals("Clubs") && jack.GetComponent<Card>().suitValueString.Equals("Spades")) {
            return true;
        } else if (trumpSuit.Equals("Spades") && jack.GetComponent<Card>().suitValueString.Equals("Clubs")) {
            return true;
        } else if (trumpSuit.Equals("Hearts") && jack.GetComponent<Card>().suitValueString.Equals("Diamonds")) {
            return true;
        } else if (trumpSuit.Equals("Diamonds") && jack.GetComponent<Card>().suitValueString.Equals("Hearts")) {
            return true;
        } else {
            return false;
        }
    }

    private string getCardSuit(GameObject firstCard, string trumpSuit) {
        if (firstCard.GetComponent<Card>().faceValue.Equals("Jack") && getJackValue(trumpSuit, firstCard)) {
            return trumpSuit;
        } else {
            return firstCard.GetComponent<Card>().suitValueString;
        }
    }

    private int getTrickWinner (string trumpSuit, GameObject[] playedCards) {
        int maxScore = 0;
        int maxIndex = 0;
        string leadSuit = getCardSuit(playedCards[0], trumpSuit);

        for (int cardIndex = 0; cardIndex < 4; cardIndex++) {
            int currentScore = 0;
            flipCard(playedCards[cardIndex]);
            if (playedCards[cardIndex].GetComponent<Card>().suitValueString.Equals(leadSuit)) {
                currentScore += 16;
            }
            if (playedCards[cardIndex].GetComponent<Card>().suitValueString.Equals(trumpSuit)) {
                currentScore += 32;
                if (playedCards[cardIndex].GetComponent<Card>().faceValue.Equals("Jack")) {
                    currentScore += 16;
                } else {
                    currentScore += playedCards[cardIndex].GetComponent<Card>().numValue;
                }
            } else if (playedCards[cardIndex].GetComponent<Card>().faceValue.Equals("Jack") && getJackValue(trumpSuit, playedCards[cardIndex])) {
                currentScore += 47; 
            } else {
                currentScore += playedCards[cardIndex].GetComponent<Card>().numValue;
            }
            if (currentScore > maxScore) {
                maxIndex = cardIndex;
                maxScore = currentScore;
            }
        }

        return maxIndex;
    }
    /* end of functions to calculate winner of a given trick*/

    void gameDecision() {
        bool finished = checkForWinner();
        if(!finished) {
            //if the game does not have a winner
            if(currentState == 0) { //shuffle cards and deal them out
                pool = cardDealer.ShuffleCards(pool);
                currentInput = "empty";

                dealer = playerQueue.Dequeue();
                Debug.Log("Dealer is: " + dealer.getUserID());
                DealerHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "Player " + dealer.getUserID();
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealer is: " + dealer.getUserID();
                playerQueue.Enqueue(dealer);
                //yield return new WaitForSeconds(2);
                /* deal cards to each player */
                int deckCount = 0;
                for(int dealNumb = 0; dealNumb < 4; dealNumb++) {
                    currentPlayer = playerQueue.Dequeue();
                    for(int handCount = 0; handCount < 5; handCount++) {
                        currentPlayer.addToHand(pool[deckCount]);
                        deckCount++;
                    }
                    playerQueue.Enqueue(currentPlayer);
                }
                topCard = pool[deckCount];
                Debug.Log("top card is: " + topCard.GetComponent<Card>().faceValue + " of " + topCard.GetComponent<Card>().suitValueString);
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "top card is: " + topCard.GetComponent<Card>().faceValue + " of " + topCard.GetComponent<Card>().suitValueString;
                DisplayTopCard(topCard);
                currentPlayer = playerQueue.Dequeue();
                currentState = 1;

            } if(currentState == 1) { //have players choose to pick up top card 
                if(pickCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        if(printed == false) {
                            currentPlayer.printHand();
                            DisplayOneHand(currentPlayer);
                            printed = true;
                            Debug.Log("Choose to pick up top card or pass");
                            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Choose to pick up top card or pass";
                        }
                        //wait for user input
                        if(!currentInput.Equals("empty")) {
                            //Debug.Log("pickCount: " + pickCount);
                            if(!(currentInput.Equals("pick") || currentInput.Equals("pick up") || currentInput.Equals("pass"))) {
                                Debug.Log("Enter a valid input (pick, pick up, or pass)");
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Enter a valid input (pick, pick up, or pass)";
                                currentInput = "empty";
                            } else if(currentInput.Equals("pick") || currentInput.Equals("pick up")) {
                                pickCount++;
                                trumpSuit = topCard.GetComponent<Card>().suitValueString;
                                TrumpHolder.GetComponent<TMPro.TextMeshProUGUI>().text = trumpSuit;
                                dealer.pickUpCard(topCard, trumpSuit);
                                if(dealer.getIsHuman()) {
                                    DisplayOneHand(dealer);
                                }
                                //Debug.Log("current move " + currentPlayer.getUserID());
                                Debug.Log("GameManager recieved User pick/pass: " + currentInput);
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved User pick/pass: " + currentInput;
                                //Debug.Log("turn numb: " + pickCount);
                                currentInput = "empty";
                                while (!currentPlayer.getUserID().Equals(dealer.getUserID())) {
                                    playerQueue.Enqueue(currentPlayer);
                                    currentPlayer = playerQueue.Dequeue();
                                }
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                                pickCount = 20;
                                //yield return new WaitForSeconds(2);
                            } else {
                                pickCount++;
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                                currentInput = "empty";
                            }
                        }
                    } else {
                        //Debug.Log("pickCount: " + pickCount);
                        //have computer choose whether to pick up or pass
                        string computerChoice = currentPlayer.pickUpDecision(topCard, dealer);
                        Debug.Log("GameManager recieved Computer " + currentPlayer.getUserID() + " top card: " + computerChoice);
                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved Computer " + currentPlayer.getUserID() + " top card: " + computerChoice;
                        pickCount++;
                        //Debug.Log("current move " + currentPlayer.getUserID());
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        if(computerChoice.Equals("Pick")) {
                            Debug.Log("pick ran" );
                            trumpSuit = topCard.GetComponent<Card>().suitValueString;
                            TrumpHolder.GetComponent<TMPro.TextMeshProUGUI>().text = trumpSuit;
                            dealer.pickUpCard(topCard, trumpSuit);

                            if(dealer.getIsHuman()) {
                                DisplayOneHand(dealer);
                            }
                            while (!currentPlayer.getUserID().Equals(dealer.getUserID())) {
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                            }
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                            pickCount = 20;
                        }
                        //yield return new WaitForSeconds(2);
                    }
                } else {
                    printed = false;
                    if(pickCount == 20) {
                        if(!dealer.getIsHuman()) {
                            flipCard(topCard);
                        }
                        currentState = 3;
                        pickCount = 0;
                    } else {
                        flipCard(topCard);
                        currentState = 2;
                        pickCount = 0;
                    }
                }
            } if(currentState == 2) { //have players choose trump (dealer has to if no one else will) 
                if(trumpCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        if(printed == false) {
                            currentPlayer.printHand();
                            DisplayOneHand(currentPlayer);
                            Debug.Log("Pick a trump suit or pass");
                            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pick a trump suit or pass";
                            printed = true;
                        }
                        //wait for user input
                        if(!currentInput.Equals("empty")) {
                            //Debug.Log("trump count: " + trumpCount);
                            if(!((currentInput.Equals("Hearts") || currentInput.Equals("Spades") || currentInput.Equals("Clubs") || currentInput.Equals("Diamonds") || currentInput.Equals("pass")))) {
                                Debug.Log("Enter a valid input (Hearts, Spades, Clubs, Diamonds, or pass)");
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Enter a valid input (Hearts, Spades, Clubs, Diamonds, or pass)";
                                currentInput = "empty";
                            } else if(trumpCount == 3 && currentInput.Equals("pass")) {
                                Debug.Log("cannot pass when you are the dealer, choose a valid suit");
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "cannot pass when you are the dealer, choose a valid suit";
                                currentInput = "empty";
                            } else {
                                trumpCount++;
                                trumpSuit = currentInput;
                                TrumpHolder.GetComponent<TMPro.TextMeshProUGUI>().text = trumpSuit;
                                Debug.Log("GameManager recieved trump pick " + currentInput + " from user: " + currentPlayer.getUserID());
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved trump pick " + currentInput + " from user: " + currentPlayer.getUserID();
                                currentInput = "empty";
                                while (!currentPlayer.getUserID().Equals(dealer.getUserID())) {
                                    playerQueue.Enqueue(currentPlayer);
                                    currentPlayer = playerQueue.Dequeue();
                                }
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                                trumpCount = 5;
                            }
                            //yield return new WaitForSeconds(2);
                        }
                    } else {
                        //Debug.Log("trump count: " + trumpCount);
                        //have computer choose trump suit or pass
                        string trumpChoice = currentPlayer.suitOrPass(dealer);
                        Debug.Log("GameManager recieved Computer " + currentPlayer.getUserID() + " trump pick " + trumpChoice);
                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved Computer " + currentPlayer.getUserID() + " trump pick " + trumpChoice;
                        trumpCount++;
                        if(!trumpChoice.Equals("Pass")) {
                            trumpSuit = trumpChoice;
                            TrumpHolder.GetComponent<TMPro.TextMeshProUGUI>().text = trumpSuit;
                            while (!currentPlayer.getUserID().Equals(dealer.getUserID())) {
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                            }
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                            trumpCount = 5;
                        } 
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        //yield return new WaitForSeconds(2);
                    }
                } else {
                    printed = false;
                    currentState = 3;
                    trumpCount = 0;
                }
            } if(currentState == 3) { //have players play their cards 
                if(cardCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        //wait for user input
                        if(printed == false) {
                            currentPlayer.printHand();
                            DisplayOneHand(currentPlayer);
                            printed = true;
                            Debug.Log("Pick a card to play");
                            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Pick a card index to play (1 leftmost card)";
                        }
                        if(!currentInput.Equals("empty")) {
                            //Debug.Log("card count: " + cardCount);
                            Debug.Log("hand size:" + currentPlayer.getHandList().Count);
                            if(!(currentInput.Equals("1") || currentInput.Equals("2") || currentInput.Equals("3") || currentInput.Equals("4") || currentInput.Equals("5"))) {
                                //make sure users can only play cards they have
                                Debug.Log("not a valid index please enter an index of a card in your hand between 1-" + currentPlayer.getHandList().Count);
                                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "not a valid index please enter an index of a card in your hand between 1-" + currentPlayer.getHandList().Count;
                                currentInput = "empty";
                            } else {
                                if(cardCount != 0 && currentPlayer.hasLeadSuit(trickCards[0].GetComponent<Card>().suitValueString, trumpSuit)) { //check to see if we have followed suit
                                    int intInput = int.Parse(currentInput);
                                    if(intInput <= currentPlayer.getHandList().Count) {
                                        bool followedSuit = followSuitCheck(trickCards[0].GetComponent<Card>().suitValueString, currentPlayer.peekAtCard(intInput - 1), trumpSuit);
                                        if(followedSuit) { //if player did follow suit
                                            trickCards[cardCount] = currentPlayer.playCard(intInput - 1);
                                            flipCard(trickCards[cardCount]);
                                            DisplayOnePlay(trickCards[cardCount]);
                                            teamID[cardCount] = currentPlayer.getTeamNumber();
                                            Debug.Log("GameManager recieved user index play " + currentInput);
                                            Debug.Log("GameManager recieved User card play " + trickCards[cardCount].GetComponent<Card>().faceValue + " of " + trickCards[cardCount].GetComponent<Card>().suitValueString);
                                            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved User card play " + trickCards[cardCount].GetComponent<Card>().faceValue + " of " + trickCards[cardCount].GetComponent<Card>().suitValueString;
                                            cardCount++;
                                            currentInput = "empty";
                                            playerQueue.Enqueue(currentPlayer);
                                            currentPlayer = playerQueue.Dequeue();
                                        } else { //if player didn't follow suit
                                            Debug.Log("Not a valid input you must follow suit if you have a card with same suit as first card played");
                                            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Not a valid input you must follow suit if you have a card with same suit as first card played";
                                            currentInput = "empty";
                                        }
                                    } else {
                                        Debug.Log("Not a valid input index does not correspond to a card in your hand");
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Not a valid input index does not correspond to a card in your hand";
                                        currentInput = "empty";
                                    }
                                } else { //if following suit is not an issue
                                    int intInput = int.Parse(currentInput);
                                    if(intInput <= currentPlayer.getHandList().Count) {
                                        trickCards[cardCount] = currentPlayer.playCard(intInput - 1);
                                        flipCard(trickCards[cardCount]);
                                        DisplayOnePlay(trickCards[cardCount]);
                                        teamID[cardCount] = currentPlayer.getTeamNumber();
                                        Debug.Log("GameManager recieved user index play " + currentInput);
                                        Debug.Log("GameManager recieved User card play " + trickCards[cardCount].GetComponent<Card>().faceValue + " of " + trickCards[cardCount].GetComponent<Card>().suitValueString);
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved User card play " + trickCards[cardCount].GetComponent<Card>().faceValue + " of " + trickCards[cardCount].GetComponent<Card>().suitValueString;
                                        cardCount++;
                                        currentInput = "empty";
                                        playerQueue.Enqueue(currentPlayer);
                                        currentPlayer = playerQueue.Dequeue();
                                    } else {
                                        Debug.Log("Not a valid input index does not correspond to a card in your hand");
                                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Not a valid input index does not correspond to a card in your hand";
                                        currentInput = "empty";
                                    }
                                    //yield return new WaitForSeconds(2);
                                }
                            }
                        }
                    } else {
                        //Debug.Log("card count: " + cardCount);
                        //have computer choose what card to play
                        GameObject playedCard = currentPlayer.chooseCardToPlay(trickCards, trumpSuit, cardCount);
                        if(currentPlayer.getUserID().Equals("2")) {
                            DisplayTwoHand(playedCard);
                        } else if(currentPlayer.getUserID().Equals("3")) {
                            DisplayThreeHand(playedCard);
                        } else if(currentPlayer.getUserID().Equals("4")) {
                            DisplayFourHand(playedCard);
                        }
                        trickCards[cardCount] = playedCard;
                        teamID[cardCount] = currentPlayer.getTeamNumber();
                        Debug.Log("GameManager recieved Computer " + currentPlayer.getUserID() + " card play " + playedCard.GetComponent<Card>().faceValue + " of " + playedCard.GetComponent<Card>().suitValueString);
                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "GameManager recieved Computer " + currentPlayer.getUserID() + " card play " + playedCard.GetComponent<Card>().faceValue + " of " + playedCard.GetComponent<Card>().suitValueString;
                        cardCount++;
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        //yield return new WaitForSeconds(2);
                    }
                } else {
                    printed = false;
                    currentState = 4;
                }
            } if(currentState == 4) { //calculate winner of the trick
                //Debug.Log("Trick count: " + trickCount);
                if(trickCount < 5) {
                    int trickTeamWinner = getTrickWinner(trumpSuit, trickCards);
                    trickCount++;
                    cardCount = 0;
                    if(trickCount < 5) {
                        currentState = 3;
                    }
                    if(teamID[trickTeamWinner] == 1) {
                        oneTrickScore++;
                        Debug.Log("Team one wins the trick");
                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text ="Team one wins the trick";
                        for(int playerIndex = 0; playerIndex < trickTeamWinner; playerIndex++) {
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                        }
                    } else {
                        twoTrickScore++;
                        Debug.Log("Team two wins the trick");
                        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text ="Team two wins the trick";
                        for(int playerIndex = 0; playerIndex < trickTeamWinner; playerIndex++) {
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                        }
                    }
                    
                } else {
                    currentState = 5;
                }
                Debug.Log("Trick Score, team one: " + oneTrickScore + " team two: " + twoTrickScore);
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Trick Score, team one: " + oneTrickScore + " team two: " + twoTrickScore;

            } if(currentState == 5) { //calculate the winner of hand and scores
                trickCount = 0;
                if(oneTrickScore > twoTrickScore) {
                    Debug.Log("Team one wins the hand!");
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Team one wins the hand!";
                    if(oneTrickScore == 5 || trumpChosingTeam == 2) {
                        teamOneScore += 5; //is normally 2 
                    } else {
                        teamOneScore += 5; //is normally 1 
                    }
                } else {
                    Debug.Log("Team two wins the hand!");
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Team two wins the hand!";
                    if(twoTrickScore == 5 || trumpChosingTeam == 1) {
                        teamTwoScore += 5; //is normally 2 
                    } else {
                        teamTwoScore += 5; //is normally 1 
                    }
                }
                oneTrickScore = 0;
                twoTrickScore = 0;
                Debug.Log("current total score is team one: " + teamOneScore + ", Team two score: " + teamTwoScore);
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text ="current total score is team one: " + teamOneScore + ", Team two score: " + teamTwoScore;
                TrumpHolder.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                //reset queue to next dealer
                while (!currentPlayer.getUserID().Equals(dealer.getUserID())) {
                    playerQueue.Enqueue(currentPlayer);
                    currentPlayer = playerQueue.Dequeue();
                }
                playerQueue.Enqueue(currentPlayer);

                currentState = 0;
            }
        }
        //yield return null;

    }
    
    IEnumerator  sleepFunction() {
        sleeping = true;
        yield return new WaitForSeconds(3);
        sleeping = false;
        sleepRunning = false;
    }

    public bool checkForWinner() {
        if(teamOneScore >= 10) {
            string message = "Team 1 wins!";
            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Team 1 wins!";
            Debug.Log(message);
            return true;
        } else if(teamTwoScore >= 10) {
            string message = "Team 2 wins!";
            GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Team 2 wins!";
            Debug.Log(message);
            return true;
        }
        return false;
    } 

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("sleep value: " + sleeping);
        if(sleeping == false) {
            //Debug.Log("Game decision run");
            gameDecision();
        } 
        if(sleepRunning == false){
            sleepRunning = true;
            StartCoroutine(sleepFunction());
            //Debug.Log("sleep running set to true");
        }
    }

    
    public void OnEnter(string userInput) {
        Debug.Log("User entered " + userInput); 
        currentInput = userInput;
    }
    

    public void OnClick() {
        Debug.Log("Deal button clicked");
        /*
        GameObject card1Instance = Instantiate(handcard1, new Vector3(-30, 0, 0), Quaternion.identity);
        GameObject card2Instance = Instantiate(handcard2, new Vector3(-60, 0, 0), Quaternion.identity);
        GameObject card3Instance = Instantiate(handcard3, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject card4Instance = Instantiate(handcard4, new Vector3(30, 0, 0), Quaternion.identity);
        GameObject card5Instance = Instantiate(handcard5, new Vector3(60, 0, 0), Quaternion.identity);

        card1Instance.transform.SetParent(handarea.transform, false);
        card2Instance.transform.SetParent(handarea.transform, false);
        card3Instance.transform.SetParent(handarea.transform, false);
        card4Instance.transform.SetParent(handarea.transform, false);
        card5Instance.transform.SetParent(handarea.transform, false);
        */

        // card1Instance.transform.scale = new Vector3(0.5f, 0.5f, 0.5f);  
    }
}


