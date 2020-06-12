using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HideAndSeekGame : MonoBehaviour
{
    GameObject global;
    string playername;
    public List<GameObject> lighting;

    // UI's
    public GameObject hiderDone;
    public GameObject hiderPlaying;
    public GameObject seekerPlaying;
    public GameObject seekerDone;
    public GameObject endUI;
    public GameObject endBTN;
    public TextMeshProUGUI seekerResponseText;

    public TextMeshProUGUI seekerDoneText;

    public int hiderRoom;
    public List<int> runRooms;
    public int lastRoom;

    bool seekerStarted = false;

    public GameObject player;

    // Start is called before the first frame update
    void OnLevelWasLoaded()
    {
        //StartHider();
        //StartSeeker();
        global = GameObject.Find("GlobalObject");
        playername = global.GetComponent<ClientSocketAccesser>().playerUsername;
        if(playername == global.GetComponent<ClientSocketAccesser>().hider)
        {
            StartHider();
        }
        else if (playername == global.GetComponent<ClientSocketAccesser>().seeker)
        {
            player.GetComponent<PlayerCharacter>().enabled = false;
            player.GetComponent<LightMovement>().enabled = false;
            StartSeeker();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playername == global.GetComponent<ClientSocketAccesser>().seeker)
        {
            if (global.GetComponent<ClientSocketAccesser>().isHidden)
            {
                if(seekerStarted == false)
                {
                    StartSeeker();
                    seekerStarted = true;
                }
            }
        }
    }

    void StartHider()
    {
        hiderPlaying.SetActive(true);
        foreach (GameObject light in lighting)
        {
            if(light.name == "Roomlight 2")
            {
                light.SetActive(true);
            }
            else
            {
                light.SetActive(false);
            }
        }
    }

    void StartSeeker()
    {
        
        if (global.GetComponent<ClientSocketAccesser>().isHidden)
        {
            seekerPlaying.SetActive(true);

            StartCoroutine(FlashRooms(runRooms));
        }
    }

    IEnumerator FlashRooms(List<int> rooms)
    {
        foreach (GameObject light in lighting)
        {
            foreach (int num in runRooms)
            {
                string strNum = string.Empty;
                for (int i = 0; i < light.name.Length; i++)
                {
                    if (char.IsDigit(light.name[i]))
                    {
                        strNum += light.name[i];
                    }
                }
                if (strNum.Length > 0 && strNum == num.ToString())
                {
                    light.SetActive(true);
                }
            }
        }
        yield return new WaitForSeconds(4);
        foreach(GameObject light in lighting)
        {
            light.SetActive(false);
        }
        player.GetComponent<PlayerCharacter>().enabled = true;
        player.GetComponent<LightMovement>().enabled = true;
        yield return null;

    }

    public void Hidden()
    {
        global.GetComponent<ClientSocketAccesser>().isHidden = true;
        hiderPlaying.SetActive(false);
        hiderDone.SetActive(true);
        global.GetComponent<ClientSocketAccesser>().HiderHidden(lastRoom);
    }

    public void FindGuess()
    {
        if (lastRoom == hiderRoom)
        {
            // Hider found
            seekerDoneText.text = "Je hebt de speler gevonden!";
            global.GetComponent<ClientSocketAccesser>().SeekerDone("won");
        }
        else
        {
            // Hider not found
            seekerDoneText.text = "Je hebt de speler niet gevonden!";
            global.GetComponent<ClientSocketAccesser>().SeekerDone("lost");
        }

        seekerPlaying.SetActive(false);
        seekerDone.SetActive(true);
    }

    public void HiderEndScene(string seekerResponse)
    {
        
        if(seekerResponse == "won")
        {
            seekerResponseText.text = "Zoeker heeft je gevonden.";
        }
        else
        {
            seekerResponseText.text = "Zoeker heeft je niet gevonden!";
        }
        //GameObject endBtn = GameObject.Find("Endbutton");
        endBTN.SetActive(true);
    }

    public void EndScene()
    {
        if(playername == global.GetComponent<ClientSocketAccesser>().hider)
        {
            hiderDone.SetActive(false);
            endUI.SetActive(true);
            global.GetComponent<ClientSocketAccesser>().Logout();
        }
        else if(playername == global.GetComponent<ClientSocketAccesser>().seeker)
        {
            StartCoroutine(EndSceneWait());
        }
    }

    IEnumerator EndSceneWait()
    {
        yield return new WaitForSeconds(5);
        hiderDone.SetActive(false);
        endUI.SetActive(true);
    }

    public void StopDemo()
    {
        Application.Quit();
    }
}
