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
    private SpriteRenderer spriteRenderer;
    public bool hintClicked = false;
    public List<GameObject> possibleHints;
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
        if (solitaire.playSpace1.Count != 0) {
            GameObject space1Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space1Card.name = solitaire.playSpace1.Last();
            possibleHints.Add(space1Card);
        }
        if (solitaire.playSpace2.Count != 0) {
            GameObject space2Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space2Card.name = solitaire.playSpace2.Last();
            possibleHints.Add(space2Card);
        }
        if (solitaire.playSpace3.Count != 0) {
            GameObject space3Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space3Card.name = solitaire.playSpace3.Last();
            possibleHints.Add(space3Card);
        }
        if (solitaire.playSpace4.Count != 0) {
            GameObject space4Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space4Card.name = solitaire.playSpace4.Last();
            possibleHints.Add(space4Card);
        }
        if (solitaire.playSpace5.Count != 0) {
            GameObject space5Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space5Card.name = solitaire.playSpace5.Last();
            possibleHints.Add(space5Card);
        }
        if (solitaire.playSpace6.Count != 0) {
            GameObject space6Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space6Card.name = solitaire.playSpace6.Last();
            possibleHints.Add(space6Card);
        }
        if (solitaire.playSpace7.Count != 0) {
            GameObject space7Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space7Card.name = solitaire.playSpace7.Last();
            possibleHints.Add(space7Card);
        }
        if (solitaire.tripsOnDisplay.Count != 0) {
            GameObject space8Card = Instantiate(solitaire.cardPrefab, new Vector3(100, 100, 0), Quaternion.identity);
            space8Card.GetComponent<Selectable>().inDeckPile = true;
            space8Card.name = solitaire.tripsOnDisplay.Last();
            possibleHints.Add(space8Card);
        }
        hintClicked = true;
        // for (int i = 0; i < possibleHints.Count; i++) {
        //     for (int j = 0; j < possibleHints.Count; j++) {
        //         if (i == j) {
        //             j++;
        //         }
        //         if (j == possibleHints.Count) {
        //             break;
        //         }
        //         solitaireUserInput.slot1 = possibleHints[i];
        //         if (solitaireUserInput.Stackable(possibleHints[j]) && name == solitaireUserInput.slot1.name) {
        //             Debug.Log("match");
        //             spriteRenderer.color = Color.yellow;
        //             break;
        //         }
        //     }
        // }
        // solitaireUserInput.slot1 = this.gameObject;
    }
}