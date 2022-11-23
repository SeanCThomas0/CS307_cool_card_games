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
using Firebase.Database;
using System.Data;
using UnityEngine.Profiling;

public class ChangeIconMenu : MonoBehaviour
{
    RawImage rawImage;
    public GameObject rawImageButton;
    public GameObject checkDefault;
    public GameObject errorText;

    FirebaseStorage storage;
    StorageReference storageReference;
    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseReference userDR;

    private string uploadUrl;
    string curUserPhotoUrl;
    string curUserIcon = "";
    bool changeSuccess = false;




    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ChangeIconMenu Start");
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        userDR = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(user.UserId);
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://cool-card-games.appspot.com/");


        FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        rawImage = rawImageButton.GetComponent<RawImage>();

        curUserPhotoUrl = user.PhotoUrl.ToString();



        /*
        userDR.Child("customization/selected_icon").GetValueAsync().ContinueWith(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                curUserIcon = task.Result.ToString();
                Vector3 curPos = GameObject.Find(curUserIcon).transform.position;
                checkDefault.SetActive(true);
                if (curUserIcon.Equals("custom_icon"))
                {
                    checkDefault.transform.localPosition = curPos + new Vector3(50, 50, 0);
                    checkDefault.SetActive(true);
                }
                else
                {
                    checkDefault.transform.localPosition = curPos + new Vector3(35, 35, 0);
                    checkDefault.SetActive(true);
                }
                Debug.Log("Initial selected icon success");
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });*/


        StorageReference image = storageReference.Child("users/").Child(auth.CurrentUser.UserId).Child("custom_icon.png");
        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                uploadUrl = task.Result.ToString();
                StartCoroutine(RefreshImage(task.Result.ToString()));
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });

    }



    public void ChangeDesignCustomButton()
    {
        StartCoroutine(ChangeDesignCustom());
        PositionCheck(50, 50, 0);
    }

    private IEnumerator ChangeDesignCustom()
    {
        string clickedName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        Debug.Log("clicked " + clickedName);
        errorText.SetActive(false);

        StorageReference image = storageReference.Child("users").Child(user.UserId).Child(clickedName + ".png");

        yield return image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                uploadUrl = task.Result.ToString();
                StartCoroutine(RefreshImage(uploadUrl));

                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    PhotoUrl = new System.Uri(uploadUrl),
                };

                //Position check
                //checkDefault.SetActive(true);
                //Vector3 curPos = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.localPosition;
                //checkDefault.transform.localPosition = curPos + new Vector3(50, 50, 0);

                user.UpdateUserProfileAsync(profile).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        //Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                        //ErrorMessage((AuthError)e.ErrorCode);
                        return;
                    }

                    Debug.Log("User profile display name updated successfully.");

                    userDR.Child("customization/selected_icon").SetValueAsync(clickedName).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.Log("Database Icon Update Canceled");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.Log("Database Icon Update Faulted: " + task.Exception);
                            return;
                        }
                        Debug.Log("Database Icon Update Successful");
                    });
                });
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    public void ChangeDesignButton()
    {
        StartCoroutine(ChangeDesign());
        PositionCheck(35, 35, 0);
    }


    private IEnumerator ChangeDesign()
    {
        string clickedName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;



        Debug.Log("clicked " + clickedName);
        errorText.SetActive(false);

        StorageReference image = storageReference.Child("profile_icons").Child(clickedName + ".png");

        yield return image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                uploadUrl = task.Result.ToString();

                //Position check

                Debug.Log("Check Moved");

                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                {
                    PhotoUrl = new System.Uri(uploadUrl),
                };

                user.UpdateUserProfileAsync(profile).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.Log("UpdateUserProfileAsync encountered an error: " + task.Exception);
                        //Firebase.FirebaseException e = task.Exception.Flatten().InnerException as Firebase.FirebaseException;
                        //ErrorMessage((AuthError)e.ErrorCode);
                        return;
                    }

                    Debug.Log("User profile display name updated successfully.");

                    userDR.Child("customization/selected_icon").SetValueAsync(clickedName).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.Log("Database Icon Update Canceled");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.Log("Database Icon Update Faulted: " + task.Exception);
                            return;
                        }
                        Debug.Log("Database Icon Update Successful");
                    });
                });
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    private void PositionCheck(float x, float y, float z)
    {
        checkDefault.SetActive(true);
        Vector3 curPos = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.localPosition;
        checkDefault.transform.localPosition = curPos + new Vector3(x, y, z);
        checkDefault.SetActive(true);
    }

    public void UploadClicked()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                Debug.Log(FileBrowser.Result[i]);
            }

            StorageReference uploadRef = storageReference.Child("users").Child(auth.CurrentUser.UserId).Child("custom_icon.png");

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

                    StorageReference image = storageReference.Child("users").Child(auth.CurrentUser.UserId).Child("custom_icon.png");

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

    private IEnumerator RefreshImage(string url)
    {
        Debug.Log("Image Refreshing");
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
}

