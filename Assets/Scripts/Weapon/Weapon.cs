using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 50;

    public void Attack(GameObject target)
    {
        target.GetComponent<Zombie>().TakeDamage(damage);
    }
}
