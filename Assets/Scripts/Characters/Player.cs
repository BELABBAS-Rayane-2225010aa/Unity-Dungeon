using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    int health = 300;
    private float nextLevelDistance = 6f;
    Animator animator;

    // Indicateur pour savoir si le joueur est mort
    private bool isDead = false;

    public float getNextLevelDistance()
    {
        return nextLevelDistance;
    }

    public void setNextLevelDistance(float distance)
    {
        nextLevelDistance = distance;
    }

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

        // Charge la scène spécifiée
        SceneManager.LoadScene("EndScene");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ignorer les dégâts si le joueur est déjà mort

        health -= damage;
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

    public int GetHealth()
    {
        return health; // Retourne la santé actuelle du joueur
    }

    public bool getIsDead()
    {
        return isDead;
    }
}
