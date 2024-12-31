using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOrbital : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float RotationSpeed = 720f;

    public float Gravity = 20f;
    public Transform mainCamera; // Référence à la caméra orbitale

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            // Obtenir l'entrée du joueur
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Mouvement relatif à la caméra orbitale
            Vector3 forward = mainCamera.forward;
            Vector3 right = mainCamera.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * vertical + right * horizontal).normalized * MoveSpeed;
        }

        // Appliquer la gravité
        moveDirection.y -= Gravity * Time.deltaTime;

        // Appliquer le mouvement
        characterController.Move(moveDirection * Time.deltaTime);

        // Tourner le joueur vers la direction du mouvement
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }
}
