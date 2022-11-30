using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
//using SimpleFileBrowser;

using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using Firebase.Auth;
using System;

public class UploadImage : MonoBehaviour
{
    public RawImage rawImage;

    FirebaseStorage storage;
    StorageReference storageReference;
    private FirebaseAuth auth;

    public GameObject uploadOption;

    /*
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

    public void UploadClicked()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                Debug.Log(FileBrowser.Result[i]);
            }

            StorageReference uploadRef = storageReference.Child("users/").Child(auth.CurrentUser.UserId).Child("/custom_card_back.png");

            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            var newMetadata = new MetadataChange();
            newMetadata.ContentType = "image/png";

            uploadRef.PutBytesAsync(bytes, newMetadata).ContinueWithOnMainThread((task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                }
                else
                {
                    Debug.Log("File upload successful.");

                    StorageReference image = storageReference.Child("users/").Child(auth.CurrentUser.UserId).Child("custom_card_back.png");

                    image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
                    {
                        if (!task.IsFaulted && !task.IsCanceled)
                        {
                            StartCoroutine(RefreshImage(Convert.ToString(task.Result)));
                        }
                        else
                        {
                            Debug.Log(task.Exception);
                        }
                    });
                }
            });
        }
    }

    IEnumerator RefreshImage(string url)
    {
        uploadOption.SetActive(true);
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
    */
}
