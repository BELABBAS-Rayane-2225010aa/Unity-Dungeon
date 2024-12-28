using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float WalkingSpeed = 10f;
    public float TurningSpeed = 10f;

    public float JumpSpeed = 8f;
    public float Gravity = 20f;

    private Vector3 moveD = Vector3.zero;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded)
        {
            // Déplacement avant/arrière (Z)
            moveD = new Vector3(0, 0, Input.GetAxis("Vertical"));

            // Déplacement latéral (strafe) avec Q et D
            moveD.x = Input.GetAxis("Horizontal");

            moveD = transform.TransformDirection(moveD);
            moveD *= WalkingSpeed;

            // Saut
            if (Input.GetButton("Jump"))
            {
                moveD.y = JumpSpeed;
            }
        }

        // Gravité
        moveD.y -= Gravity * Time.deltaTime;

        // Rotation gauche/droite avec E et A
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * TurningSpeed * Time.deltaTime*10);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.down * TurningSpeed * Time.deltaTime*10);
        }

        // Appliquer le mouvement
        characterController.Move(moveD * Time.deltaTime);
    }
}