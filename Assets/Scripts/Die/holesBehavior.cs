using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holesBehavior : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            animator.SetTrigger("open");
            other.gameObject.GetComponent<Player>().TakeDamage(100);
            animator.SetTrigger("close");
        }
    }
}
