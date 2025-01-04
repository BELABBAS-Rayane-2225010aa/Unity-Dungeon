using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    int health = 100;

    void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    void Awake()
    {

        if (Instance == null)

        {

            Instance = this;

        }

        else

        {

            Destroy(gameObject);

        }
    }
}
