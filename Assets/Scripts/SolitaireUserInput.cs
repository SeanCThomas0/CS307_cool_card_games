using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolitaireUserInput : MonoBehaviour
{
    private Solitaire solitaire;
    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) {
                //what has been hit? I.E. Deck/Card/EmptySlot/ETC...
                if (hit.collider.CompareTag("Deck")) {
                    //clicked Deck
                    Deck();
                } else if (hit.collider.CompareTag("Card")) {
                    //clicked Card
                    Card();
                } else if (hit.collider.CompareTag("Top")) {
                    //clicked Top
                    Top();
                } else if (hit.collider.CompareTag("Bottom")) {
                    //clicked Bottom
                    Bottom();
                }
            }
        }
    }
    void Deck() {
        //deck click actions
        print("clicked on deck");
        solitaire.DealFromDeck();
    }
    
    void Card() {
        //card click actions
        print("clicked on card");
    }

    void Top() {
        //top click actions
        print("clicked on top");
    }

    void Bottom() {
        //bottom click actions
        print("clicked on bottom");
    }
}
