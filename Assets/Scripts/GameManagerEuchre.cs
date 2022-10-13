using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEuchre : MonoBehaviour
{
    /*
    public GameObject handcard1;
    public GameObject handcard2;
    public GameObject handcard3;
    public GameObject handcard4;
    public GameObject handcard5;
    public GameObject handarea;
    */
    public GameObject UserPlayer;
    public GameObject GameManager;

    /*Temporary card class for testing to be replaced with CardDealer script*/
    public class Card {
        private int numbValue;
        private string faceValue;
        private string suit;

        public Card(int numbValue, int suit) {
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

        public int getNumbValue() {
            return numbValue;
        }

        public string getFaceValue() {
            return faceValue;
        }
        
        public string getSuit() {
            return suit;
        }

    }

    /*CardPlayer class for individual game players*/
    public class CardPlayer {
        private List<Card> hand;
        private string userID;
        private int teamNumber;
        private bool isHuman;

        public CardPlayer(string userID, int teamNumber, bool isHuman) {
            hand = new List<Card>();
            this.userID = userID;
            this.teamNumber = teamNumber;
            this.isHuman = isHuman;
        }

        public List<Card> getHandList() {
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
            foreach (Card currentCard in hand) {
                Debug.Log(count + ": " + currentCard.getFaceValue() + " of " + currentCard.getSuit());
                count++;
            }
        }

        public void addToHand(Card cardToAdd) {
            hand.Add(cardToAdd);
        }

        public Card removeFromHand(int indexInHand) {
            Card returnCard = (Card) hand[indexInHand];
            hand.RemoveAt(indexInHand);
            return returnCard;
        }

        private bool getJackPair(string trumpSuit, Card possibleJack) {
            if (trumpSuit.Equals("Clubs") && possibleJack.getSuit().Equals("Spades")) {
                return true;
            } else if (trumpSuit.Equals("Spades") && possibleJack.getSuit().Equals("Clubs")) {
                return true;
            } else if (trumpSuit.Equals("Hearts") && possibleJack.getSuit().Equals("Diamonds")) {
                return true;
            } else if (trumpSuit.Equals("Diamonds") && possibleJack.getSuit().Equals("Hearts")) {
                return true;
            } else {
                return false;
            }
        }
        
        public bool hasLeadSuit(string leadSuit, string trumpSuit) {
            bool leadSuitFound = false;
            for (int i = 0; i < hand.Count; i++) {
                Card currentCard = (Card) hand[i];
                if (currentCard.getSuit().Equals(leadSuit)) {
                    leadSuitFound = true;
                } else if (leadSuit.Equals(trumpSuit) && getJackPair(trumpSuit, currentCard)) {
                    leadSuitFound = true;
                }
            }
            return leadSuitFound;
        }

        /*gets card value and removes it from the User's hand*/
        public Card playCard(int indexInHand) {
            Card returnCard = (Card) hand[indexInHand];
            removeFromHand(indexInHand);
            return returnCard;
        }

        private void adjustDecision(int[] suits, int[] totalCardValue, int[] avgCardValue, Card currCard, int index) {
            suits[index]++;
            if (currCard.getNumbValue() == 11) {
                totalCardValue[index] += 15;
            } else {
                totalCardValue[index] += currCard.getNumbValue();
            }
            avgCardValue[index] = totalCardValue[index]/suits[index];
        }

        public string pickUpCard(Card topCard, CardPlayer dealer) {
            string topSuit = topCard.getSuit();
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
                Card currCard = (Card) hand[i];
                if (currCard.getSuit().Equals("Clubs")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 0);
                } else if (currCard.getSuit().Equals("Diamonds")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 1);
                } else if (currCard.getSuit().Equals("Hearts")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 2);
                } else if (currCard.getSuit().Equals("Spades")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 3);
                }
            }
            int trumpIndex = 0;
            if (topCard.getSuit().Equals("Clubs")) {
                decisionScore = avgCardValue[0] + 1;
                trumpIndex = 0;
            } else if (topCard.getSuit().Equals("Diamonds")) {
                decisionScore = avgCardValue[1] + 1;
                trumpIndex = 1;
            } else if (topCard.getSuit().Equals("Hearts")) {
                decisionScore = avgCardValue[2] + 1;
                trumpIndex = 2;
            } else if (topCard.getSuit().Equals("Spades")) {
                decisionScore = avgCardValue[3] + 1;
                trumpIndex = 3;
            }
            suits[trumpIndex]++;

            if (topCard.getNumbValue() == 11 || topCard.getNumbValue() == 13 || topCard.getNumbValue() == 14) {
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

        private String getSuit(int place) {
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
        public void pickUpCard (Card topCard, string trumpSuit) {
            int[] handScores = new int[5];
            int count = 0;
            int currentWorst = 0;
            foreach(Card currCard in hand) {
                if (currCard.getSuit().Equals(trumpSuit)) {
                    handScores[count] += 15 + currCard.getNumbValue();
                } else {
                    handScores[count] += currCard.getNumbValue();
                }
                if (handScores[count] < handScores[currentWorst]) {
                    currentWorst = count;
                }
                count++;
            }
            removeFromHand(currentWorst);
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
            foreach(Card currCard in hand) {
                if (currCard.getSuit().Equals("Clubs")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 0);
                } else if (currCard.getSuit().Equals("Diamonds")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 1);
                } else if (currCard.getSuit().Equals("Hearts")) {
                    adjustDecision(suits, totalCardValue, avgCardValue, currCard, 2);
                } else if (currCard.getSuit().Equals("Spades")) {
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

            wouldBeSuit = getSuit(trumpIndex);
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
        public Card chooseCardToPlay(Card[] cardsPlayed, string trumpSuit, int numbPlayed) {
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
                foreach(Card currCard in hand) {
                    if (currCard.getSuit().Equals(trumpSuit)) {
                        handScores[count] += currCard.getNumbValue();
                    } else {
                        handScores[count] += 4 + currCard.getNumbValue();
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
                Card leadCard = cardsPlayed[0];

                if (leadCard.getFaceValue().Equals("Jack")) {
                    if (getJackPair(trumpSuit, leadCard)) {
                        leadSuit = trumpSuit;
                    }
                } else {
                    leadSuit = cardsPlayed[0].getSuit();
                }
                bool leadSuitFound = false;

                foreach(Card playedCard in cardsPlayed) {
                    if (playedCard == null) {
                        break;
                    }
                    if (playedCard.getSuit().Equals(trumpSuit)) {
                        playedCardsValue[count] += 15 + playedCard.getNumbValue();
                    } else {
                        playedCardsValue[count] += playedCard.getNumbValue();
                    }
                    if (playedCardsValue[count] > playedCardsValue[topCard]) {
                        topCard = count;
                    }
                    count++;
                }

                count = 0;
                foreach(Card currCard in hand) {
                    if (currCard.getSuit().Equals(leadSuit)) {
                        handScores[count] += 100;
                        leadSuitFound = true;
                    }
                    if (currCard.getSuit().Equals(trumpSuit)) {
                        handScores[count] += 15 + currCard.getNumbValue();
                    } else {
                        handScores[count] += currCard.getNumbValue();
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
    
    private static bool getJackPair(string trumpSuit, Card possibleJack) {
        if (trumpSuit.Equals("Clubs") && possibleJack.getSuit().Equals("Spades")) {
            return true;
        } else if (trumpSuit.Equals("Spades") && possibleJack.getSuit().Equals("Clubs")) {
            return true;
        } else if (trumpSuit.Equals("Hearts") && possibleJack.getSuit().Equals("Diamonds")) {
            return true;
        } else if (trumpSuit.Equals("Diamonds") && possibleJack.getSuit().Equals("Hearts")) {
            return true;
        } else {
            return false;
        }
    }

    public bool followSuitCheck (string leadSuit, Card cardPlayed, string trumpSuit) {
        if (getCardSuit(cardPlayed, trumpSuit).Equals(leadSuit)) {
            return true;
        }
        return false;
    }

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
    public Card[] trickCards = new Card[4]; //array for cards played in a given trick
    public Card topCard = null;
    public int[] teamID = new int[4];
    
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Euchre Game Starting");

        /*create cards needed for game*/
        for(int i = 9; i <= 14; i++) {
            for(int j = 1; j <= 4; j++) {
                cardDeck.Add(new Card(i, j));
            }
        }
        cardDeck = shuffle(cardDeck);
        foreach(Card addedCard in cardDeck) {
            string tempFace = addedCard.getFaceValue();
            string tempSuit = addedCard.getSuit();
            string message = "Added card: " + tempFace + " of " + tempSuit + "\n" ; 
            Debug.Log(message);
        }
        Debug.Log(cardDeck.Count);
        
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
        gameDecision();
    }

    /* functions to calculate winner of a given trick refactored from a previous personal project authored by Isaac Stephan*/
    private bool getJackValue(string trumpSuit, Card jack) {
        if (trumpSuit.Equals("clubs") && jack.getSuit().Equals("spades")) {
            return true;
        } else if (trumpSuit.Equals("spades") && jack.getSuit().Equals("clubs")) {
            return true;
        } else if (trumpSuit.Equals("hearts") && jack.getSuit().Equals("diamonds")) {
            return true;
        } else if (trumpSuit.Equals("diamonds") && jack.getSuit().Equals("hearts")) {
            return true;
        } else {
            return false;
        }
    }

    private string getCardSuit(Card firstCard, string trumpSuit) {
        if (firstCard.getFaceValue().Equals("jack") && getJackValue(trumpSuit, firstCard)) {
            return trumpSuit;
        } else {
            return firstCard.getSuit();
        }
    }

    private int getTrickWinner (string trumpSuit, Card[] playedCards) {
        int maxScore = 0;
        int maxIndex = 0;
        string leadSuit = getCardSuit(playedCards[0], trumpSuit);

        for (int cardIndex = 0; cardIndex < 4; cardIndex++) {
            int currentScore = 0;
            if (playedCards[cardIndex].getSuit().Equals(leadSuit)) {
                currentScore += 16;
            }
            if (playedCards[cardIndex].getSuit().Equals(trumpSuit)) {
                currentScore += 32;
                if (playedCards[cardIndex].getFaceValue().Equals("jack")) {
                    currentScore += 16;
                } else {
                    currentScore += playedCards[cardIndex].getNumbValue();
                }
            } else if (playedCards[cardIndex].getFaceValue().Equals("jack") && getJackValue(trumpSuit, playedCards[cardIndex])) {
                currentScore += 47; 
            } else {
                currentScore += playedCards[cardIndex].getNumbValue();
            }
            if (currentScore >maxScore) {
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
                cardDeck = shuffle(cardDeck);
                currentInput = "empty";

                dealer = playerQueue.Dequeue();
                playerQueue.Enqueue(dealer);
                //yield return new WaitForSeconds(2);
                /* deal cards to each player */
                int deckCount = 0;
                for(int dealNumb = 0; dealNumb < 4; dealNumb++) {
                    currentPlayer = playerQueue.Dequeue();
                    for(int handCount = 0; handCount < 5; handCount++) {
                        currentPlayer.addToHand(cardDeck[deckCount]);
                        deckCount++;
                    }
                    currentPlayer.printHand();
                    playerQueue.Enqueue(currentPlayer);
                }
                topCard = cardDeck[deckCount];
                currentPlayer = playerQueue.Dequeue();
                currentState = 1;

            } if(currentState == 1) { //have players choose to pick up top card 
                if(pickCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        //wait for user input
                        if(!currentInput.Equals("empty")) {
                            Debug.Log("pickCount: " + pickCount);
                            if(!(currentInput.Equals("pick") || currentInput.Equals("pick up") || currentInput.Equals("pass"))) {
                                Debug.Log("Enter a valid input (pick, pick up, or pass)");
                                currentInput = "empty";
                            } else if(currentInput.Equals("pick") || currentInput.Equals("pick up")) {
                                pickCount++;
                                trumpSuit = topCard.getSuit();
                                dealer.pickUpCard(topCard, dealer);
                                Debug.Log("current move " + currentPlayer.getUserID());
                                Debug.Log("GameManager recieved pick/pass: " + currentInput);
                                Debug.Log("turn numb: " + pickCount);
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
                            }
                        }
                    } else {
                        Debug.Log("pickCount: " + pickCount);
                        //have computer choose whether to pick up or pass
                        string computerChoice = currentPlayer.pickUpCard(topCard, dealer);
                        Debug.Log("GameManager recieved Computer top card: " + computerChoice);
                        pickCount++;
                        Debug.Log("current move " + currentPlayer.getUserID());
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        if(computerChoice.Equals("Pick")) {
                            Debug.Log("pick ran" );
                            trumpSuit = topCard.getSuit();
                            dealer.pickUpCard(topCard, dealer);
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
                    if(pickCount == 20) {
                        currentState = 3;
                        pickCount = 0;
                    } else {
                        currentState = 2;
                        pickCount = 0;
                    }
                }
            } if(currentState == 2) { //have players choose trump (dealer has to if no one else will) 
                if(trumpCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        //wait for user input
                        if(!currentInput.Equals("empty")) {
                            Debug.Log("trump count: " + trumpCount);
                            if(!((currentInput.Equals("Hearts") || currentInput.Equals("Spades") || currentInput.Equals("Clubs") || currentInput.Equals("Diamonds") || currentInput.Equals("pass")))) {
                                Debug.Log("Enter a valid input (Hearts, Spades, Clubs, Diamonds, or pass)");
                                currentInput = "empty";
                            } else if(trumpCount == 3 && currentInput.Equals("pass")) {
                                Debug.Log("cannot pass when you are the dealer, choose a valid suit");
                                currentInput = "empty";
                            } else {
                                trumpCount++;
                                trumpSuit = currentInput;
                                Debug.Log("GameManager recieved trump pick " + currentInput);
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
                        Debug.Log("trump count: " + trumpCount);
                        //have computer choose trump suit or pass
                        string trumpChoice = currentPlayer.suitOrPass(dealer);
                        Debug.Log("GameManager recieved Computer trump pick " + trumpChoice);
                        trumpCount++;
                        if(!trumpChoice.Equals("Pass")) {
                            trumpSuit = trumpChoice;
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
                    currentState = 3;
                    trumpCount = 0;
                }
            } if(currentState == 3) { //have players play their cards 
                if(cardCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        //wait for user input
                        if(!currentInput.Equals("empty")) {
                            Debug.Log("card count: " + cardCount);
                            Debug.Log("hand size:" + currentPlayer.getHandList().Count);
                            if(!(currentInput.Equals("1") || currentInput.Equals("2") || currentInput.Equals("3") || currentInput.Equals("4") || currentInput.Equals("5"))) {
                                //make sure users can only play cards they have
                                Debug.Log("not a valid index please enter an index of a card in your hand between 1-5");
                                currentInput = "empty";
                            } else {
                                int intInput = int.Parse(currentInput);
                                if(intInput <= currentPlayer.getHandList().Count) {
                                    trickCards[cardCount] = currentPlayer.playCard(intInput - 1);
                                    teamID[cardCount] = currentPlayer.getTeamNumber();
                                    Debug.Log("GameManager recieved user index play " + currentInput);
                                    Debug.Log("GameManager recieved User card play " + trickCards[cardCount].getFaceValue() + " of " + trickCards[cardCount].getSuit());
                                    cardCount++;
                                    currentInput = "empty";
                                    playerQueue.Enqueue(currentPlayer);
                                    currentPlayer = playerQueue.Dequeue();
                                } else {
                                    Debug.Log("Not a valid input index does not correspond to a card in your hand");
                                    currentInput = "empty";
                                }
                                //yield return new WaitForSeconds(2);
                            }
                        }
                    } else {
                        Debug.Log("card count: " + cardCount);
                        //have computer choose what card to play
                        Card playedCard = currentPlayer.chooseCardToPlay(trickCards, trumpSuit, cardCount);
                        trickCards[cardCount] = playedCard;
                        teamID[cardCount] = currentPlayer.getTeamNumber();
                        Debug.Log("GameManager recieved Computer card play " + playedCard.getFaceValue() + " of " + playedCard.getSuit());
                        cardCount++;
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        //yield return new WaitForSeconds(2);
                    }
                } else {
                    currentState = 4;
                }
            } if(currentState == 4) { //calculate winner of the trick
                Debug.Log("Trick count: " + trickCount);
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
                        for(int playerIndex = 0; playerIndex < trickTeamWinner; playerIndex++) {
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                        }
                    } else {
                        twoTrickScore++;
                        Debug.Log("Team two wins the trick");
                        for(int playerIndex = 0; playerIndex < trickTeamWinner; playerIndex++) {
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                        }
                    }
                } else {
                    currentState = 5;
                }
            } if(currentState == 5) { //calculate the winner of hand and scores
                playerQueue.Enqueue(currentPlayer);
                trickCount = 0;
                if(oneTrickScore > twoTrickScore) {
                    if(oneTrickScore == 5 || trumpChosingTeam == 2) {
                        teamOneScore += 2;
                    } else {
                        teamTwoScore += 1;
                    }
                } else {
                    if(twoTrickScore == 5 || trumpChosingTeam == 1) {
                        teamTwoScore += 2;
                    } else {
                        teamTwoScore += 1;
                    }
                }
                //reset queue to next dealer
                dealer = playerQueue.Dequeue();
                playerQueue.Enqueue(dealer);

                currentState = 0;
            }
        }


    }

    public bool checkForWinner() {
        if(teamOneScore >= 10) {
            string message = "Team 1 wins!";
            Debug.Log(message);
            return true;
        } else if(teamTwoScore >= 10) {
            string message = "Team 2 wins!";
            Debug.Log(message);
            return true;
        }
        return false;
    } 

    // Update is called once per frame
    void Update()
    {
        gameDecision();
    }

    
    public void OnEnter(string userInput) {
        Debug.Log("User entered " + userInput); 
        currentInput = userInput;
        //gameObject.SendMessage("recieve", userInput);
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


