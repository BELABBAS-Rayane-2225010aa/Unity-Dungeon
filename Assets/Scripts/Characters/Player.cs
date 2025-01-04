using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int health = 100;

    GameObject EquipedWeapon;

    void Die()
    {
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

    public void Attack(GameObject target)
    {
        if (EquipedWeapon != null)
        {
            EquipedWeapon.GetComponent<Weapon>().Attack(target);
        }
        else
        {
            AttackWithFists(target);
        }
    }

    void AttackWithFists(GameObject target)
    {
        target.GetComponent<ZombieBehavior>().TakeDamage(10);
    }

    public void EquipWeapon(GameObject weapon)
    {
        EquipedWeapon = weapon;
    }
}
