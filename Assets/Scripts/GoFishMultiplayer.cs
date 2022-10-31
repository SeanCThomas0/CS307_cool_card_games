using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class GoFishMultiplayer : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {

    }

    //public const byte UpDateGoFishPoolCode = 1;




    private void SendGoFishPoolEvent()
    {
        
    }


    public void OnEvent(EventData photonEvent) {
    byte eventCode = photonEvent.Code;

        if (eventCode == UpDateGoFishPoolCode) {

        }
    }
}
