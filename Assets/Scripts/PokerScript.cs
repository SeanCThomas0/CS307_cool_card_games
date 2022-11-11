using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

public class PokerScript : MonoBehaviour
{
    public GameObject GameMessages;
    public GameObject CardDealer;
    public GameObject QuitButton;
    public GameObject ExperiencedButton;
    public GameObject EasyButton;
    private CardDealer cardDealer;
    public bool game_won = false;

    /*Temporary card class for testing to be replaced with CardDealer script*/
    public float userXPos = -7;
    public float userYPos = -124;
    public static bool experienced = false;

    private UserPreferences.backgroundColor backgroundColor;
    public GameObject mainCam;

    /*
    [SerializeField] public AudioSource ClickSound;
    [SerializeField] public AudioSource WinSound;
    [SerializeField] public AudioSource CardSound;
    [SerializeField] public AudioSource Music;
    */
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


    /*CardPlayer class for individual game players*/
    public class CardPlayer
    {
        private List<GameObject> hand;
        private string userID;
        private int teamNumber;
        private bool isHuman;

        public CardPlayer(string userID, int teamNumber, bool isHuman)
        {
            hand = new List<GameObject>();
            this.userID = userID;
            this.teamNumber = teamNumber;
            this.isHuman = isHuman;
        }

        public List<GameObject> getHandList()
        {
            return hand;
        }

        public string getUserID()
        {
            return userID;
        }

        public int getTeamNumber()
        {
            return teamNumber;
        }

        public bool getIsHuman()
        {
            return isHuman;
        }

        public void printHand()
        {
            int count = 1;
            Debug.Log("\n" + userID + "Current hand:");
            foreach (GameObject currentCard in hand)
            {
                Debug.Log(count + ": " + currentCard.GetComponent<Card>().faceValue + " of " + currentCard.GetComponent<Card>().suitValueString);
                count++;
            }
        }

        public void addToHand(GameObject cardToAdd)
        {
            hand.Add(cardToAdd);
        }

        public GameObject removeFromHand(int indexInHand)
        {
            GameObject returnCard = (GameObject)hand[indexInHand];
            hand.RemoveAt(indexInHand);
            return returnCard;
        }

        public GameObject peekAtCard(int indexInHand)
        {
            GameObject returnCard = (GameObject)hand[indexInHand];
            return returnCard;
        }

        /*gets card value and removes it from the User's hand*/
        public GameObject playCard(int indexInHand)
        {
            GameObject returnCard = (GameObject)hand[indexInHand];
            removeFromHand(indexInHand);
            return returnCard;
        }
    }

    //visually show cards 
    private void DisplayOneHand(CardPlayer currentPlayer)
    {
        float x = userXPos;
        float z = 0;

        for (int i = 0; i < currentPlayer.getHandList().Count; i++)
        {
            currentPlayer.peekAtCard(i).transform.position = new Vector3(x, userYPos, z);

            currentPlayer.peekAtCard(i).SetActive(true);

            x = x + 0.45f;
            z = z - 0.1f;
        }
    }

    private void DisplayOnePlay(GameObject currentCard)
    {
        float x = userXPos;
        float z = 0;


        currentCard.transform.position = new Vector3(x + 1, userYPos + 2, z);

        currentCard.SetActive(true);
    }

    private void DisplayTwoHand(GameObject currentCard)
    {
        float x = userXPos;
        float z = 0;


        currentCard.transform.position = new Vector3(x - 5, -4, z);

        currentCard.SetActive(true);

    }

    private void DisplayThreeHand(GameObject currentCard)
    {
        float x = userXPos - 5;
        float z = 0;

        currentCard.transform.position = new Vector3(x + 6, -2, z);

        currentCard.SetActive(true);
    }

    private void DisplayFourHand(GameObject currentCard)
    {
        float x = userXPos;
        float z = 0;

        currentCard.transform.position = new Vector3(x + 7, -4, z);

        currentCard.SetActive(true);
    }

    private void flipCard(GameObject currentCard)
    {
        currentCard.SetActive(false);
    }

    public string currentInput = "empty";
   
    public Queue<CardPlayer> playerQueue = new Queue<CardPlayer>();
    public CardPlayer playerOne;
    public CardPlayer playerTwo;
    public CardPlayer playerThree;
    public CardPlayer playerFour;
    public CardPlayer dealer = null;
    public CardPlayer currentPlayer = null;
    
    public int currentState = 0;
    /*
        0=Initial,
        1=pick up top card
        2 = choose trump
        3 = play card
    */

    public bool sleeping = false;
    public bool sleepRunning = false;
    public List<GameObject> pool;

    public bool databaseUpdated; //Whether or not the database has been updated after the game ends
    //private DatabaseReference databaseReference;
    //private FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        
        /*
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        ClickSound.volume = volumeValue;
        WinSound.volume = volumeValue;
        CardSound.volume = volumeValue / 3;
        */

        cardDealer = CardDealer.GetComponent<CardDealer>();

        Debug.Log("Poker Game Starting");
        GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Poker Game Starting";
        StartCoroutine(sleepFunction());
        pool = cardDealer.RandomCards(52, 1, 13, true, true, true, true);
        
        foreach (GameObject cards in pool)
        {
            //Debug.Log(cards.GetComponent<Card>().numValue + " of " + cards.GetComponent<Card>().suitValue);
            if (cards.GetComponent<Card>().numValue == 11)
            {
                cards.GetComponent<Card>().faceValue = "Jack";
            }
            else if (cards.GetComponent<Card>().numValue == 12)
            {
                cards.GetComponent<Card>().faceValue = "Queen";
            }
            else if (cards.GetComponent<Card>().numValue == 13)
            {
                cards.GetComponent<Card>().faceValue = "King";
            }
            else if (cards.GetComponent<Card>().numValue == 1)
            {
                cards.GetComponent<Card>().numValue = 14;
                cards.GetComponent<Card>().faceValue = "Ace";
            } 
            else
            {
                cards.GetComponent<Card>().faceValue = cards.GetComponent<Card>().numValue.ToString();
            }

            if (cards.GetComponent<Card>().suitValue == Card.suit.CLUBS)
            {
                cards.GetComponent<Card>().suitValueString = "Clubs";
            }
            else if (cards.GetComponent<Card>().suitValue == Card.suit.HEARTS)
            {
                cards.GetComponent<Card>().suitValueString = "Hearts";
            }
            else if (cards.GetComponent<Card>().suitValue == Card.suit.DIAMONDS)
            {
                cards.GetComponent<Card>().suitValueString = "Diamonds";
            }
            else if (cards.GetComponent<Card>().suitValue == Card.suit.SPADES)
            {
                cards.GetComponent<Card>().suitValueString = "Spades";
            }
        }
        

        pool = cardDealer.ShuffleCards(pool);

        currentState = -1;
        
    }

    void gameDecision()
    {
        bool finished = checkForWinner();
        if (!finished)
        {
            //if the game does not have a winner
            if (currentState == -1)
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Select a difficulty button";
                if (currentInput.Equals("Button"))
                {
                    EasyButton.SetActive(false);
                    ExperiencedButton.SetActive(false);
                    currentState = 0;
                    currentInput = "empty";
                    GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Game Starting";
                }
            } 
            else if (currentState == 0)
            {
                //Select dealer and big/small blind
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealer is: ";
                currentState = 1;
            } 
            else if (currentState == 1)
            {
                //get 
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("Show hand");
            }
            else if (currentState == 2)
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("Show hand");
            }
            else if (currentState == 3)
            {
                GameMessages.GetComponent<TMPro.TextMeshProUGUI>().text = "Dealing Cards";
                Debug.Log("Show hand");
            }
            
        }

    }

    IEnumerator sleepFunction()
    {
        sleeping = true;
        yield return new WaitForSeconds(3);
        sleeping = false;
        sleepRunning = false;
    }

    public bool checkForWinner()
    {
        
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("sleep value: " + sleeping);
        if (sleeping == false)
        {
            //Debug.Log("Game decision run");
            gameDecision();
        }
        if (sleepRunning == false)
        {
            sleepRunning = true;
            StartCoroutine(sleepFunction());
            //Debug.Log("sleep running set to true");
        }
    }

    
    public void UseExitButton()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
    

    public void OnEnter(string userInput)
    {
        Debug.Log("User entered " + userInput);
        currentInput = userInput;
    }

    public void ExperiencedMode()
    {
        experienced = true;
        currentInput = "Button";
    }

    public void EasyMode()
    {
        experienced = false;
        currentInput = "Button";
    }

}
