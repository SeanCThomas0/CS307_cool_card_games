using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContactDeveloperManager : MonoBehaviour
{
    private UserPreferences.backgroundColor backgroundColor;
    public GameObject mainCam;

    void OnEnable()
    {
        backgroundColor = (UserPreferences.backgroundColor)PlayerPrefs.GetInt("backgroundColor");

        switch (backgroundColor)
        {
            case UserPreferences.backgroundColor.GREEN:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(49, 121, 58, 255);
                break;
            case UserPreferences.backgroundColor.BLUE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(43, 100, 159, 255);
                break;
            case UserPreferences.backgroundColor.RED:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(222, 50, 73, 255);
                break;
            case UserPreferences.backgroundColor.ORANGE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(226, 119, 28, 255);
                break;
            case UserPreferences.backgroundColor.PURPLE:
                mainCam.GetComponent<Camera>().backgroundColor = new Color32(120, 37, 217, 255);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField messageInput;
    public TMP_Dropdown subjectDropdown;
    public GameObject sentText;


    //got help with the code found on https://www.mrventures.net/all-tutorials/sending-emails
    public void sendEmail()
    {
        //get login values from Firebase Auth
        //GameObject auth = GameObject.Find("AuthController");
        //AuthController authController = auth.GetComponent<AuthController>();
        string username = AuthController.username;
        string email = AuthController.userEmail;
        string userId = AuthController.userId;
        string currentSubject = subjectDropdown.options[subjectDropdown.value].text;


        Debug.Log(email);
        //Create mail with basic info
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("CoolCardGames11@outlook.com");
        mail.To.Add("CoolCardGames11@outlook.com");
        mail.Subject = "Subject: " + currentSubject + " User: " + username;
        mail.Body = messageInput.text;
        mail.Body += "\n\nFrom " + firstNameInput.text + " " + lastNameInput.text;
        mail.Body += "\nUserID: " + userId;
        mail.Body += "\nEmail: " + email;

        //Setup server with outlook
        SmtpClient smtpServer = new SmtpClient("smtp.office365.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential("CoolCardGames11@outlook.com", "Unityisapain307");
        smtpServer.EnableSsl = true;

        //Send mail to server
        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception e)
        {
            Debug.Log("Email error: " + e.Message);
        }
        finally
        {
            Debug.Log("Email sent!");
        }

        //reset input fields
        firstNameInput.text = "";
        lastNameInput.text = "";
        messageInput.text = "";
        subjectDropdown.value = 0;
        sentText.SetActive(true);

    }

    //reset all the fields to their default state
    public void resetFields()
    {
        firstNameInput.text = "";
        lastNameInput.text = "";
        messageInput.text = "";
        subjectDropdown.value = 0;
    }

    //load main menu scene
    public void backButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
