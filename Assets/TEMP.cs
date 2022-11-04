using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP : MonoBehaviour
{
    [SerializeField] public AudioSource ClickSound;
    // Start is called before the first frame update
    void Start()
    {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        ClickSound.volume=volumeValue;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
