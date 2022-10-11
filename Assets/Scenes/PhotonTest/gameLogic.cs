using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using TMPro;
using System;




    public class gameLogic : MonoBehaviourPun
    {

        public  int score = 0;



        //int num_players = PhotonNetwork.PlayerList.Length;





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
            //PhotonNetwork.AutomaticallySyncScene = true;

            
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
            /*
            if(num_players > 0) {
                
                Player cur_player =PhotonNetwork.PlayerList[0];


                string guh =cur_player.NickName;

                cur_player = cur_player.GetNext();

                //this.photonView.RPC("update_turn_text", RpcTarget.All,guh);
                turnText.text=guh;
                


            }
            */
        



        }

        public int get_score()
        {
            return score;
        }

        public void set_score(int val)
        {
            score =val;
        }

        public string get_turn_text()
        {
            return turnText.text;
        }

        public void set_turn_text(string new_text)
        {
            turnText.text=new_text;
        }







    }

