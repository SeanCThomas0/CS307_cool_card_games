using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown() {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}
