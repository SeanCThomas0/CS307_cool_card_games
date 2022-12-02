using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFishRules : MonoBehaviour
{
    public GameObject TextBox1;
    public GameObject TextBox2;
    // Start is called before the first frame update
    void Start()
    {
         //display GoFish Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
