using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    int health = 100;

    public GameObject drop;

    Animator animator;

    void Start()
    {
        // Récupération de l'Animator attaché à ce GameObject
        animator = GetComponent<Animator>();
    }

    void Die()
    {
        // Désactiver tous les scripts attachés à ce GameObject
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        if (animator != null)
        {
            animator.SetTrigger("Death");
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        // Obtenir la durée de l'animation de mort
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Si l'animation "Death" est déjà en cours
        while (!stateInfo.IsName("Death"))
        {
            yield return null; // Attendre le début de l'animation
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Attendre la durée de l'animation
        yield return new WaitForSeconds(stateInfo.length);

        DropLoot();
        Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Zombie health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void DropLoot()
    {
        // drop 1 drop when the zombie dies
        GameObject droppedLoot = Instantiate(drop, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), drop.transform.rotation);
        droppedLoot.name = "Drop";
    }
}
