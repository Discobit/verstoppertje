using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSocketAccesser : MonoBehaviour
{
    public static ClientSocketAccesser Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Startup()
    {
        ISocket.StartSocket("", 34000);
    }

    public void Login()
    {
        string username = "";
        string password = "";

        ISocket.login(username, password);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void Register()
    {
        string username = "";
        string password = "";
        ISocket.register(username, password);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void GameCreate()
    {
        string username = "";
        string password = "";
        ISocket.gamecreate(username, password);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void GameAccess()
    {
        string username = "";
        string password = "";
        ISocket.gameaccess(username);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void Logout()
    {
        string username = "";
        string password = "";
        ISocket.logout(username);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void ChangePlayerType()
    {
        int gameid = 0;
        ISocket.changeplayertype(gameid);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void HiderHidden()
    {
        int gameid = 0;
        ISocket.hiderhidden(gameid);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void EnterRoom()
    {
        int gameid = 0;
        int roomnum = 0;
        ISocket.roomenter(gameid, roomnum);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

    public void SeekerDone()
    {
        int gameid = 0;
        ISocket.seekerdone(gameid);
        //string response = ISocket.ResponseListener(); //Add connection responsetype
    }

}
