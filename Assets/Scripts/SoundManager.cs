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
    [SerializeField] private Slider volumeSliderMusic = null;
    [SerializeField] private TMPro.TextMeshProUGUI volumeSliderMusicText = null;
    [SerializeField] public AudioSource UNOSONG1;


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
        float volumeValueMusic = PlayerPrefs.GetFloat("VolumeValueMusic");
        volumeSlider.value = volumeValue;
        volumeSliderMusic.value =volumeValueMusic;

        guh.volume=volumeValue;
        UNOSONG1.volume = volumeValueMusic /4;


        
    }

    public void VolumeSliderMusic(float volume) {
        volumeSliderMusicText.text=volume.ToString("0.0");

        float volumeValue = volumeSliderMusic.value;
        PlayerPrefs.SetFloat("VolumeValueMusic",volumeValue);
        LoadValues();
    }


}
