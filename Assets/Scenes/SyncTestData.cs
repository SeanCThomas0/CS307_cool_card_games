using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
[RequireComponent(typeof(PhotonView))]
public class SyncTestData : MonoBehaviourPun ,IPunObservable 
{
    //PhotonView photonView = PhotonView.Get(this);


    gameLogic GameLogic;
    [SerializeField] GameObject GameLogicController;

    [SerializeField]
    public int game_score =0;

    [SerializeField]
    public string turn_text ="";





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
        if (photonView.IsMine ){
                game_score = GameLogic.score;
        }
        GameLogic.score = game_score;
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // writing if you are SETTING data
        // reading if you are GETTING data
        // MUST SET AND GET in same order
        if(stream.IsWriting)
        {
            stream.SendNext(game_score);
            Debug.Log("STREAM WAS WROTE");
            Debug.Log(game_score);

        }
        else{
            
            game_score = (int)stream.ReceiveNext();
            Debug.Log("STREAM WAS READ");
            Debug.Log(game_score);
            GameLogic.counter.text=game_score.ToString();

        }
    }










}
