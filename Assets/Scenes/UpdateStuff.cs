using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class UpdateStuff : MonoBehaviourPun
{
    PhotonView photonView;

        [Tooltip("add button")]
        [SerializeField]
        private GameObject add;

        [Tooltip("sub button")]
        [SerializeField]
        private GameObject subtract;

        [Tooltip("pass button")]
        [SerializeField]
        private GameObject passButton;

        [Tooltip("score text")]
        [SerializeField]
        private TMP_Text counter;

        [Tooltip("turn text")]
        [SerializeField]
        private TMP_Text turnText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    void update_turn_text(string new_player_name) {
        turnText.text =new_player_name;
    }

}
