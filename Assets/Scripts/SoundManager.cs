using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMPro.TextMeshProUGUI volumeSliderText = null;
    [SerializeField] public GameObject clickSound;
    [SerializeField] public AudioSource guh;


    // Start is called before the first frame update
    void Start()
    {
        LoadValues();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolumeSlider(float volume) {
        volumeSliderText.text=volume.ToString("0.0");

        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue",volumeValue);
        LoadValues();
    }
    void LoadValues() {
        float volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        volumeSlider.value = volumeValue;
        guh.volume=volumeValue;


        
    }


}
