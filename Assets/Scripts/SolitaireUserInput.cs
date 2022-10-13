using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolitaireUserInput : MonoBehaviour
{
    public GameObject slot1;
    
    private Solitaire solitaire;
    private float timer;
    private float doubleClickTime = 0.3f;
    private int clickCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (clickCount == 1) {
            timer += Time.deltaTime;
        }
        if (clickCount == 3) {
            timer = 0;
            clickCount = 1;
        }
        if (timer > doubleClickTime) {
            timer = 0;
            clickCount = 0;
        }
        GetMouseClick();
    }

    void GetMouseClick() {
        if (Input.GetMouseButtonDown(0)) {
            clickCount++;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) {
                //what has been hit? I.E. Deck/Card/EmptySlot/ETC...
                if (hit.collider.CompareTag("Deck")) {
                    //clicked Deck
                    Deck();
                } else if (hit.collider.CompareTag("Card")) {
                    //clicked Card
                    Card(hit.collider.gameObject);
                } else if (hit.collider.CompareTag("Top")) {
                    //clicked Top
                    Top(hit.collider.gameObject);
                } else if (hit.collider.CompareTag("Bottom")) {
                    //clicked Bottom
                    Bottom(hit.collider.gameObject);
                }
            }
        }
    }
    void Deck() {
        //deck click actions
        print("clicked on deck");
        solitaire.DealFromDeck();
        slot1 = this.gameObject;
    }
    
    void Card(GameObject selected) {
        //card click actions
        print("clicked on card");

        if (!selected.GetComponent<Selectable>().faceUp) {//if the card clicked on is facedown
            //if the card clicked on is not blocked
                //flip it over
            if (!Blocked(selected)) {
                selected.GetComponent<Selectable>().faceUp = true;
                slot1 = this.gameObject;
            }     
        } else if (selected.GetComponent<Selectable>().inDeckPile) {
            if (!Blocked(selected)) {
                if (slot1 == selected) {
                    if (DoubleClick()) {
                        AutoStack(selected);
                    }
                } else {
                    slot1 = selected;
                }
            }
        } else {
            //if the card clicked on is in the deck pile with the trips
                //if it is not blocked
                    //select it
            
            //if the card is face up
                //if there is no card currently selected
                    //select the card

            if (slot1 == this.gameObject) {
                slot1 = selected;
            }

            //if there is already a card selected (and it is not the same card)
                //if the new card is eligible to stack on the old card
                    //stack it
                //else
                    //select the new card

            else if (slot1 != selected) {
                if (Stackable(selected)) {
                    Stack(selected);
                } else {
                    slot1 = selected;
                }
            }

            else if (slot1 == selected) {
                if (DoubleClick()) {
                    AutoStack(selected);
                }
            }
        }

        //else if there is already a card selected and it is the same carx
            //if the time is short enough then it is a double click
                //if the card is eligible to fly up top then do it

    }

    void Top(GameObject selected) {
        //top click actions
        print("clicked on top");
        if (slot1.CompareTag("Card")) {
            if (slot1.GetComponent<Selectable>().value == 1) {
                Stack(selected);
            }
        }
    }

    void Bottom(GameObject selected) {
        //bottom click actions
        print("clicked on bottom");
        if (slot1.CompareTag("Card")) {
            if (slot1.GetComponent<Selectable>().value == 13) {
                Stack(selected);
            }
        }
    }

    bool Stackable(GameObject selected) {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        // compare them to see if they stack

        if (!s2.inDeckPile) {
            if (s2.top) { // if in the top pile must stack suited Ace to King
                if (s1.suit == s2.suit || (s1.value == 1 && s2.suit == null)) {
                    if (s1.value == s2.value + 1) {
                        return true;
                    }
                } else {
                    return false;
                }
            } else { // if in the bottom pile must stack alternate colors King to Ace
                if (s1.value == s2.value - 1) {
                    bool card1Red = true;
                    bool card2Red = true;

                    if (s1.suit == "C" || s1.suit == "S") {
                        card1Red = false;
                    }

                    if (s2.suit == "C" || s2.suit == "S") {
                        card2Red = false;
                    }

                    if (card1Red == card2Red) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void Stack(GameObject selected) {
        // if on top of King or empty bottom stack the cards in place
        // else stack the cards with a negative y offset
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = selected.GetComponent<Selectable>();
        float yOffset = 0.3f;

        if (s2.top || (!s2.top && s1.value == 13)) {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z - 0.01f);
        slot1.transform.parent = selected.transform;

        if (s1.inDeckPile) {
            solitaire.tripsOnDisplay.Remove(slot1.name);
        } else if (s1.top && s2.top && s1.value == 1) {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = 0;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = null;
        } else if (s1.top) {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value - 1;
        } else {
            solitaire.playSpaces[s1.row].Remove(slot1.name);
        }

        s1.inDeckPile = false;
        s1.row = s2.row;

        if (s2.top) {
            solitaire.topPos[s1.row].GetComponent<Selectable>().value = s1.value;
            solitaire.topPos[s1.row].GetComponent<Selectable>().suit = s1.suit;
            s1.top = true;
        } else {
            s1.top = false;
        }

        slot1 = this.gameObject;
    }

    bool Blocked(GameObject selected) {
        Selectable s2 = selected.GetComponent<Selectable>();
        if (s2.inDeckPile == true) {
            if (s2.name == solitaire.tripsOnDisplay.Last()) {
                return false;
            } else {
                return true;
            }
        } else {
            if (s2.name == solitaire.playSpaces[s2.row].Last()) {
                return false;
            } else {
                return true;
            }
        }
    }

    bool DoubleClick() {
        if (timer < doubleClickTime && clickCount == 2) {
            return true;
        }
        else {
            return false;
        }
    }

    void AutoStack(GameObject selected) {
        for (int i = 0; i < solitaire.topPos.Length; i++) {
            Selectable stack = solitaire.topPos[i].GetComponent<Selectable>();
            if (selected.GetComponent<Selectable>().value == 1) {
                if (solitaire.topPos[i].GetComponent<Selectable>().value == 0) {
                    slot1 = selected;
                    Stack(stack.gameObject);
                    break;
                }
            } else {
                if ((solitaire.topPos[i].GetComponent<Selectable>().suit == slot1.GetComponent<Selectable>().suit) && (solitaire.topPos[i].GetComponent<Selectable>().value == slot1.GetComponent<Selectable>().value - 1)) {
                    if (HasNoChildren(slot1)) {
                        slot1 = selected;
                        string lastCardname = stack.suit + stack.value.ToString();
                        if (stack.value == 1) {
                            lastCardname = stack.suit + "A";
                        }
                        if (stack.value == 11) {
                            lastCardname = stack.suit + "J";
                        }
                        if (stack.value == 12) {
                            lastCardname = stack.suit + "Q";
                        }
                        if (stack.value == 13) {
                            lastCardname = stack.suit + "K";
                        }
                        GameObject lastCard = GameObject.Find(lastCardname);
                        Stack(lastCard);
                        break;
                    }
                }
            }
        }
    }

    bool HasNoChildren(GameObject card) {
        int i = 0;
        foreach (Transform child in card.transform) {
            i++;
        }
        if (i == 0) {
            return true;
        } else {
            return false;
        }
    }
}