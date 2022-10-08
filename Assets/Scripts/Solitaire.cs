using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cardFaces;
    public static string[] suits = new string[] {"C", "D", "H", "S"};
    public static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    public GameObject SolitaireGame;
    public GameObject cardPrefab;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public List<string>[] playSpaces;
    public List<string>[] tops;

    private List<string> playSpace1 = new List<string>();
    private List<string> playSpace2 = new List<string>();
    private List<string> playSpace3 = new List<string>();
    private List<string> playSpace4 = new List<string>();
    private List<string> playSpace5 = new List<string>();
    private List<string> playSpace6 = new List<string>();
    private List<string> playSpace7 = new List<string>();

    public List<string> deck;

    // Start is called before the first frame update
    void Start()
    {
        playSpaces = new List<string>[] {playSpace1, playSpace2, playSpace3, playSpace4, playSpace5, playSpace6, playSpace7};
       PlayCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards() {
        deck = GenerateDeck();
        Shuffle(deck);
        SolitaireSort();
        StartCoroutine(SolitaireDeal());
    }

    public static List<string> GenerateDeck() {
        List<string> newDeck = new List<string>();
        foreach (string s in suits) {
            foreach (string v in values) {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    void Shuffle<T>(List<T> list) {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1) {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    IEnumerator SolitaireDeal() {
        for (int i = 0; i < 7; i++) {
            float yOffset = 0;
            float zOffset = 0.03f;
            foreach (string card in playSpaces[i]) {
                yield return new WaitForSeconds(0.01f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;
                if (card == playSpaces[i][playSpaces[i].Count - 1]) {
                    newCard.GetComponent<Selectable>().faceUp = true;
                }
                yOffset = yOffset + 0.3f;
                zOffset = zOffset + 0.03f;
            }
        }
    }
    void SolitaireSort() {
        for (int i = 0; i < 7; i++) {
            for (int j = i; j < 7; j++) {
                playSpaces[j].Add(deck.Last<string>());
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }
}
