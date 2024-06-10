using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeckSelectionManager : MonoBehaviour
{
    public TextMeshProUGUI selectedDeckText; // Texto para mostrar el mensaje de deck seleccionado
    private int selectedDeckId;

    public void SelectDeck(string deckName, int deckId)
    {
        selectedDeckId = deckId;
        selectedDeckText.text = $"Deck {deckName} seleccionado";
        PlayerPrefs.SetInt("SelectedDeckId", deckId);
        PlayerPrefs.Save();
    }

    public void ConfirmSelection()
    {
        if (selectedDeckId != 0)
        {
            SceneManager.LoadScene("MenuInicial"); // Cambia "MenuScene" por el nombre de tu escena de menú
        }
        else
        {
            Debug.Log(selectedDeckId);
            Debug.LogError("No deck selected!");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Deck"); // Cambia "MenuScene" por el nombre de tu escena de menú
    }
}
