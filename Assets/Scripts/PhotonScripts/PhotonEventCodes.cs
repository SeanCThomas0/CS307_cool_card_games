using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhotonEventCodes
{
    HostToClientData        =0,
    StartGame               =1,
    HostSendCardsToPlayer   =2,
    GoFishFourOf            =3,
    WinGame                 =4,
    ExitGame                =5,
    HostRemoveCardFromPlayer=6,
    SyncCardsAtStart        =7,
    HostSendRoomInfo        =8
}
