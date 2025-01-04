using System.Collections;
using UnityEngine;

public class teleporter : MonoBehaviour
{
    public float nextLevelDistance = 4f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(nextLevelDistance);
            GameObject player = other.gameObject;

            // désactive PlayerBehavior
            player.GetComponent<PlayerBehavior>().enabled = false;

            // téléporte le joueur à la position du prochain level
            player.transform.position = new Vector3(-8, nextLevelDistance + .5f, 7);

            // réactive PlayerBehavior après un court délai
            StartCoroutine(ReenablePlayerBehavior(player));
        }
    }

    IEnumerator ReenablePlayerBehavior(GameObject player)
    {
        yield return new WaitForSeconds(0.1f); // Attendre 0.1 seconde avant de réactiver
        player.GetComponent<PlayerBehavior>().enabled = true;
        nextLevelDistance += 4;
    }
}