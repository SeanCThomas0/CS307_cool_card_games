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

    [Tooltip("pass button")]
    [SerializeField]
    public GameObject passButton;

    Player localPlayerVar = PhotonNetwork.LocalPlayer;
    bool isMasterPlayer;

    Player[] playerListArr = PhotonNetwork.PlayerList;

    int next_player =0;





    // Start is called before the first frame update
    void Awake()
    {
        
        GameLogic = GameLogicController.GetComponent<gameLogic>();
        
        for (int i = 0; i < playerListArr.Length;i++) {
            if (playerListArr[i] == localPlayerVar) {
                next_player = ((i+1)%2);
            }
        }
        

        



    }


    private void Start()
    {  

        isMasterPlayer = photonView.IsMine;
        if (photonView.IsMine ){
            GameLogic.turnText.text =PhotonNetwork.LocalPlayer.NickName;
            GameLogic.passButton.SetActive(false);
        }
        else {
            GameLogic.add.SetActive(false);
            GameLogic.subtract.SetActive(false);

        }

    }

    // Update is called once per frame
    private void Update()
    {
        if (photonView.IsMine ){
            GameLogic.turnText.text =PhotonNetwork.LocalPlayer.NickName;
            GameLogic.passButton.SetActive(false);
            GameLogic.add.SetActive(true);
            GameLogic.subtract.SetActive(true);


        }
        else {
            GameLogic.add.SetActive(false);
            GameLogic.subtract.SetActive(false);
            GameLogic.passButton.SetActive(true);

        }
        isMasterPlayer = photonView.IsMine;
        if (photonView.IsMine ){
                game_score = GameLogic.score;
        }
        GameLogic.score = game_score;

        isMasterPlayer = photonView.IsMine;
        //Debug.Log(isMasterPlayer);


        
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

    public void OnPassButton () {


        photonView.TransferOwnership(localPlayerVar.ActorNumber);

    }













}
