using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    public AudioClip pickupSound; // Son à jouer lorsqu'une pièce est ramassée.
    public int coinValue = 1; // Valeur de la pièce, ajustable dans l'inspecteur.

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est le joueur
        if (other.CompareTag("Player"))
        {
            // Ajoutez la valeur de la pièce au score du joueur
            PlayerScore.Instance.AddScore(coinValue);

            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Détruisez la pièce après l'avoir ramassée
            Destroy(gameObject);
        }
    }
}
