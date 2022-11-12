using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using Firebase.Auth;
using System;

public class ImageUpdater : MonoBehaviour
{
    RawImage rawImage;
    FirebaseStorage storage;
    StorageReference storageReference;

    private FirebaseAuth auth;

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        rawImage = gameObject.GetComponent<RawImage>();
        // StartCoroutine(LoadImage("https://firebasestorage.googleapis.com/v0/b/cool-card-games.appspot.com/o/profile_icons%2FAmogus.png?alt=media&token=129832ef-d72a-4a5f-8d2b-c6ba13571d02"));

        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://cool-card-games.appspot.com/");

        StorageReference image = storageReference.Child("users/").Child(auth.CurrentUser.UserId).Child("custom_card_back.png");

        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result)));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        { //deprecated
            Debug.Log(request.error);
        }
        else
        {
            rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
