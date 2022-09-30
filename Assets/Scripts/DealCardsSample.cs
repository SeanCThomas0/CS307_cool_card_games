using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealCardsSample : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject square;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        GameObject card1Instance = Instantiate(card1, new Vector3(1, 0, 0), Quaternion.identity);
        GameObject card2Instance = Instantiate(card2, new Vector3(-1, 0, 0), Quaternion.identity);

        card1Instance.transform.SetParent(square.transform, false);
        card2Instance.transform.SetParent(square.transform, false);

        // card1Instance.transform.scale = new Vector3(0.5f, 0.5f, 0.5f);  
    }
}
