using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingsElement : MonoBehaviour
{
    public TextMeshProUGUI username;
    public TextMeshProUGUI stat;
    public TextMeshProUGUI order;
    public string orderText;

    public void NewRankingElement(string order, string username, string stat)
    {
        this.order.text = order;
        this.username.text = username;
        this.stat.text = stat;
        orderText = order;
    }
}
