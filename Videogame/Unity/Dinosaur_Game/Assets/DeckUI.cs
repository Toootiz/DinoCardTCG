using UnityEngine;
using TMPro;

public class DeckUI : MonoBehaviour
{
    public TextMeshProUGUI deckNameText;
    public TextMeshProUGUI deckDescriptionText;
    public Transform cardsPanel;

    void Awake()
    {
        // Asegurarse de que todos los componentes están asignados
        if (deckNameText == null || deckDescriptionText == null || cardsPanel == null)
        {
            Debug.LogError("One or more UI components are not assigned in DeckUI.");
        }
    }
}