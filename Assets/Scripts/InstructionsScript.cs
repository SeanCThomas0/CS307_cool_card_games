using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScript : MonoBehaviour
{
    public GameObject EuchreButton;
    public GameObject SolitareButton;
    public GameObject GoFishButton;
    public GameObject PokerButton;
    public GameObject BruhButton;
    public GameObject ExitButton;
    public GameObject TextBox1;
    public GameObject TextBox2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Rules Page Opened");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EuchreRules() {
        //display Euchre Rules in textBox
        Debug.Log("Euchre Button Pressed");
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/EuchreTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/EuchreTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void SolitareRules() {
        //display Solitare Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/SolitaireTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/SolitaireTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void GoFishRules() {
        //display GoFish Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/GoFishTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void PokerRules() {
        //display Poker Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/PokerTextpg1.txt");
        string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/PokerTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void BruhRules() {
        //display Bruh Rules in textBox
        string text1 = System.IO.File.ReadAllText("Assets/GameTextFiles/BruhTextpg1.txt");
        //string text2 = System.IO.File.ReadAllText("Assets/GameTextFiles/BruhTextpg2.txt");
        TextBox1.GetComponent<TMPro.TextMeshProUGUI>().text = text1;
        //TextBox2.GetComponent<TMPro.TextMeshProUGUI>().text = text2;
    }

    public void UseExitButton() {
        Debug.Log("Exit Button Pressed");
        SceneManager.LoadScene("Scenes/MainMenu");
    }

}


