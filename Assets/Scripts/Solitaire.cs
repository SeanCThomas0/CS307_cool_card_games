using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Solitaire : MonoBehaviour
{
    private CardDealer cardDealer;
    public GameObject cardDealerController;
    public GameObject deckButton;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public List<GameObject>[] playSpaces;
    public List<GameObject>[] tops;
    public List<GameObject> tripsOnDisplay = new List<GameObject>();
    public List<List<GameObject>> deckTrips = new List<List<GameObject>>();

    private List<GameObject> playSpace1 = new List<GameObject>();
    private List<GameObject> playSpace2 = new List<GameObject>();
    private List<GameObject> playSpace3 = new List<GameObject>();
    private List<GameObject> playSpace4 = new List<GameObject>();
    private List<GameObject> playSpace5 = new List<GameObject>();
    private List<GameObject> playSpace6 = new List<GameObject>();
    private List<GameObject> playSpace7 = new List<GameObject>();

    public List<GameObject> deck;
    public List<GameObject> discardPile = new List<GameObject>();
    private int deckLocation;
    private int trips;
    private int tripsRemainder;

    // Start is called before the first frame update
    void Start()
    {
        cardDealer = cardDealerController.GetComponent<CardDealer>();
        playSpaces = new List<GameObject>[] {playSpace1, playSpace2, playSpace3, playSpace4, playSpace5, playSpace6, playSpace7};
       PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards() {
        foreach (List<GameObject> list in playSpaces) {
            list.Clear();
        }
        deck = cardDealer.RandomCards(52);
        SolitaireSort();
        StartCoroutine(SolitaireDeal());
        SortDeckIntoTrips();
    }

    IEnumerator SolitaireDeal() {
        for (int i = 0; i < 7; i++) {
            float yOffset = 0;
            float zOffset = 0.03f;
            foreach (GameObject card in playSpaces[i]) {
                yield return new WaitForSeconds(0.01f);
                card.transform.SetParent(bottomPos[i].transform, false);
                card.transform.position = new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset);
                
                // card.GetComponent<Selectable>().row = i;
                Debug.Log("got here");
                card.SetActive(true);
                if (card != playSpaces[i][playSpaces[i].Count - 1]) {
                    cardDealer.ShowBackKeepValue(card, Card.backColor.BLUE, Card.backDesign.OUTLINE_SIMPLE_PATTERN);
                } else {
                    cardDealer.SetSprite(card);
                }
                yOffset = yOffset + 0.3f;
                zOffset = zOffset + 0.03f;
                discardPile.Add(card);
            }
        }
        foreach (GameObject card in discardPile) {
            if (deck.Contains(card)) {
                deck.Remove(card);
            }
        }
        discardPile.Clear();
    }

    void SolitaireSort() {
        for (int i = 0; i < 7; i++) {
            for (int j = i; j < 7; j++) {
                Debug.Log(deck.Last<GameObject>().GetComponent<Card>().numValue);
                playSpaces[j].Add(deck.Last<GameObject>());
                Debug.Log(playSpaces[j][i].GetComponent<Card>().numValue);
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    public void SortDeckIntoTrips() {
        trips = deck.Count / 3;
        tripsRemainder = deck.Count % 3;
        deckTrips.Clear();

        int modifier = 0;
        for (int i = 0; i < trips; i++) {
            List<GameObject> myTrips = new List<GameObject>();
            for (int j = 0; j < 3; j++) {
                myTrips.Add(deck[j + modifier]);
            }
            deckTrips.Add(myTrips);
            modifier += 3;
        }
        if (tripsRemainder != 0) {
            List<GameObject> myRemainders = new List<GameObject>();
            modifier = 0;
            for (int k = 0; k < tripsRemainder; k++) {
                myRemainders.Add(deck[deck.Count - tripsRemainder + modifier]);
                modifier++;
            }
            deckTrips.Add(myRemainders);
            trips++;
        }
        deckLocation = 0;
    }

    public void DealFromDeck() {

        //add remaining cards to discard pile

        foreach (Transform child in deckButton.transform) {
            if (child.CompareTag("Card")) {
                deck.Remove(child.GetComponent<GameObject>());
                discardPile.Add(child.GetComponent<GameObject>());
                Destroy(child.gameObject);
            }
        }

        if (deckLocation < trips) {
            tripsOnDisplay.Clear();
            float xOffset = -2.5f;
            float zOffset = -0.2f;
            float x = 10.0f;
            float y = 3.14342f;
            float z = -9.008199f;
            foreach (GameObject card in deckTrips[deckLocation]) {
                //card.transform.SetParent(deckButton.transform, false);
                card.transform.position = new Vector3(x + xOffset, y, z + zOffset);
                card.SetActive(true);
                xOffset -= 0.5f;
                zOffset -= 0.2f;
                tripsOnDisplay.Add(card);
                cardDealer.SetSprite(card);
                card.GetComponent<Card>().inPool = true;
            }
            deckLocation++;
        } else {
            //restack the top deck
            RestackTopDeck();
        }
    }

    void RestackTopDeck() {
        deck.Clear();
        foreach (GameObject card in discardPile) {
            deck.Add(card);
        }
        discardPile.Clear();
        SortDeckIntoTrips();
    }
}
