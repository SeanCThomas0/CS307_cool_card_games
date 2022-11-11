using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase.Auth;
using Firebase.Storage;

public class UserProfile : MonoBehaviour
{
    public GameObject picture;
    RawImage profilePic;
    string errorPicUrl = "https://firebasestorage.googleapis.com/v0/b/cool-card-games.appspot.com/o/profile_icons%2FNuvola_apps_error.svg.png?alt=media&token=1a6dcef1-3e45-4290-b0d5-7059f3b1a9aa";
    FirebaseUser user;



    // Start is called before the first frame update
    void Start()
    {
        profilePic = picture.GetComponent<RawImage>();
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        string profilePicUrl = errorPicUrl;
        if (user.PhotoUrl != null)
        {
            profilePicUrl = user.PhotoUrl.ToString();
        }
        StartCoroutine(LoadImage(profilePicUrl));
    }

    private IEnumerator LoadImage(string profilePicUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(profilePicUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            profilePic.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
