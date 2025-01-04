using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    GameObject EquipedWeapon;

    private float AttackRange = 1.5f;

    public float MoveSpeed = 5f;
    public float RotationSpeed = 720f;
    public float Gravity = 20f;
    public float JumpForce = 8f; // Force de saut
    public float MouseSensitivity = 250f;

    public Transform mainCamera; // Référence à la caméra orbitale
    public Animator animator; // Référence à l'Animator pour gérer les animations

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private float mouseX;

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

        // si la scene n'est pas EntryScene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "EntryScene")
        {
            Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'écran
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
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "EntryScene")
        {
            if (characterController.isGrounded)
            {
                // Déplacement avant/arrière (Z)
                moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

                // Déplacement latéral (strafe) avec Q et D
                moveDirection.x = Input.GetAxis("Horizontal");

                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= MoveSpeed;

                // Saut
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = JumpForce;
                }
            }

            // Gravité
            moveDirection.y -= Gravity * Time.deltaTime;

            // Rotation avec la souris
            mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);

            // Appliquer le mouvement
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else{
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
    }

    // si le joueur appuis sur la touche d'attaque et s'il est dans la range avec un ou plusieurs zombies
    private void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche de la souris
        {
            Zombie[] zombies = FindObjectsOfType<Zombie>();
            foreach (Zombie zombie in zombies)
            {
                if (Vector3.Distance(transform.position, zombie.transform.position) <= AttackRange)
                {
                    Attack(zombie.gameObject);
                }
            }
        }
    }

    public void Attack(GameObject target)
    {
        if (EquipedWeapon != null)
        {
            // attendre la fin de l'animation d'attaque
            StartCoroutine(AttackWeapon(target));
        }
        else
        {
            // attendre la fin de l'animation d'attaque
            StartCoroutine(AttackWithFists(target));
        }
    }

    private IEnumerator AttackWeapon(GameObject target)
    {
        animator.SetTrigger("Weapon");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        EquipedWeapon.GetComponent<Weapon>().Attack(target);
    }

    private IEnumerator AttackWithFists(GameObject target)
    {
        animator.SetTrigger("Fist");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        target.GetComponent<Zombie>().TakeDamage(10);
    }

    public void EquipWeapon(GameObject weapon)
    {
        EquipedWeapon = weapon;
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
