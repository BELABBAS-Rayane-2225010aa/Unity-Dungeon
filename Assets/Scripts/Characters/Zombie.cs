using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    int health = 100;

    public GameObject drop;

    void Die()
    {
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
