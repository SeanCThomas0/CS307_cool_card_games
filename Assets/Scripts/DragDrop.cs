using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging) {
            Debug.Log(isDragging);
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void StartDrag() 
    {
        Debug.Log("clicked");
        startPosition = transform.position;
        isDragging = true;
    }

    public void EndDrag() 
    {
        isDragging = false;
    }
}
