using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEuchre : MonoBehaviour
{
    public GameObject handcard1;
    public GameObject handcard2;
    public GameObject handcard3;
    public GameObject handcard4;
    public GameObject handcard5;
    public GameObject handarea;
    public GameObject UserPlayer;
    public GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Euchre Game Starting");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        Debug.Log("Deal button clicked");
        GameObject card1Instance = Instantiate(handcard1, new Vector3(1, 0, 0), Quaternion.identity);
        GameObject card2Instance = Instantiate(handcard2, new Vector3(-1, 0, 0), Quaternion.identity);
        GameObject card3Instance = Instantiate(handcard3, new Vector3(1, 0, 0), Quaternion.identity);
        GameObject card4Instance = Instantiate(handcard4, new Vector3(-1, 0, 0), Quaternion.identity);
        GameObject card5Instance = Instantiate(handcard5, new Vector3(1, 0, 0), Quaternion.identity);

        card1Instance.transform.SetParent(handarea.transform, false);
        card2Instance.transform.SetParent(handarea.transform, false);
        card3Instance.transform.SetParent(handarea.transform, false);
        card4Instance.transform.SetParent(handarea.transform, false);
        card5Instance.transform.SetParent(handarea.transform, false);

        // card1Instance.transform.scale = new Vector3(0.5f, 0.5f, 0.5f);  
    }
}


