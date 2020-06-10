using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTrigger : MonoBehaviour
{
    public bool use_X;
    public GameObject doortrigger;
    public GameObject player;

    public GameObject lights_1;
    public GameObject lights_2;

    private void OnTriggerEnter(Collider other)
    {
        if (!use_X)
        {
            if(doortrigger.transform.position.z > player.transform.position.z)
            {
                Debug.Log("Player below");
                lights_1.SetActive(true);
            }
            else
            {
                Debug.Log("Player above");
                lights_2.SetActive(true);
            }
        }
        else 
        {
            if (doortrigger.transform.position.x > player.transform.position.x)
            {
                Debug.Log("Player left");
                lights_1.SetActive(true);
            }
            else
            {
                Debug.Log("Player right");
                lights_2.SetActive(true);
            }
        }

    }
}
