using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEuchre : MonoBehaviour
{
    public GameObject UserPlayer;
    public GameObject Opponent1;
    public GameObject PartnerPlayer;
    public GameObject Opponent2;
    public GameObject CardArea;
    public GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCLick() {
        GameObject cardOne = Instantiate(UserPlayer, new Vector3(0,0,0), Quaternion.identity);
    }
}
