using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneVolumeSetter : MonoBehaviour
{
    [SerializeField] public AudioSource MusicSource;
    // Start is called before the first frame update
    void Start()
    {
        LoadValues();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void LoadValues() {

        float volumeValueMusic = PlayerPrefs.GetFloat("VolumeValueMusic");
        MusicSource.volume = volumeValueMusic /4;


        
    }

}
