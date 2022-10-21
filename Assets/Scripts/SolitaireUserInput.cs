using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SolitaireUserInput : MonoBehaviour
{
    public GameObject slot1;
    public GameObject wrongMove;
    
    private Solitaire solitaire;
    private float timer;
    private float doubleClickTime = 0.3f;
    private int clickCount = 0;
    public SolitaireUIButton solitaireUIButton;
    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        solitaireUIButton = FindObjectOfType<SolitaireUIButton>();
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

        if (!selected.GetComponent<Card>().showingFront) {//if the card clicked on is facedown
            //if the card clicked on is not blocked
                //flip it over
            if (!Blocked(selected)) {
                selected.GetComponent<Card>().showingFront = true;
                slot1 = this.gameObject;
            }     
        } else if (selected.GetComponent<Card>().inPool) {
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
            if (slot1.GetComponent<Card>().numValue == 1) {
                Stack(selected);
            }
        }
    }

    void Bottom(GameObject selected) {
        //bottom click actions
        print("clicked on bottom");
        if (slot1.CompareTag("Card")) {
            if (slot1.GetComponent<Card>().numValue == 13) {
                Stack(selected);
            }
        }
    }

    bool Stackable(GameObject selected) {
        Card s1 = slot1.GetComponent<Card>();
        Card s2 = selected.GetComponent<Card>();
        // compare them to see if they stack

        if (!s2.inPool) {
            if (s2.top) { // if in the top pile must stack suited Ace to King
                if (s1.suitValue == s2.suitValue || (s1.numValue == 1 && s2.suitValueString == null)) {
                    if (s1.numValue == s2.numValue + 1) {
                        wrongMove.SetActive(false);
                        return true;
                    }
                } else {
                    wrongMove.SetActive(true);
                    return false;
                }
            } else { // if in the bottom pile must stack alternate colors King to Ace
                if (s1.numValue == s2.numValue - 1) {
                    bool card1Red = true;
                    bool card2Red = true;

                    if (s1.suitValueString == "clubs" || s1.suitValueString == "spades") {
                        card1Red = false;
                    }

                    if (s2.suitValueString == "clubs" || s2.suitValueString == "spades") {
                        card2Red = false;
                    }

                    if (card1Red == card2Red) {
                        wrongMove.SetActive(true);
                        return false;
                    } else {
                        wrongMove.SetActive(false);
                        return true;
                    }
                }
            }
        }
        wrongMove.SetActive(true);
        return false;
    }

    void Stack(GameObject selected) {
        // if on top of King or empty bottom stack the cards in place
        // else stack the cards with a negative y offset
        Card s1 = slot1.GetComponent<Card>();
        Card s2 = selected.GetComponent<Card>();
        float yOffset = 0.3f;
        if (!solitaireUIButton.clicked) {
            
            

            if (s2.top || (!s2.top && s1.numValue == 13)) {
                yOffset = 0;
            }

            slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z - 0.01f);
            slot1.transform.parent = selected.transform;

            if (s1.inPool) {
                solitaire.tripsOnDisplay.Remove(slot1);
            } else if (s1.top && s2.top && s1.numValue == 1) {
                solitaire.topPos[s1.row].GetComponent<Card>().numValue = 0;
                solitaire.topPos[s1.row].GetComponent<Card>().suitValueString = null;
            } else if (s1.top) {
                solitaire.topPos[s1.row].GetComponent<Card>().numValue = s1.numValue - 1;
            } else {
                solitaire.playSpaces[s1.row].Remove(slot1);
            }

            s1.inPool = false;
            s1.row = s2.row;

            if (s2.top && s2.suitValueString == s1.suitValueString) {
                if (solitaire.topPos[0].GetComponent<Card>().suitValueString == s1.suitValueString) {
                    solitaire.topPos[0].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[0].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                } else if (solitaire.topPos[1].GetComponent<Card>().suitValueString == s1.suitValueString) {
                    solitaire.topPos[1].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[1].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                } else if (solitaire.topPos[2].GetComponent<Card>().suitValueString == s1.suitValueString) {
                    solitaire.topPos[2].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[2].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                } else if (solitaire.topPos[3].GetComponent<Card>().suitValueString == s1.suitValueString) {
                    solitaire.topPos[3].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[3].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                }
            }
            else if (s2.top) {
                if (solitaire.topPos[0].GetComponent<Card>().numValue == 0) {
                    solitaire.topPos[0].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[0].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                } else if (solitaire.topPos[1].GetComponent<Card>().numValue == 0) {
                    solitaire.topPos[1].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[1].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                } else if (solitaire.topPos[2].GetComponent<Card>().numValue == 0) {
                    solitaire.topPos[2].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[2].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                } else if (solitaire.topPos[3].GetComponent<Card>().numValue == 0) {
                    solitaire.topPos[3].GetComponent<Card>().numValue = s1.numValue;
                    solitaire.topPos[3].GetComponent<Card>().suitValueString = s1.suitValueString;
                    s1.top = true;
                }
            } else {
                s1.top = false;
            }

            slot1 = this.gameObject;
        } else {
            slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z - 0.01f);
            slot1.transform.parent = selected.transform;
            if (s1.inPool) {
                solitaire.tripsOnDisplay.Remove(slot1);
            } else if (s1.top && s2.top && s1.numValue == 1) {
                solitaire.topPos[s1.row].GetComponent<Card>().numValue = 0;
                solitaire.topPos[s1.row].GetComponent<Card>().suitValueString = null;
            } else if (s1.top) {
                solitaire.topPos[s1.row].GetComponent<Card>().numValue = s1.numValue - 1;
            } else {
                solitaire.playSpaces[s1.row].Remove(slot1);
            }
            s1.inPool = false;
            s1.row = s2.row;
            if (s2.top) {
                solitaire.topPos[0].GetComponent<Card>().numValue = 13;
            }
        }
    }

    bool Blocked(GameObject selected) {
        Card s2 = selected.GetComponent<Card>();
        if (s2.inPool == true) {
            if (s2.GetComponent<GameObject>() == solitaire.tripsOnDisplay.Last()) {
                wrongMove.SetActive(false);
                return false;
            } else {
                wrongMove.SetActive(true);
                return true;
            }
        } else {
            if (s2.GetComponent<GameObject>() == solitaire.playSpaces[s2.row].Last()) {
                wrongMove.SetActive(false);
                return false;
            } else {
                wrongMove.SetActive(true);
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
            Card stack = solitaire.topPos[i].GetComponent<Card>();
            if (selected.GetComponent<Card>().numValue == 1) {
                if (solitaire.topPos[i].GetComponent<Card>().numValue == 0) {
                    slot1 = selected;
                    Stack(stack.gameObject);
                    break;
                }
            } else {
                if ((solitaire.topPos[i].GetComponent<Card>().suitValueString == slot1.GetComponent<Card>().suitValueString) && (solitaire.topPos[i].GetComponent<Card>().numValue == slot1.GetComponent<Card>().numValue - 1)) {
                    if (HasNoChildren(slot1)) {
                        slot1 = selected;
                        string lastCardname = stack.suitValueString + stack.numValue.ToString();
                        if (stack.numValue == 1) {
                            lastCardname = stack.suitValueString + "ace";
                        }
                        if (stack.numValue == 11) {
                            lastCardname = stack.suitValueString + "jack";
                        }
                        if (stack.numValue == 12) {
                            lastCardname = stack.suitValueString + "queen";
                        }
                        if (stack.numValue == 13) {
                            lastCardname = stack.suitValueString + "king";
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
            wrongMove.SetActive(false);
            return true;
        } else {
            wrongMove.SetActive(true);
            return false;
        }
    }
}
