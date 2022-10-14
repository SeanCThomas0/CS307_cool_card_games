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


    public List<GameObject> pool2;









    // Start is called before the first frame update
    void Start()
    {
        pool2 =goFishLogic.pool;

        isMasterPlayer = photonView.IsMine;
        if (photonView.IsMine ){

        }
        else {

        }
        
    }

    void Awake() {
        goFishLogic = LogicController.GetComponent<GoFishLogic>();

        for (int i = 0; i < playerListArr.Length;i++) {
            if (playerListArr[i] == localPlayerVar) {
                next_player = ((i+1)%4);
            }

        }
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
            stream.SendNext(pool2);
            Debug.Log("STREAM WAS WROTE");
            //Debug.Log(game_score);

        }
        else{
            
            pool2 = (List<GameObject>)stream.ReceiveNext();
            Debug.Log("STREAM WAS READ");
            //Debug.Log(game_score);
            //GameLogic.counter.text=game_score.ToString();

        }
    }









}