using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Associez le composant texte dans l'inspecteur.

    private void Update()
    {
        // Met à jour le texte en fonction du score
        scoreText.text = "Score: " + PlayerScore.Instance.GetScore();
    }
}
