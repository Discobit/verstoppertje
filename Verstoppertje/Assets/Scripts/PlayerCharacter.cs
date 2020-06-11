using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Vector3 lastMoveDir;

    private void Awake()
    {
        
    }

    private void Update()
    {
        HandleMovement();
        //rotation
        //transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void HandleMovement()
    {
        float speed = 3f;
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveZ = +1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveZ = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
        }

        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
        Vector3 targetMovePos = transform.position + moveDir * speed * Time.deltaTime;
        RaycastHit raycastHit;

        if(Physics.Raycast(transform.position, moveDir, out raycastHit, speed * Time.deltaTime))
        {
            if (raycastHit.collider.gameObject.name.Contains("DoorTrigger"))
            {
                transform.position = targetMovePos;
            }

            Vector3 testMoveDir = new Vector3(moveDir.x, 0f).normalized;
            Vector3 targetMovePos_2 = transform.position + testMoveDir * speed * Time.deltaTime;
            bool raycastHit_2 = Physics.Raycast(transform.position, testMoveDir, speed * Time.deltaTime);

            if (!raycastHit_2)
            {
                // Can move horizontal
                transform.position = targetMovePos_2;
            }
            else
            {
                // Can't move horizontal
                Vector3 testMoveDir_2 = new Vector3(0f, 0f, moveDir.z).normalized;
                Vector3 targetMovePos_3 = transform.position + testMoveDir_2 * speed * Time.deltaTime;
                bool raycastHit_3 = Physics.Raycast(transform.position, testMoveDir_2, speed * Time.deltaTime);

                if (!raycastHit_3)
                {
                    // Can move vertically
                    transform.position = targetMovePos_3;
                }

            }

        }
        else
        {
            // Player can move. No hits
            transform.position = targetMovePos;
        }
    }
}
