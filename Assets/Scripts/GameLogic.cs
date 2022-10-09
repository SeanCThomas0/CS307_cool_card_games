using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;


namespace com.CS307.CoolCardGames.gameLogic
{

    public class gameLogic : MonoBehaviourPunCallbacks
    {
        int score = 0;




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
             // Use this for initialization
        void Start ()
        {
            
        }
        
        // Update is called once per frame
        void Update ()
        {
            
        }

        public void add_score()
        {
            score= score+1;

            counter.text=score.ToString();
        }

        public void sub_score()
        {
            score= score-1;

            counter.text=score.ToString();
        }


        public void switch_turn()
        {
            if(turnText.text=="Player 1")
            {
                turnText.text ="Player 2";
            }
            else {
                turnText.text ="Player 1";
            }


        }





    }
}