using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    // the zombie wants to move at the player only if the player is in this range
    private float MoveRange = 5f;

    private float AttackRange = 1.5f;

    public float MoveSpeed = 5f;
    public float RotationSpeed = 720f;
    public float Gravity = 20f;

    //public Animator animator;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private GameObject player;
    Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        player = GameObject.FindWithTag("Player");
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            // si le joueur est dans la range de déplacement
            if (Vector3.Distance(transform.position, player.transform.position) <= MoveRange)
            {
                // Mouvement relatif au joueur
                Vector3 forward = (player.transform.position - transform.position).normalized;

                forward.y = 0f;
                forward.Normalize();

                moveDirection = forward * MoveSpeed;
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }

        // Appliquer la gravité
        moveDirection.y -= Gravity * Time.deltaTime;

        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            // Appliquer le mouvement
            characterController.Move(moveDirection * Time.deltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private void HandleAttack()
    {
        // si le joueur est dans la range d'attaque et attendre un peu avant d'attaquer
        if (!isAttacking && Vector3.Distance(transform.position, player.transform.position) <= AttackRange)
        {
            StartCoroutine(AttackAfterDelay());
        }
    }

    IEnumerator AttackAfterDelay()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        player.GetComponent<Player>().TakeDamage(10); // attaquer le joueur
        animator.SetTrigger("unAttack");
        isAttacking = false;
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