using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using SimpleFileBrowser;

using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using Firebase.Auth;
using System;

public class ChangeIconMenu : MonoBehaviour
{
    public RawImage rawImage;

    FirebaseStorage storage;
    StorageReference storageReference;
    private FirebaseAuth auth;

    public GameObject uploadOption;


    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));

        FileBrowser.SetDefaultFilter(".png");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://cool-card-games.appspot.com/");
    }


    /*
     * Function: SaveButton
     * 
     * 
     * 
     */
    void SaveButton()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
