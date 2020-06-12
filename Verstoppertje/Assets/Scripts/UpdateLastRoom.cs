using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateLastRoom : MonoBehaviour
{
    public GameObject game;
    public TextMeshProUGUI roomcount;

    // Update is called once per frame
    void Update()
    {
        roomcount.text = game.GetComponent<HideAndSeekGame>().lastRoom.ToString();
    }
}
