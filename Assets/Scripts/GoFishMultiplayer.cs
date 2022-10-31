using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class GoFishMultiplayer : MonoBehaviourPun

{
    [SerializeField]
    public GameObject GoFishLogic;
    public List<GameObject> curPool;
    // Start is called before the first frame update
    void Start()
    {
        curPool = GoFishLogic.GetComponent<GoFishLogic>().pool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake() {

    }

    [PunRPC]
    void updatePool(List<GameObject> guh)
    {
        curPool = guh;
        //wha
    }

}
