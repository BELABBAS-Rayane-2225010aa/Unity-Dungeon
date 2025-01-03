using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    void Start()
    {
        // Récupère le score du joueur
        int score = PlayerScore.Instance.GetScore();

        // Affiche le score dans le composant texte
        Debug.Log("Score: " + score);
        GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;
    }
}
