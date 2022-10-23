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

public class Rankings : MonoBehaviour
{
    private string DATA_URL = "https://cool-card-games-default-rtdb.firebaseio.com/";

    private DatabaseReference databaseReference;
    private FirebaseAuth auth;
    public GameObject rankingsElement;
    public Transform rankingsContent;

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
                Debug.Log(username);
                if (childSnapshot.Child("game_statistics/" + path).Value == null)
                {
                    break;
                }

                string stat = childSnapshot.Child("game_statistics/" + path).Value.ToString();

                GameObject rankingsBoardElement = Instantiate(rankingsElement, rankingsContent);
                rankingsElement.GetComponent<RankingsElement>().NewRankingElement(order.ToString(), username, stat);
                Debug.Log("Ranking added: " + order + " " + username + " " + stat);
                order++;
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
