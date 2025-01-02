using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance; // Singleton pour accéder facilement à l'instance

    private int score = 0;

    private void Awake()
    {
        // Vérifie si une autre instance existe
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Empêche la destruction lors du chargement d'une nouvelle scène
        }
        else
        {
            Destroy(gameObject); // Détruit le nouvel objet si une instance existe déjà
        }
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0; // Remet le score à zéro si nécessaire (par exemple, pour redémarrer une partie)
    }
}
