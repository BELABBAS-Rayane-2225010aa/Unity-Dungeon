using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    int health = 100;

    public GameObject drop;
    bool isDead = false;

    Animator animator;

    void Start()
    {
        // Récupération de l'Animator attaché à ce GameObject
        animator = GetComponent<Animator>();
    }
    void Die()
    {
        if (isDead) return; 

        isDead = true; 
        DropLoot();
        
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

        // Si l'animation "Die" est déjà en cours
        while (!stateInfo.IsName("Z_FallingBack"))
        {
            yield return null; // Attendre le début de l'animation
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Attendre la durée de l'animation
        yield return new WaitForSeconds(stateInfo.length);
        Destroy(gameObject);


    }
    public void TakeDamage(int damage)
    {
        health -= damage;
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
