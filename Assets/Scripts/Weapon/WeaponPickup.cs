using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefab; // Référence à l'arme que le joueur peut ramasser
    private bool isNearWeapon = false; // Indique si le joueur est près de l'arme

    void Update()
    {
        if (isNearWeapon && Input.GetKeyDown(KeyCode.E)) // Lorsque le joueur appuie sur 'E'
        {
            EquipWeapon();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur qui entre en collision
        {
            isNearWeapon = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur qui sort de la collision
        {
            isNearWeapon = false;
        }
    }

    void EquipWeapon()
    {
        // Équipe l'arme au joueur
        Player.Instance.EquipWeapon(weaponPrefab); // Appel de la méthode EquipWeapon du joueur
        Destroy(gameObject); // Détruit l'arme sur le sol après l'avoir ramassée
    }
}
