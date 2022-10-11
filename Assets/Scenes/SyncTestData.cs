using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
public class SyncTestData : MonoBehaviourPun
{
    //PhotonView photonView = PhotonView.Get(this);


    gameLogic GameLogic;
    [SerializeField] GameObject GameLogicController;

    private int game_score=0; 
    private string turn_text ="";




    // Start is called before the first frame update
    void Awake()
    {
        GameLogic = GameLogicController.GetComponent<gameLogic>();
    }


    private void Start()
    {  
        
        
    }

    // Update is called once per frame
    private void Update()
    {
        game_score = GameLogic.score;
        
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // writing if you are SETTING data
        // reading if you are GETTING data
        // MUST SET AND GET in same order
        if(stream.IsWriting)
        {
            stream.SendNext(game_score);
            stream.SendNext(turn_text);

        }
        else if(stream.IsReading){
            
            game_score = (int)stream.ReceiveNext();

        }
    }









}
