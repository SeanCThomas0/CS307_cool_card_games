using System;


[Serializable]
public class Player 
{
    public string Username;
    public string Password;

    public Player() { }

    public Player(string name, string pass)
    {
        Username = name;
        Password = pass;
    }
}
