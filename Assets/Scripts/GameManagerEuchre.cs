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

        public void removeFromHand(int indexInHand) {
            hand.RemoveAt(indexInHand - 1);
        }

        /*gets card value and removes it from the User's hand*/
        public Card playCard(int indexInHand) {
            Card returnCard = (Card) hand[indexInHand - 1];
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
    public Card[] trickCards; //array for cards played in a given trick
    public Card topCard = null;
    
    

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
                            if(!((currentInput.Equals("pick") || currentInput.Equals("pick up") || currentInput.Equals("pass")))) {
                                Debug.Log("Enter a valid input (pick, pick up, or pass)");
                                currentInput = "empty";
                            } else {
                                pickCount++;
                                Debug.Log("current move " + currentPlayer.getUserID());
                                Debug.Log("GameManager recieved pick/pass: " + currentInput);
                                Debug.Log("turn numb: " + pickCount);
                                currentInput = "empty";
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                                //yield return new WaitForSeconds(2);
                            }
                        }
                    } else {
                        //have computer choose whether to pick up or pass
                        string computerChoice = currentPlayer.pickUpCard(topCard, dealer);
                        Debug.Log("GameManager recieved Computer top card: " + computerChoice);
                        pickCount++;
                        Debug.Log("current move " + currentPlayer.getUserID());
                        Debug.Log("turn numb: " + pickCount);
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        if(computerChoice.Equals("Pick")) {
                            while (!currentPlayer.getUserID().Equals(dealer.getUserID())) {
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                            }
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                            pickCount = 4;
                        }
                        //yield return new WaitForSeconds(2);
                    }
                } else {
                    currentState = 2;
                    pickCount = 0;
                }
            } if(currentState == 2) { //have players choose trump (dealer has to if no one else will) 
                if(trumpCount < 4) {
                    if(currentPlayer.getIsHuman() == true) {
                        //wait for user input
                        if(!currentInput.Equals("empty")) {
                            trumpCount++;
                            Debug.Log("GameManager recieved trump pick" + currentInput);
                            currentInput = "empty";
                            playerQueue.Enqueue(currentPlayer);
                            currentPlayer = playerQueue.Dequeue();
                            //yield return new WaitForSeconds(2);
                        }
                    } else {
                        //have computer choose trump suit or pass
                        Debug.Log("GameManager recieved Computer trump pick [suit]");
                        trumpCount++;
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
                            if(((currentInput.Equals("pick") || currentInput.Equals("pick up") || currentInput.Equals("pass")))) {
                                //make sure users can only play cards they have
                            } else {
                                cardCount++;
                                Debug.Log("GameManager recieved user card play " + currentInput);
                                currentInput = "empty";
                                playerQueue.Enqueue(currentPlayer);
                                currentPlayer = playerQueue.Dequeue();
                                //yield return new WaitForSeconds(2);
                            }
                        }
                    } else {
                        //have computer choose what card to play
                        Debug.Log("GameManager recieved Computer card play [computer card]");
                        cardCount++;
                        playerQueue.Enqueue(currentPlayer);
                        currentPlayer = playerQueue.Dequeue();
                        //yield return new WaitForSeconds(2);
                    }
                } else {
                    currentState = 4;
                }
            } if(currentState == 4) { //calculate winner of the trick
                if(trickCount < 5) {
                    trickCount++;
                    cardCount = 0;
                    oneTrickScore++;
                    currentState = 3;
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

    void gameLoop() {
        /*begin game flow of dealing and playing cards */
        while (teamOneScore < 10 && teamTwoScore < 10) {
            cardDeck = shuffle(cardDeck);
            currentInput = "empty";

            dealer = playerQueue.Dequeue();
            playerQueue.Enqueue(dealer);

            /* deal cards to each player */
            currentPlayer = null;
            int deckCount = 0;
            for(int dealNumb = 0; dealNumb < 4; dealNumb++) {
                currentPlayer = playerQueue.Dequeue();
                for(int handCount = 0; handCount < 5; handCount++) {
                    currentPlayer.addToHand(cardDeck[deckCount]);
                    deckCount++;
                }
                playerQueue.Enqueue(currentPlayer);
            }
            
            //test to make sure each player gets 5 unique cards
            for(int dealNumb = 0; dealNumb < 4; dealNumb++) {
                currentPlayer = playerQueue.Dequeue();
                List<Card> tempHand = currentPlayer.getHandList();
                foreach(Card currCard in tempHand) {
                    StringBuilder testsb = new StringBuilder("", 100);
                    testsb.AppendFormat("{0} has {1} of {2}", currentPlayer.getUserID(), currCard.getFaceValue(), currCard.getSuit());
                    string message = testsb.ToString();
                    Debug.Log(message);
                }
                playerQueue.Enqueue(currentPlayer);
            }
            //ignore above section and delete later

            /* have users choose to pick up card that has been flipped on top of remaining cards*/

            /* have users choose the trump suit if top card was not picked up*/
            
            /* Play 5 tricks to determine who wins the hand*/ 
            //string trumpSuit = "";
            int teamOneTrickScore = 0;
            int teamTwoTrickScore = 0;

            for(int trickNumber = 0; trickNumber < 5; trickNumber++) {
                //string leadSuit = "";
                /* have players each play their cards */
                
                /* calculate winner of the given trick */
                teamTwoTrickScore++;
                
                /*rotate queue so that winner plays the first card at the beginning of the next trick */
            }

            /* calculate winner of the hand and points based off who called trump */
            if(teamOneTrickScore > teamTwoScore) {
                teamOneScore+=3;
            } else {
                teamTwoScore+=3;
            }
        }

        /* Check to see if */
        if(teamOneScore >= 10) {
            string message = "Team 1 wins!";
            Debug.Log(message);
        } else if(teamTwoScore >= 10) {
            string message = "Team 2 wins!";
            Debug.Log(message);
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


