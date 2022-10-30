using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmoteManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [SerializeField]
    private GameObject emoteInterface;
    [SerializeField]
    private GameObject currentEmote;

    public void setActiveEmoteInterface() {
        if(emoteInterface.activeSelf) {
            emoteInterface.SetActive(false);
        }
        else {
            emoteInterface.SetActive(true);
        }
    }

    public void sendEmote(GameObject emoteButton) {
        emoteInterface.SetActive(false);
        currentEmote.SetActive(true);
        currentEmote.GetComponent<Image>().sprite = emoteButton.GetComponent<Image>().sprite;
        currentEmote.GetComponent<RectTransform>().sizeDelta = new Vector2(emoteButton.GetComponent<RectTransform>().rect.width, emoteButton.GetComponent<RectTransform>().rect.height);
        Invoke("passTime", 2);
        
    }

    public void passTime() {
        Debug.Log("Passing 2 seconds of time");
        currentEmote.SetActive(false);
    }
}
