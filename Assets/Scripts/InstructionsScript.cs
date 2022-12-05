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
        string text1 = "Euchre Rules:\nGameflow\n\n\tEuchre is a game played by 4 players, with two players each on opposing teams. The game only uses cards with default values from lowest to highest: 9, 10, Jack, Queen, King, Ace (value hierarchy explained in card values section). The first team to reach a score of 10 wins the game, and a player increases their score by winning a trick (which will be explained later).\n\tPlay begins when a dealer is chosen from one of the 4 players, cards are dealt to each player, and the remaining 4 cards (kitty) are placed in the middle of the table with the card on the top being flipped over. The players take turns, starting with the player to the left of the dealer, choosing whether to have the dealer pick up the top card that is displayed and replace it with a card in their hand, or pass the decision off to the next player in line. If a player decides to have the dealer pick up the card on the top of the kitty, the trump suit is set to that top card, the dealer picks it up and discards a card in their hand, and card play begins. If all players decide to pass and not have the dealer pick up the top card, then action begins again  to the left of the dealer with players deciding to either choose a trump suit or pass. If all players pass, the dealer must choose a suit.\n\tOnce a trump suit is chosen or top card is picked up players begin to play five tricks, with the winner of the hand gaining a point. Players take turns choosing 1 card to play, and the highest card out of the 4 cards played in a given trick wins that trick.\n\tThe player to the left of the dealer starts on the first trick, and after that the winner of the previous trick starts play. If the lead player plays a card, and you have a card with the same suit in your hand, you must play a card that matches that lead suit, otherwise if you don’t have to follow suit you can play whatever card you want.\n\tOnce all the tricks have been played each team’s score is updated. In addition there are different score values depending on what happens during the course of a hand. If an opposing team chose trump or told the dealer to pick up the top card, but your team won the hand then you get 2 points. In addition if a team wins all 5 tricks in a hand they get 2 points. All other cases in this implementation get 1 point for a hand win with 3 or 4 trick wins, and the winning team chooses trump. Once a team reaches a score of 10 the game finishes and the winner is displayed\n\tCard Values/Choosing Trump\n\tEuchre is unique in that the trump suit can change the value of certain cards from game to game. When a user picks a trump suit for a hand, that means that all cards that have that suit are higher in value than cards without that suit. In addition the Jack of that suit becomes the highest card for that hand, and the jack that matches the color of the trump suit (red or black) becomes the second highest card in the game. If a trump suit is not played for a given trick then the suit with the highest value is the suit of the first card played.";
        // System.IO.File.ReadAllText("/Users/jonathanm./Desktop/CS 307/Cool Card Games Project/CS307_cool_card_games/Assets/GameTextFiles");
        // string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/EuchreTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/EuchreTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void SolitareRules()
    {
        //display Solitare Rules in textBox
        string text1 = "Solitaire Game Rules	\n\n\tSolitaire is a single player game that primarily focuses on sorting. The version we are using for our application is klondike solitaire. During the course of the game a player has to deal with four types of card piles: the tableau, the stock, the foundation, and the waste. The foundation is a set of four piles where each pile corresponds to a suit. \n\tThe goal of the game is to sort cards of each suit from highest to lowest value, with ace being the lowest value, and king being the highest value. The game begins with many cards already dealt in the tableau and the stock holding the remaining cards face down. The player then must move cards into the foundation from the tableau in sequential order from lowest to highest based on the card suit. If no top cards that are flipped over in the tableau can be moved to the foundation a player can move already flipped over cards from stack to stack if the card is lower in value than the card it is going on top of and of opposite color. Then when a card is moved from a tableau column, if the card below it is not already flipped over, that card is flipped over to show its value. If a player has rearranged cards in the tableau and they can no longer move cards within the tableau or to the foundation then three cards from the stock can be moved to the waste. If the top card on the waste pile cannot be played then the user can deal out another 3 cards from the waste pile and put the previous 3 cards into the waste pile.\n\tEventually if the user is able to work the cards around to organize them in the foundations from smallest to largest value then they win. If at a certain point no possible moves are available for the user make from the tableau or the waste then the player loses the game.";
        //string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/SolitaireTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/SolitaireTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void GoFishRules()
    {
        //display GoFish Rules in textBox
        string text1 = "Go Fish Game Rules\n\n\tGo Fish is a game that in our application can be played with between 2 and 4 players. The goal of the game is to acquire sets of 4 cards of the same value, with the player who is able to clear their hand first winning the game. \n\tThe game begins when the dealer gives 5 cards to each player in the game. Players in the game take turns asking the other players if they have a card that the user themselves has in their hand and is trying to make a set of 4 of. If a player asks another player for say a 4, and they have any number of 4’s they must give all your 4’s to that player. If a player guesses that a player has a given card correctly, that player gets to go again. If a player asks another player for a card and they don’t have it, the other player tells the requesting player to go fish, and the requesting player takes a card out of the middle pile and adds it to their hand. Once a player is told to go fish the next player has their opportunity to ask for cards.\n\tThis sequence repeats until a player gets rid of all the cards in their hand by creating sets of 4, and the player who does that is declared the winner, ending the game.";
        //string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void PokerRules()
    {
        //display Poker Rules in textBox
        string text1 = "Texas Hold 'em Poker Game Rules\n\n\tTexas Hold ‘em Poker is a poker variation that in this implementation can have between 2 and 6 players. Players each start with $150 of fake money to bet on the game with. The goal of the game is to be the player to either have the most money after a set number of rounds, or be the last player to have money. Money is represented in the game with different colored chips, and those chips are used to bet. \n\tWhen a round begins one player is designated as the dealer who gives every player 2 cards, one player is designated as the small blind who has to bet $1 automatically, and the large blind who has to bet $2 automatically. These roles rotate to the left of the player circle after each hand. The small blind is the player to the left of the dealer and the large blind is the player to the left of the small blind. Note that if there are only 2 players in a game then each player bets $2 automatically. Then betting begins with the player to the left of the big blind. They can choose to bet any number of chips as long as they have that many chips, and each player gets a chance to match or raise those pre flop bets. If a player doesn’t want to play their hand or match a bet then they can choose to fold their hand and discard their two cards, this can be done at any time. Once all players that are still playing have matched the top bet 3 cards are dealt off the top of the remaining deck. Players can then choose to check (not bet) and move the choice to the next player, or bet based on the cards shown. Again players can fold if they don’t want to match a raised bet. Once all players have matched the bet then another card is taken off the top of the deck and shown. Betting/folding happens again and then the final card is played off the top of the deck. The final round of betting then begins, and if there is more than one player left in after betting has concluded then the players left in show the cards in their hand and a winner is determined. If at any time during play all players fold but one then that player wins the hand and gets to talk the money in the pot that holds all bets for the round. \n\tThere are a number of different card combinations that are used to determine which player won. If two or more players tie for the win then the pot is split. Otherwise the hierarchy for card combination wins is as follows for the top 7 card combinations a user may make from highest to lowest: 1. Royal flush 2. straight flush 3. four of a kind 4. full house 5. flush 6. straight 7. three of a kind 8. two pairs 9. single pair 10. highest card. For more information on the specifics of these rankings and what the terminology may be for each look up texas hold em card rankings online, or look up the name of the card combination. \n\tOnce all players that are still playing have matched the top bet 3 cards are dealt off the top of the remaining deck. Players can then choose to check (not bet) and move the choice to the next player, or bet based on the cards shown. Again players can fold if they don’t want to match a raised bet. Once all players have matched the bet then another card is taken off the top of the deck and shown. Betting/folding happens again and then the final card is played off the top of the deck. The final round of betting then begins, and if there is more than one player left in after betting has concluded then the players left in show the cards in their hand and a winner is determined. If at any time during play all players fold but one then that player wins the hand and gets to talk the money in the pot that holds all bets for the round. \n\tThere are a number of different card combinations that are used to determine which player won. If two or more players tie for the win then the pot is split. Otherwise the hierarchy for card combination wins is as follows for the top 7 card combinations a user may make from highest to lowest: 1. Royal flush 2. straight flush 3. four of a kind 4. full house 5. flush 6. straight 7. three of a kind 8. two pairs 9. single pair 10. highest card. For more information on the specifics of these rankings and what the terminology may be for each look up texas hold em card rankings online, or look up the name of the card combination.";
        //string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/PokerTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/PokerTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void BruhRules()
    {
        //display Bruh Rules in textBox
        string text1 = "Bruh Game rules\n\n\tBruh is a card game that is unique to our application. It is a single player game where the goal is to get rid of all cards in your hand. While it is a single player game, there are computer players that can work to either trick or help you win the game. In this aspect the game is probably most similar to the card game Mao which requires a user to follow a certain set of rules to get rid of the cards in their hand. However, bruh is different in that it has different unwritten rules for the user to figure out, as well as a computer companion and computer opponent that change the dynamics of the game significantly. \n\tThe game begins when the computer opponent gives the player 7 cards to start with. The computer opponent then gives the user a comment that they can either follow or ignore. The player may choose to ignore the comment as the computer opponent chooses randomly whether to give the user helpful advice. The player plays one card, and the opponent either tells the user that it is a valid move, or that it is invalid, and the player will either have to just take their card back, or take their card back and pick up another card. In addition a user has a computer companion that they can use to give them advice and suggested moves that will always be correct. The only caveat is that if a user chooses to use the computer companion they must wait 3 moves to use it again. \n\tThe game finishes when a user wins and gets rid of all the cards in their hand, or they lose by amassing 20 or more cards in their hand.";
        //string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/BruhTextpg1.txt");
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


