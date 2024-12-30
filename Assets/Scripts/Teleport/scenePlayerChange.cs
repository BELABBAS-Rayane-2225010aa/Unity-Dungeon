using UnityEngine;
using UnityEngine.SceneManagement;

public class scenePlayerChange : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Nom de la scène à charger

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet qui entre est le joueur
        if (other.CompareTag("Player"))
        {
            // Charge la scène spécifiée
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
