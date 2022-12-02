using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class InstructionsScript : MonoBehaviour
{
    public GameObject EuchreButton;
    public GameObject SolitareButton;
    public GameObject GoFishButton;
    public GameObject PokerButton;
    public GameObject BruhButton;
    public GameObject ExitButton;
    public GameObject TextBox1;
    public GameObject TextBox2;

    private UserPreferences.backgroundColor backgroundColor;
    public GameObject mainCam;

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



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Rules Page Opened");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EuchreRules()
    {
        //display Euchre Rules in textBox
        Debug.Log("Euchre Button Pressed");
        string text1 = "Euchre Rules:\nGameflow\n\nEuchre is a game played by 4 players, with two players each on opposing teams. The game only uses cards with default values from lowest to highest: 9, 10, Jack, Queen, King, Ace (value hierarchy explained in card values section). The first team to reach a score of 10 wins the game, and a player increases their score by winning a trick (which will be explained later).\nPlay begins when a dealer is chosen from one of the 4 players, cards are dealt to each player, and the remaining 4 cards (kitty) are placed in the middle of the table with the card on the top being flipped over. The players take turns, starting with the player to the left of the dealer, choosing whether to have the dealer pick up the top card that is displayed and replace it with a card in their hand, or pass the decision off to the next player in line. If a player decides to have the dealer pick up the card on the top of the kitty, the trump suit is set to that top card, the dealer picks it up and discards a card in their hand, and card play begins. If all players decide to pass and not have the dealer pick up the top card, then action begins again  to the left of the dealer with players deciding to either choose a trump suit or pass. If all players pass, the dealer must choose a suit.\nOnce a trump suit is chosen or top card is picked up players begin to play five tricks, with the winner of the hand gaining a point. Players take turns choosing 1 card to play, and the highest card out of the 4 cards played in a given trick wins that trick.\nThe player to the left of the dealer starts on the first trick, and after that the winner of the previous trick starts play. If the lead player plays a card, and you have a card with the same suit in your hand, you must play a card that matches that lead suit, otherwise if you don’t have to follow suit you can play whatever card you want.\nOnce all the tricks have been played each team’s score is updated. In addition there are different score values depending on what happens during the course of a hand. If an opposing team chose trump or told the dealer to pick up the top card, but your team won the hand then you get 2 points. In addition if a team wins all 5 tricks in a hand they get 2 points. All other cases in this implementation get 1 point for a hand win with 3 or 4 trick wins, and the winning team chooses trump. Once a team reaches a score of 10 the game finishes and the winner is displayed\nCard Values/Choosing Trump\nEuchre is unique in that the trump suit can change the value of certain cards from game to game. When a user picks a trump suit for a hand, that means that all cards that have that suit are higher in value than cards without that suit. In addition the Jack of that suit becomes the highest card for that hand, and the jack that matches the color of the trump suit (red or black) becomes the second highest card in the game. If a trump suit is not played for a given trick then the suit with the highest value is the suit of the first card played.";
        // System.IO.File.ReadAllText("/Users/jonathanm./Desktop/CS 307/Cool Card Games Project/CS307_cool_card_games/Assets/GameTextFiles");
        // string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/EuchreTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/EuchreTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void SolitareRules()
    {
        //display Solitare Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/SolitaireTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/SolitaireTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void GoFishRules()
    {
        //display GoFish Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void PokerRules()
    {
        //display Poker Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/PokerTextpg1.txt");
        string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/PokerTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void BruhRules()
    {
        //display Bruh Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/BruhTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/BruhTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void UseExitButton()
    {
        Debug.Log("Exit Button Pressed");
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            SceneManager.LoadScene("Scenes/OfflineMainMenu");
        }
        else
        {
            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }
}


