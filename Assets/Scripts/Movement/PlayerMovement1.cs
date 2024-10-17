using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float WalkingSpeed = 10f;
    public float TurningSpeed = 10f;

    public float jumpspeed = 8f;

    public float gravity = 20f;

    private Vector3 moveD = Vector3.zero;
    CharacterController Cac;

    void Start()
    {
        Cac = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cac.isGrounded)
        {
            moveD = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveD = transform.TransformDirection(moveD);
            moveD *= WalkingSpeed;

            if (Input.GetButton("Jump"))
            {
                moveD.y = jumpspeed;
            }
        }

        moveD.y -= gravity * Time.deltaTime;
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * TurningSpeed * 10);

        Cac.Move(moveD * Time.deltaTime);
    }
}