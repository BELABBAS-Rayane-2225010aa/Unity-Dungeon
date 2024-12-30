using UnityEngine;
using Cinemachine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint; // Point d'apparition
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Caméra virtuelle

    private void Start()
    {
        // Trouve le joueur avec le tag "Player"
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && spawnPoint != null)
        {
            // Déplace le joueur au point d'apparition
            player.transform.position = spawnPoint.position;
            
            // Réattribue le joueur à la caméra virtuelle
            if (virtualCamera != null)
            {
                virtualCamera.Follow = player.transform;
                virtualCamera.LookAt = player.transform;
            }
        }
    }
}
