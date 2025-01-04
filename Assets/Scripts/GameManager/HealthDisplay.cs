using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText; // Référence au texte qui affiche les PV
    private Player player; // Référence au joueur

    void Start()
    {
        // Assurez-vous que le joueur est bien référencé
        player = Player.Instance;
    }

    void Update()
    {
        // Met à jour l'affichage des PV
        if (player != null)
        {
            healthText.text = player.GetHealth()+"/100"; // Affiche les PV
        }
    }
}
