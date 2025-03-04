using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerBehavior : MonoBehaviour
{
    GameObject EquipedWeapon;

    bool asKey = false;

    public float AttackRange = 1.5f;

    public float MoveSpeed = 5f;
    public float RotationSpeed = 720f;
    public float Gravity = 20f;
    public float JumpForce = 8f; // Force de saut
    public float MouseSensitivity = 250f;

    public Transform mainCamera; // Référence à la caméra orbitale
    public Animator animator; // Référence à l'Animator pour gérer les animations
    public Image keyImage;  // Référence à l'image de la clé
    public AudioSource audioSource; // Référence au composant AudioSource
    public AudioClip[] sounds; // Liste des sons
    public GameObject footstepParticlePrefab; // Le prefab des particules
    public Transform leftFoot; // Transform du pied gauche
    public Transform rightFoot; // Transform du pied droit

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private float mouseX;

    private float footstepTimer = 0f;
    private float footstepInterval = 0.5f; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
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
                    TriggerJumpAnimation(); // Lancer l'animation de saut
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
                    TriggerJumpAnimation(); // Lancer l'animation de saut
                    // animator.SetTrigger("unJump");
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

    private void TriggerJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Jump");
            animator.SetTrigger("endJump");
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
        if (sounds.Length > 0 && audioSource != null)
        {
            audioSource.volume = 0.2f;
            audioSource.PlayOneShot(sounds[1]);
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        EquipedWeapon.GetComponent<Weapon>().Attack(target);
        animator.SetTrigger("unWeapon");
    }

    private IEnumerator AttackWithFists(GameObject target)
    {
        animator.SetTrigger("Fist");
        if (sounds.Length > 0 && audioSource != null)
        {
            audioSource.volume = 0.01f;
            audioSource.PlayOneShot(sounds[3]);
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        target.GetComponent<Zombie>().TakeDamage(5);
        animator.SetTrigger("unFist");
    }

    public void EquipWeapon(GameObject weapon)
    {
        if (EquipedWeapon != null)
        {
            Destroy(EquipedWeapon); 
        }

        // Equipe la nouvelle arme
        EquipedWeapon = Instantiate(weapon, transform.position, transform.rotation);
        
        // Trouver la main droite du personnage (assurez-vous que le nom de l'os correspond)
        Transform rightHand = transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
        
        if (rightHand != null)
        {
            if (sounds.Length > 0 && audioSource != null)
            {
                audioSource.volume = 0.2f;
                audioSource.PlayOneShot(sounds[2]);
            }
            // Attacher l'arme à la main droite
            EquipedWeapon.transform.SetParent(rightHand);
            
            // Ajuster la position et la rotation de l'arme
            EquipedWeapon.transform.localPosition = new Vector3 (-0.4f, 0.2f, 0); // Position relative à la main
            EquipedWeapon.transform.localRotation = Quaternion.Euler(90, 0, 90); // Rotation de l'arme
        }
        else
        {
            Debug.LogError("Right Hand not found in the model.");
        }
    }

    public void PlayFootstepSound()
    {
        if (sounds.Length > 0 && audioSource != null)
        {
            audioSource.volume = 0.2f;
            audioSource.PlayOneShot(sounds[0]);
        }
    }
    public void PlayFootstepParticles()
    {
        if (footstepParticlePrefab != null)
        {
            // Instancier les particules à la position des pieds
            Instantiate(footstepParticlePrefab, leftFoot.position, Quaternion.identity);
            Instantiate(footstepParticlePrefab, rightFoot.position, Quaternion.identity);
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
    // Jouer le son de pas si le joueur est en mouvement
        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                PlayFootstepParticles();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // Réinitialiser le timer si le joueur s'arrête
        }
    }

    public void SetHasKey(bool value)
    {
        asKey = value;

        if (asKey)
        {
            keyImage.enabled = true;

        }
        else
        {
            keyImage.enabled = false;
        }
    }

    public bool HasKey()
    {
        return asKey;
    }
}
