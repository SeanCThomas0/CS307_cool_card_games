using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ResolutionManager : MonoBehaviour
{    
    public TMP_Dropdown resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(1920, 1080, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //sets the current resolution of the screen to one of the dropdown options
    public void setResolution() {
        //get current string value of resolution dropdown
        string currentResolution = resolutionDropdown.options[resolutionDropdown.value].text;
        //split string into width and height
        string[] resolutionArray = currentResolution.Split('x');
        Screen.SetResolution(Int32.Parse(resolutionArray[0]), Int32.Parse(resolutionArray[1]), Screen.fullScreen);
    }

    //toggles the fullscreen setting of the screen
    public void toggleFullscreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }

}
