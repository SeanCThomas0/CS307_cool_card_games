using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    // public Sprite cardFace;
    // public Sprite cardBack;
    // private SpriteRenderer spriteRenderer;
    // private Selectable selectable;
    // private Solitaire solitaire;
    // private SolitaireUserInput solitaireUserInput;


    // // Start is called before the first frame update
    // void Start()
    // {
    //     List<string> deck = Solitaire.GenerateDeck();
    //     solitaire = FindObjectOfType<Solitaire>();
    //     solitaireUserInput = FindObjectOfType<SolitaireUserInput>();

    //     int i = 0;
    //     foreach (string card in deck) {
    //         if (this.name == card) {
    //             cardFace = solitaire.cardFaces[i];
    //             break;
    //         }
    //         i++;
    //     }
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    //     selectable = GetComponent<Selectable>();

    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (selectable.faceUp == true) {
    //         spriteRenderer.sprite = cardFace;
    //     } else {
    //         spriteRenderer.sprite = cardBack;
    //     }
    //     if (solitaireUserInput.slot1) {
    //         if (name == solitaireUserInput.slot1.name) {
    //             spriteRenderer.color = Color.yellow;
    //         } else {
    //             spriteRenderer.color = Color.white;
    //         }
    //     }
    // }
}
