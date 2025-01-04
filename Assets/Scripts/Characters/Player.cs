using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    int health = 100;
    GameObject EquipedWeapon;
    Animator animator;

    // Indicateur pour savoir si le joueur est mort
    bool isDead = false;

    void Start()
    {
        // Récupération de l'Animator attaché à ce GameObject
        animator = GetComponent<Animator>();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Désactiver tous les scripts attachés à ce GameObject
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
            StartCoroutine(WaitForDeathAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        // Obtenir la durée de l'animation de mort
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Si l'animation "Die" est déjà en cours
        while (!stateInfo.IsName("Die"))
        {
            yield return null; // Attendre le début de l'animation
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Attendre la durée de l'animation
        yield return new WaitForSeconds(stateInfo.length);

        // Détruire le GameObject après l'animation
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ignorer les dégâts si le joueur est déjà mort

        health -= damage;
        Debug.Log("Player health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Awake()
    {
        if (isDead) return; // Pas d'attaque possible si le joueur est mort
        
        if (Instance == null){
            Instance = this;
        }
        else
        {

            Destroy(gameObject);

        }
    }

    // Exemple pour empêcher le déplacement
    public void Move(Vector3 direction)
    {
        if (isDead) return; // Pas de déplacement possible si le joueur est mort

        // Logique de déplacement (exemple)
        transform.Translate(direction * Time.deltaTime);
    }
}
