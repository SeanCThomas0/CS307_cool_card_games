using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEuchre : MonoBehaviour
{
    public GameObject handcard1;
    public GameObject handcard2;
    public GameObject handcard3;
    public GameObject handcard4;
    public GameObject handcard5;
    public GameObject handarea;
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

        public CardPlayer(string userID, int teamNumber) {
            hand = new List<Card>();
            this.userID = userID;
            this.teamNumber = teamNumber;
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
    
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Euchre Game Starting");
        List<Card> cardDeck = new List<Card>();
        Queue<CardPlayer> playerQueue = new Queue<CardPlayer>();
        
        CardPlayer playerOne;
        CardPlayer playerTwo;
        CardPlayer playerThree;
        CardPlayer playerFour;

        /*create cards needed for game*/
        for(int i = 9; i <= 14; i++) {
            for(int j = 1; j <= 4; j++) {
                cardDeck.Add(new Card(i, j));
            }
        }
        foreach(Card addedCard in cardDeck) {
            string tempFace = addedCard.getFaceValue();
            string tempSuit = addedCard.getSuit();
            string message = "Added card: " + tempFace + " of " + tempSuit + "\n" ; 
            Debug.Log(message);
        }
        
        int oneCount = 0;
        int twoCount = 0;

        /*create card player objects with userID's*/
        for(int i = 1; i <= 4; i++) {
            if(i % 2 == 1) {
                if(i == 1) {
                    playerOne = new CardPlayer("" + i, 1);
                    playerQueue.Enqueue(playerOne);
                } else {
                    playerThree = new CardPlayer("" + i, 1);
                    playerQueue.Enqueue(playerThree);
                }
                oneCount++;
            } else if(i % 2 == 0) {
                if(i == 2) {
                    playerTwo = new CardPlayer("" + i, 2);
                    playerQueue.Enqueue(playerTwo);
                } else {
                    playerFour = new CardPlayer("" + i, 2);
                    playerQueue.Enqueue(playerFour);
                }
                twoCount++;
            }
        }
        int teamOneScore = 0;
        int teamTwoScore = 0;
        CardPlayer dealer = null;

        /*begin game flow of dealing and playing cards */
        while (teamOneScore < 10 && teamTwoScore < 10) {
            dealer = playerQueue.Dequeue();
            playerQueue.Enqueue(dealer);

            /* deal cards to each player */
            CardPlayer currentPlayer = null;
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
                teamOneScore+=10;
            } else {
                teamTwoScore+=10;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        Debug.Log("Deal button clicked");
        GameObject card1Instance = Instantiate(handcard1, new Vector3(1, 0, 0), Quaternion.identity);
        GameObject card2Instance = Instantiate(handcard2, new Vector3(-1, 0, 0), Quaternion.identity);
        GameObject card3Instance = Instantiate(handcard3, new Vector3(1, 0, 0), Quaternion.identity);
        GameObject card4Instance = Instantiate(handcard4, new Vector3(-1, 0, 0), Quaternion.identity);
        GameObject card5Instance = Instantiate(handcard5, new Vector3(1, 0, 0), Quaternion.identity);

        card1Instance.transform.SetParent(handarea.transform, false);
        card2Instance.transform.SetParent(handarea.transform, false);
        card3Instance.transform.SetParent(handarea.transform, false);
        card4Instance.transform.SetParent(handarea.transform, false);
        card5Instance.transform.SetParent(handarea.transform, false);

        // card1Instance.transform.scale = new Vector3(0.5f, 0.5f, 0.5f);  
    }
}


