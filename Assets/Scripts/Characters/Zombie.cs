using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    int health = 100;

    void Die()
    {
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
}
