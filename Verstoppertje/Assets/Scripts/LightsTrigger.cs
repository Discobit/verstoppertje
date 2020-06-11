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
                Debug.Log("Player below");
                lights_top.SetActive(true);
            }
            else
            {
                Debug.Log("Player above");
                lights_down.SetActive(true);
            }
        }
        else 
        {
            if (doortrigger.transform.position.x > player.transform.position.x)
            {
                Debug.Log("Player left");
                lights_right.SetActive(true);
            }
            else
            {
                Debug.Log("Player right");
                lights_left.SetActive(true);
            }
        }

    }
}
