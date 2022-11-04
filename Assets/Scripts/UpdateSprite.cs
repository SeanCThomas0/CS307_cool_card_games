using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Solitaire solitaire;
    private SolitaireUserInput solitaireUserInput;
    private SolitaireUIButton solitaireUIButton;


    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = Solitaire.GenerateDeck();
        solitaire = FindObjectOfType<Solitaire>();
        solitaireUserInput = FindObjectOfType<SolitaireUserInput>();
        solitaireUIButton = FindObjectOfType<SolitaireUIButton>();

        int i = 0;
        foreach (string card in deck) {
            if (this.name == card) {
                cardFace = solitaire.cardFaces[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!solitaireUIButton.hintClicked) {
            if (selectable.faceUp == true) {
                spriteRenderer.sprite = cardFace;
            } else {
                spriteRenderer.sprite = cardBack;
            }
            if (solitaireUserInput.slot1) {
                if (name == solitaireUserInput.slot1.name) {
                    spriteRenderer.color = Color.yellow;
                } else {
                    spriteRenderer.color = Color.white;
                }
            }
        } else {
            if (selectable.faceUp == true) {
                spriteRenderer.sprite = cardFace;
            } else {
                spriteRenderer.sprite = cardBack;
            }
            for (int i = 0; i < solitaireUIButton.possibleHints.Count; i++) {
                for (int j = 0; j < solitaireUIButton.possibleHints.Count; j++) {
                    if (i == j) {
                        j++;
                    }
                    if (j == solitaireUIButton.possibleHints.Count) {
                        break;
                    }
                    solitaireUserInput.slot1 = solitaireUIButton.possibleHints[i];
                    if (solitaireUserInput.Stackable(solitaireUIButton.possibleHints[j]) && (name == solitaireUIButton.possibleHints[j].name || name == solitaireUserInput.slot1.name)) {
                        Debug.Log("match");
                        solitaireUserInput.slot1 = solitaire.gameSpaces[i].Last();
                        solitaireUserInput.Stack(solitaire.gameSpaces[j].Last());
                        solitaireUIButton.hintClicked = false;
                        break;
                    }
                }
            }
            solitaireUserInput.slot1 = this.gameObject;
        }
    }
}
