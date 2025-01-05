using System.Collections;
using UnityEngine;

public class teleporter : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;

            // get next level distance
            float nextLevelDistance = player.GetComponent<Player>().getNextLevelDistance();

            // désactive PlayerBehavior
            player.GetComponent<PlayerBehavior>().enabled = false;

            // téléporte le joueur à la position du prochain level
            player.transform.position = new Vector3(-8, nextLevelDistance + 0.5f, 7);

            // réactive PlayerBehavior après un court délai
            StartCoroutine(ReenablePlayerBehavior(player));

            // incrémente la distance du prochain level
            player.GetComponent<Player>().setNextLevelDistance(nextLevelDistance + 6f);
        }
    }

    IEnumerator ReenablePlayerBehavior(GameObject player)
    {
        yield return new WaitForSeconds(0.1f); // Attendre 0.1 seconde avant de réactiver
        player.GetComponent<PlayerBehavior>().enabled = true;
    }
}