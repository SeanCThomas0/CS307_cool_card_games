using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{


    public Text finalTimeText;
    public Selectable[] topStacks;
    public GameObject highScorePanel;
    public GameObject gamePanel;
    private bool scoreUpdated = false;
    //Database database;
    DatabaseReference userRef;
    private SolitaireUserInput solitaireUserInput;
    Database database;
    Firebase.Auth.FirebaseUser user;
    int win_count;
    int finalTime;

    [SerializeField] public AudioSource WinSound;
    [SerializeField] public AudioSource Music;
 




    // Start is called before the first frame update
    void Start()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        WinSound.volume = volumeValue;
        solitaireUserInput = FindObjectOfType<SolitaireUserInput>();


        user = FirebaseAuth.DefaultInstance.CurrentUser;
        userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(user.UserId);

        userRef.Child("game_statistics").Child("solitaire").Child("win_count").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result.Value == null)
                {
                    Debug.Log("GetUserScore Null");
                    win_count = 0;
                }
                else
                {
                    Debug.Log("GetUserScore Success");
                    Debug.Log(task.Result.Value.ToString());
                    win_count = Int32.Parse(task.Result.Value.ToString());
                }

            }
            else
            {
                Debug.Log("GetUserScore Fail");
                win_count = 0;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (HasWon() && !scoreUpdated) {
            
            WinSound.Play();
            Music.Pause();
            Win();
        }
    }

    public bool HasWon() {
        int i = 0;
        foreach (Selectable topstack in topStacks) {
            i += topstack.value;
        }
        if (i >= 52) {
            return true;
        } else {
            return false;
        }
    }

    void Win() {
        finalTime = (int) solitaireUserInput.totalTimer;
        finalTimeText.text = finalTime.ToString();
        scoreUpdated = true;
        highScorePanel.SetActive(true);
        gamePanel.SetActive(false);

        print("You have won!");
        win_count++;
        userRef.Child("game_statistics/solitaire/win_count").SetValueAsync(win_count);

        Debug.Log("New win_count: " + win_count);
    }
}
