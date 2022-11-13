using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContactDeveloperManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TMP_InputField usernameInput;
    public TMP_InputField messageInput;

    [SerializeField]
    private GameObject reportArea;
    
    //got help with the code found on https://www.mrventures.net/all-tutorials/sending-emails
    public void sendEmail() {
        //Create mail with basic info
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("CoolCardGames11@outlook.com");
        mail.To.Add("CoolCardGames11@outlook.com");
        mail.Subject = "Report aginst user " + usernameInput.text;
        mail.Body = messageInput.text;

        //Setup server  with outlook
        SmtpClient smtpServer = new SmtpClient("smtp.office365.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential("CoolCardGames11@outlook.com", "Unityisapain307");
        smtpServer.EnableSsl = true;       

        //Send mail to server
        try {
            smtpServer.Send(mail);
        }
        catch (System.Exception e) {
            Debug.Log("Email error: " + e.Message);
        }
        finally {
            Debug.Log("Email sent!");
        }

        //reset input fields
        usernameInput.text = "";
        messageInput.text = "";
    }
}
