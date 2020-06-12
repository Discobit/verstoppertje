using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTriggered : MonoBehaviour
{
    GameObject global;
    public int roomnum;

    void Start()
    {
        global = GameObject.Find("GlobalObject");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (!global.GetComponent<ClientSocketAccesser>().openedRooms.Contains(roomnum))
            {
                global.GetComponent<ClientSocketAccesser>().EnterRoom(roomnum);
                global.GetComponent<ClientSocketAccesser>().openedRooms.Add(roomnum);
            }
        }
    }

    public void UpdateRoom()
    {
        GameObject game = GameObject.Find("Maingame");
        game.GetComponent<HideAndSeekGame>().lastRoom = roomnum;
    }
}
