using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClientSocketAccesser : MonoBehaviour
{
    public static ClientSocketAccesser Instance;

    // Global app variables
    public string playerUsername = "!playername";
    public string player2Username = "!player2name";
    string playerPassword;
    public int globalGameId;
    public GameObject game;

    // Global game variables
    public string hider = "";
    public string seeker = "";
    public bool isHidden = false;

    public List<int> openedRooms = new List<int>();

    public void SetPlayerUsername(string username){ playerUsername = username; }
    public void SetPlayer2Username(string username){ player2Username = username; }
    public void SetPlayerPassword(string password){ playerPassword = password; }
    public void SetGlobalGameID(int gameID){ globalGameId = gameID; }

    public GameObject mainMenu;
    public GameObject loginMenu;
    public GameObject gameMenu;
    public GameObject registermenu;
    public GameObject waitQueue;

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

    private void FixedUpdate()
    {
        try
        {
            List<string> queue = ISocket.GetResponseQueue();
            if (queue.Count > 0)
            {
                for (int i = 0; i < queue.Count; i++)
                {
                    CallbackHandler(queue[i]);
                    ISocket.RemoveFromResponseQueue(queue[i]);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void WaitForSeconds(int seconds)
    {
        DateTime lasttime = DateTime.Now;
        double secPast = (DateTime.Now - lasttime).TotalSeconds;
        int secondsPast = Int32.Parse(secPast.ToString());

        while (secondsPast < seconds)
        {
            secPast = (DateTime.Now - lasttime).TotalSeconds;
            string switchVar = secPast.ToString();
            secondsPast = Int32.Parse(switchVar);
        }
    }

    public void Startup()
    {
        string serverIPaddress;
        int portnumber;
        InputField serverip = GameObject.Find("serveripinput").GetComponent<InputField>();
        InputField serverport = GameObject.Find("serverportinput").GetComponent<InputField>();

        if(serverip.text == "")
        {
            serverIPaddress = "86.94.191.44";
        }
        else
        {
            serverIPaddress = serverip.text;
        }

        if(serverport.text == "")
        {
            portnumber = 34000;
        }
        else
        {
            try
            {
                portnumber = Int32.Parse(serverport.text);
            }catch(Exception e)
            {
                portnumber = 34000;
            }
        }

        ISocket.StartSocket(serverIPaddress, portnumber);
        ISocket.testMessage();
    }

    public void Login()
    {
        InputField usernameInput = GameObject.Find("UsernameLoginInput").GetComponent<InputField>();
        InputField passwordInput = GameObject.Find("PasswordLoginInput").GetComponent<InputField>();
        string username = usernameInput.text;
        string password = passwordInput.text;

        Startup();

        ISocket.login(username, password);

        playerUsername = username;
        playerPassword = password;
    }

    public void Register()
    {
        InputField usernameInput = GameObject.Find("UsernameRegisterInput").GetComponent<InputField>();
        InputField passwordInput = GameObject.Find("PasswordRegisterInput").GetComponent<InputField>();
        string username = usernameInput.text;
        string password = passwordInput.text;
        //ISocket.register(username, password);
    }

    public void CreateNewGame()
    {
        ISocket.gamecreate(playerUsername, playerPassword);

    }

    public void GameAccess()
    {
        InputField joinInput = GameObject.Find("joinInput").GetComponent<InputField>();
        string username = joinInput.text;
        ISocket.gameaccess(username, playerUsername);
        player2Username = username;
    }

    public void Logout()
    {
        ISocket.logout(playerUsername);
    }

    public void ChangePlayerType()
    {
        ISocket.changeplayertype(globalGameId);
    }

    public void HiderHidden(int lastroomnum)
    {
        ISocket.hiderhidden(globalGameId,lastroomnum);
    }

    public void EnterRoom(int roomnum)
    {
        ISocket.roomenter(globalGameId, roomnum);
    }

    public void SeekerDone(string answer)
    {
        ISocket.seekerdone(globalGameId, answer);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        ISocket.gameaccess(player2Username, playerUsername, "gamestart");

    }

    void CallbackHandler(string call)
    {
        JObject json = JObject.Parse(call);

        switch (json["connectionType"].ToString())
        {
            case "loginResponse":
                if (json["loginStatus"].ToString() == "success")
                {
                    mainMenu.SetActive(true);
                    loginMenu.SetActive(false);
                }
                break;
            case "connectionTest":
                Debug.Log(call);
                break;
            case "logoutResponse":
                if (json["logoutStatus"].ToString() == "success")
                {
                    gameMenu.SetActive(false);
                    mainMenu.SetActive(false);
                    registermenu.SetActive(false);
                    loginMenu.SetActive(true);
                }
                break;
            case "registerResponse": // WE ZIEN WEL
                break;
            case "createResponse":
                globalGameId = Int32.Parse(json["roomId"].ToString());
                mainMenu.SetActive(false);
                gameMenu.SetActive(true);
                hider = playerUsername;
                seeker = player2Username;
                break;
            case "gameaccessResponse":
                if(json["gameaccessStatus"].ToString() == "success")
                {
                    string playertype = json["playerType"].ToString();
                    globalGameId = Int32.Parse(json["gameid"].ToString());
                    if(playertype == "seeker")
                    {
                        seeker = playerUsername;
                        hider = player2Username;

                    }else if(playertype == "hider")
                    {
                        hider = playerUsername;
                        seeker = player2Username;
                    }
                    mainMenu.SetActive(false);
                    waitQueue.SetActive(true);
                }
                else if (json["gameaccessStatus"].ToString() == "newPlayer")
                {
                    player2Username = json["playerName"].ToString();
                    if(playerUsername == hider)
                    {
                        seeker = player2Username;
                    }
                    else
                    {
                        hider = player2Username;
                    }
                }
                else if (json["gameaccessStatus"].ToString() == "gamestart")
                {
                    SceneManager.LoadScene(1);
                }
                break;
            case "hiddenResponse":
                string[] stringRooms = json["runRooms"].ToString().Split(',');
                List<int> tempRooms = new List<int>();
                foreach(string room in stringRooms)
                {
                    tempRooms.Add(Int32.Parse(room));
                }
                game = GameObject.Find("Maingame");
                game.GetComponent<HideAndSeekGame>().runRooms = tempRooms;
                game.GetComponent<HideAndSeekGame>().hiderRoom = Int32.Parse(json["lastRoom"].ToString());
                isHidden = true;
                break;
            case "seekerResponse":
                game = GameObject.Find("Maingame");
                if (json["playerType"].ToString() == "hider")
                {
                    string seekerAnswer = json["seekerAnswer"].ToString();
                    game.GetComponent<HideAndSeekGame>().HiderEndScene(seekerAnswer);
                }
                else if (json["playerType"].ToString() == "seeker")
                {
                    game.GetComponent<HideAndSeekGame>().EndScene();
                }
                
                break;
            case "changePlayerResponse":
                if(json["changePlayerStatus"].ToString() == "changed")
                {
                    string helpvar = hider;
                    hider = seeker;
                    seeker = helpvar;
                }
                break;
        }


    }

    public void ExitProgram()
    {
        Application.Quit();
    }



}
