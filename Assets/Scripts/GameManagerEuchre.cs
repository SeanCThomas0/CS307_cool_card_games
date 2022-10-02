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

    public class CardPlayer {
        private ArrayList hand;
        private string userID;

        public CardPlayer(string userID) {
            hand = new ArrayList();
            this.userID = userID;
        }

        public ArrayList getHandList() {
            return hand;
        }
        
        public string getUserID() {
            return userID;
        }

        public Card playCard(int indexInHand) {
            Card returnCard = (Card) hand[indexInHand - 1];
            hand.RemoveAt(indexInHand - 1);
            return returnCard;
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Euchre Game Starting");
        ArrayList cardDeck = new ArrayList();
        ArrayList playerQueue = new ArrayList();

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
        CardPlayer[] teamOne = new CardPlayer[2];
        CardPlayer[] teamTwo = new CardPlayer[2];
        int oneCount = 0;
        int twoCount = 0;

        for(int i = 1; i <= 4; i++) {
            playerQueue.Add(new CardPlayer("" + i));
            if(i % 2 == 1) {
                teamOne[oneCount] = (CardPlayer) playerQueue[i-1];
                oneCount++;
            }
            if(i % 2 == 0) {
                teamTwo[twoCount] = (CardPlayer) playerQueue[i-1];
                twoCount++;
            }
        }
        int teamOneScore = 0;
        int teamTwoScore = 0;

        while (teamOneScore <= 10 && teamTwoScore <= 10) {
            teamOneScore++;
        }
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


