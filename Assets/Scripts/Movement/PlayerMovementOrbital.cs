using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOrbital : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float RotationSpeed = 720f;
    public float Gravity = 20f;
    public float JumpForce = 8f; // Force de saut

    public Transform mainCamera; // Référence à la caméra orbitale
    public Animator animator; // Référence à l'Animator pour gérer les animations

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        HandleMovement();
        UpdateAnimation();
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

            // Vérifier si le joueur appuie sur la touche de saut
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = JumpForce;
            }
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

    private void UpdateAnimation()
    {
        // Vérifier si le joueur est en mouvement (horizontal ou vertical)
        bool isMoving = moveDirection.x != 0 || moveDirection.z != 0;

        // Appliquer l'animation Idle ou de mouvement
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }
}
