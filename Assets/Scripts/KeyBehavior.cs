using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KeyBehavior : MonoBehaviour
{

    public AudioClip pickupSound;

    // s'il est toucher par le joueur
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehavior>().SetHasKey(true);
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
