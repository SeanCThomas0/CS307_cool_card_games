using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    public string[] suits = new string[] {"C", "D", "H", "S"};
    public string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    public GameObject SolitaireGame;
    public GameObject cardPrefab;

    public List<string> deck;

    // Start is called before the first frame update
    void Start()
    {
        // SolitaireDeal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCards() {
        deck = GenerateDeck();
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
            T temp list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    void SolitaireDeal() {
        float yOffset = 0;
        float zOffset = 0.03f;
        
        yOffset = yOffset + 0.1f;
        zOffset = zOffset + 0.03f;
    }
}
