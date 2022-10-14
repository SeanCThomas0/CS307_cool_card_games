using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;
[RequireComponent(typeof(PhotonView))]
public class GoFishMultiplayer : MonoBehaviourPun, IPunObservable
{

    GoFishLogic goFishLogic;
    [SerializeField] GameObject LogicController;

    Photon.Realtime.Player localPlayerVar = PhotonNetwork.LocalPlayer;
    bool isMasterPlayer;

    Photon.Realtime.Player[] playerListArr = PhotonNetwork.PlayerList;

    int next_player =0;
    int cur_player =0;

    






    // Start is called before the first frame update
    void Start()
    {

        isMasterPlayer = photonView.IsMine;
        if (photonView.IsMine ){

        }
        else {

        }
        
    }

    void Awake() {
        goFishLogic = LogicController.GetComponent<GoFishLogic>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


     public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // writing if you are SETTING data
        // reading if you are GETTING data
        // MUST SET AND GET in same order
        if(stream.IsWriting)
        {
            //stream.SendNext(game_score);
            Debug.Log("STREAM WAS WROTE");
            //Debug.Log(game_score);

        }
        else{
            
            //game_score = (int)stream.ReceiveNext();
            Debug.Log("STREAM WAS READ");
            //Debug.Log(game_score);
            //GameLogic.counter.text=game_score.ToString();

        }
    }









}
