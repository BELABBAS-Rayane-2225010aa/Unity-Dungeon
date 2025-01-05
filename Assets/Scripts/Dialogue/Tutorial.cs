using UnityEngine;
using UnityEngine.UI; // Pour le système UI
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueUI; // Référence au GameObject de la bulle de dialogue
    public string dialogueText; // Texte à afficher dans la bulle

    // private TextMeshProUGUI dialogueTextComponent;

    private void Start()
    {
        // Assurez-vous que la bulle de dialogue est désactivée au début
        dialogueUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vérifiez si l'objet qui entre dans la zone est le joueur
        if (other.CompareTag("Player"))
        {
            dialogueUI.SetActive(true); // Affichez la bulle
            TextMeshProUGUI dialogueTextComponent = dialogueUI.GetComponentInChildren<TextMeshProUGUI>(); // Récupérez le composant texte     
            dialogueTextComponent.text = dialogueText; // Mettez à jour le texte
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Désactivez la bulle lorsque le joueur sort de la zone
        if (other.CompareTag("Player"))
        {
            dialogueUI.SetActive(false);
            Destroy(gameObject); // Détruisez l'objet déclencheur
        }
    }
}
