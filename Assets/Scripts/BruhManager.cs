using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BruhManager : MonoBehaviour
{

    
    /* Global variables */ 
    //private
    private CardDealer cardDealer;

    //public
    public bool sleeping = false;
    public bool sleepRunning = false;

    public int playerColorConsecCount = 0;
    public int playerNumberConsecCount = 0;
    public int playerGreaterConsecCount = 0;
    public int playerLowerConsecCount = 0;
    public int playerBruhConsecCount = 0;
    public int playerMovesConsecCount = 0; //for playing card >3 times


    /* abstracted methods */
    public bool checkForWinner() { 
        return false;
    } 

    public void gameLoop() {
        //bruh
    }

    // Update is called once per frame
    IEnumerator  sleepFunction() {
        sleeping = true;
        yield return new WaitForSeconds(3);
        sleeping = false;
        sleepRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Bruh game Starting");
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("sleep value: " + sleeping);
        if(sleeping == false) {
            //Debug.Log("Game decision run");
            gameLoop();
        } 
        if(sleepRunning == false){
            sleepRunning = true;
            StartCoroutine(sleepFunction());
            //Debug.Log("sleep running set to true");
        }
    }
}
