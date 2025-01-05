using UnityEngine;
using TMPro;
using System.Collections;

public class TextManager : MonoBehaviour
{
    public TextMeshProUGUI textObject;  // Référence à l'objet Text (UI ou TextMeshPro)
    private bool isTextVisible = false;

    void Update()
    {
        // Si le texte est visible et que le joueur appuie sur E
        if (isTextVisible && Input.GetKeyDown(KeyCode.E))
        {
            HideText();
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowText();
        }
    }
    public void ShowText()
    {
        // Afficher le texte et activer le suivi de visibilité
        textObject.gameObject.SetActive(true);
        isTextVisible = true;
    }

    public void HideText()
    {
        // Faire disparaître le texte et désactiver le suivi de visibilité
        textObject.gameObject.SetActive(false);
        isTextVisible = false;
    }
}
