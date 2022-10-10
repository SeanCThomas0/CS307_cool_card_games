using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                Debug.Log("pos: " + mousePos2D.x + ", " + mousePos2D.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                Debug.Log("hit: " + hit);
                if (hit.collider) {
                    Debug.Log("Collided: " + hit.collider.gameObject.GetComponent<Card>().numValue + ", " + hit.collider.gameObject.GetComponent<Card>().suitValue);
                    // hit.collider.attachedRigidbody.AddForce(Vector2.up);
                }
            }
        }
    }

    public void OnMouseDown() {
        Card dets = this.GetComponent<Card>();
        Debug.Log("CLICK A DOODLE DOO " + dets.numValue + ", " + dets.suitValue);
    }
}
