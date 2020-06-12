using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTrigger : MonoBehaviour
{
    public bool use_left_right;
    public GameObject doortrigger;
    public GameObject player;

    public GameObject lights_left;
    public GameObject lights_right;

    public GameObject lights_top;
    public GameObject lights_down;

    private void OnTriggerEnter(Collider other)
    {
        if (!use_left_right)
        {
            if(doortrigger.transform.position.z > player.transform.position.z)
            {
                lights_top.SetActive(true);
                lights_top.GetComponent<LightTriggered>().UpdateRoom();
            }
            else
            {
                lights_down.SetActive(true);
                lights_down.GetComponent<LightTriggered>().UpdateRoom();
            }
        }
        else 
        {
            if (doortrigger.transform.position.x > player.transform.position.x)
            {
                lights_right.SetActive(true);
                lights_right.GetComponent<LightTriggered>().UpdateRoom();
            }
            else
            {
                lights_left.SetActive(true);
                lights_left.GetComponent<LightTriggered>().UpdateRoom();
            }
        }

    }
}
