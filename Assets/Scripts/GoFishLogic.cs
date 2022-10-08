using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFishLogic : MonoBehaviour
{
    public GameObject Controller;
    CardDealer cardDealer;

    List<Card> pool;

    // Start is called before the first frame update
    void Start()
    {
        cardDealer = Controller.GetComponent<CardDealer>();

       pool = cardDealer.RandomCards(52);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Player {
        private string userID;
        private List<Card> hand;

        public Player(string userID) {
            this.userID = userID;
        }

        public string getUserID() {
            return userID;
        }

        public void addToHand(Card card) {

        }

        public void removeFromHand(Card card) {

        }
    }

    public void addToHand_removeFromPool(Player player) {

    }
}
