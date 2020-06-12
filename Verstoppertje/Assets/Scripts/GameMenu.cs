using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI playername = GameObject.Find("Playername").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI verstoppername = GameObject.Find("HiderName").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI gameID = GameObject.Find("GameID").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI zoekername = GameObject.Find("SeekerName").GetComponent<TextMeshProUGUI>();

        GameObject global = GameObject.Find("GlobalObject");
        playername.text = global.GetComponent<ClientSocketAccesser>().playerUsername;
        verstoppername.text = global.GetComponent<ClientSocketAccesser>().hider;
        gameID.text = global.GetComponent<ClientSocketAccesser>().globalGameId.ToString();
        zoekername.text = global.GetComponent<ClientSocketAccesser>().seeker;
    }
}
