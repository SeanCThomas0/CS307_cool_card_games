using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SolitaireUIButton : MonoBehaviour
{
    public GameObject highScorePanel;
    public Solitaire solitaire;
    public SolitaireUserInput solitaireUserInput;
    public bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        solitaireUserInput = FindObjectOfType<SolitaireUserInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain() {
        highScorePanel.SetActive(false);
        ResetScene();
    }

    public void ResetScene() {
        clicked = false;
        UpdateSprite[] cards = FindObjectsOfType<UpdateSprite>();
        foreach (UpdateSprite card in cards) {
            Destroy(card.gameObject);
        }
        ClearTopValues();
        FindObjectOfType<Solitaire>().PlayCards();
        
    }

    void ClearTopValues() {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        foreach (Selectable selectable in selectables) {
            if (selectable.CompareTag("Top")) {
                selectable.suit = null;
                selectable.value = 0;
            }
        }
    }

    public void OneMoveFromWin() {
        // solitaire.topPos[0].value = 12;
        // solitaire.topPos[1].value = 13;
        // solitaire.topPos[2].value = 13;
        // solitaire.topPos[3].value = 13;
        // solitaire.topPos[0].suit = "S";
        // solitaire.topPos[1].suit = "D";
        // solitaire.topPos[2].suit = "H";
        // solitaire.topPos[3].suit = "C";
        clicked = true;
        solitaire.topPos[0].GetComponent<Selectable>().value = 12;
        solitaire.topPos[1].GetComponent<Selectable>().value = 13;
        solitaire.topPos[2].GetComponent<Selectable>().value = 13;
        solitaire.topPos[3].GetComponent<Selectable>().value = 13;
        solitaire.topPos[0].GetComponent<Selectable>().suit = "S";
        solitaire.topPos[1].GetComponent<Selectable>().suit = "D";
        solitaire.topPos[2].GetComponent<Selectable>().suit = "H";
        solitaire.topPos[3].GetComponent<Selectable>().suit = "C";
    }

    public void Quit(string scene) {
        Debug.Log("Change to" + scene);
        SceneManager.LoadScene(scene);
    }
}