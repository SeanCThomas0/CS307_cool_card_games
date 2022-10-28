using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using Firebase.Auth;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Linq;
using TMPro;

public class Rankings : MonoBehaviour
{
    private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;
    public GameObject rankingsElement;
    public Transform rankingsContent;

    public GameObject curUserRanking;
    TextMeshProUGUI curUserRankingText;
    public GameObject curUserUsername;
    TextMeshProUGUI curUserUsernameText;
    public GameObject curUserStat;
    TextMeshProUGUI curUserStatText;

    // Start is called before the first frame update
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RankingsButton(string path)
    {
        StartCoroutine(LoadRankingData(path));
    }

    private IEnumerator LoadRankingData(string path)
    {
        if (auth == null)
        {
            auth = FirebaseAuth.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        //Set up text boxes for current user rankings
        curUserRankingText = curUserRanking.GetComponent<TextMeshProUGUI>();
        curUserUsernameText = curUserUsername.GetComponent<TextMeshProUGUI>();
        curUserStatText = curUserStat.GetComponent<TextMeshProUGUI>();

        //Get the current user's username
        string curUsername = null;
        databaseReference.Child("users").Child(auth.CurrentUser.UserId).Child("user_data/username").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("Rankings.cs curUsername canceled");
            }
            if (task.IsFaulted)
            {
                Debug.Log("Rankings.cs curUsername faulted");
            }
            else
            {
                curUsername = task.Result.Value.ToString();
            }
        });

        //Get a list of all children with existing statistics
        var task = databaseReference.Child("users").OrderByChild("game_statistics/" + path).GetValueAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);


        if (task.Exception != null)
        {
            Debug.LogWarning("Failed to grab ranking data: " + task.Exception.ToString());
        }
        else
        {
            DataSnapshot snapshot = task.Result;

            foreach (Transform child in rankingsContent.transform)
            {
                Destroy(child.gameObject);
            }

            int order = 1; //Order of the rankings elements

            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("user_data/username").Value.ToString();
                
                //Debug.Log(username);
                if (childSnapshot.Child("game_statistics/" + path).Value == null)
                {
                    break;
                }

                string stat = childSnapshot.Child("game_statistics/" + path).Value.ToString();

                //Debug.Log(childSnapshot.Key + " " + auth.CurrentUser.UserId);

                //If the current childSnapshot is the current user, set the current user data text to the user's values
                if (childSnapshot.Key.Equals(auth.CurrentUser.UserId))
                {
                    curUserRankingText.text = order.ToString();
                    curUserUsernameText.text = username;
                    curUserStatText.text = stat;
                }

                GameObject rankingsBoardElement = Instantiate(rankingsElement, rankingsContent);
                rankingsElement.GetComponent<RankingsElement>().NewRankingElement(order.ToString(), username, stat);
                Debug.Log("Ranking added: " + order + " " + username + " " + stat);
                order++;
            }

            //If the current user wasn't in the childSnapshots, set the current user data text to null values
            if (curUserUsernameText.Equals(curUsername))
            {
                curUserRankingText.text = "n/a";
                curUserUsernameText.text = curUsername;
                curUserStatText.text = "n/a";
            }

            //These lines are accounting for some bs that doesn't allow the last player to be printed. You can ignore these for now
            GameObject blankElement = Instantiate(rankingsElement, rankingsContent);
            rankingsElement.GetComponent<RankingsElement>().NewRankingElement("", "", "");

            foreach (Transform child in rankingsContent.transform)
            {
                if (child.GetComponent<RankingsElement>().orderText == "")
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void TestRankingData()
    {
        GameObject rankingsBoardElement = Instantiate(rankingsElement, rankingsContent);
        rankingsElement.GetComponent<RankingsElement>().NewRankingElement("almost 0", "Grant", "9001");
    } 
}
