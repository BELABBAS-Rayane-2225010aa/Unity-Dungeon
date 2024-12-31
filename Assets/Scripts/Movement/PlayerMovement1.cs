using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float WalkingSpeed = 10f;
    public float JumpSpeed = 8f;
    public float Gravity = 20f;
    public float MouseSensitivity = 100f;

    private Vector3 moveD = Vector3.zero;
    private CharacterController characterController;
    private float mouseX;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'écran
    }

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

        // Rotation avec la souris
        mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Appliquer le mouvement
        characterController.Move(moveD * Time.deltaTime);
    }
}
