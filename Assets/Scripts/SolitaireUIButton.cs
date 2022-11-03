using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
            if (card.gameObject.name != "Card") {
                Destroy(card.gameObject);
            }
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
    public void Hint() {
        GameObject space1Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space1Card.name = solitaire.playSpace1.Last();
        GameObject space2Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space2Card.name = solitaire.playSpace2.Last();
        GameObject space3Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space3Card.name = solitaire.playSpace3.Last();
        GameObject space4Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space4Card.name = solitaire.playSpace4.Last();
        GameObject space5Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space5Card.name = solitaire.playSpace5.Last();
        GameObject space6Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space6Card.name = solitaire.playSpace6.Last();
        GameObject space7Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space7Card.name = solitaire.playSpace7.Last();
        GameObject space8Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
        space8Card.name = solitaire.tripsOnDisplay.Last();
    }
}