using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMovement : MonoBehaviour
{
    public GameObject rb;
    public Camera cam;
    Vector2 mousePos;
    public float offset;
    //public float sensitivity;
    Vector3 lookDir;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            lookDir = hit.point - rb.transform.position;
            Debug.DrawLine(rb.transform.position, hit.point);
        }
    }

    private void FixedUpdate()
    {
        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
        //Debug.Log(angle);
        rb.transform.eulerAngles = new Vector3(0, angle, 0);
    }
}
