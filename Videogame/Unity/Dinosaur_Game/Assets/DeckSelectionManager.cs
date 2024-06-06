using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeckSelectionManager : MonoBehaviour
{
    public TextMeshProUGUI selectedDeckText; // Texto para mostrar el mensaje de deck seleccionado
    private string selectedDeckName;

    public void SelectDeck(string deckName)
    {
        selectedDeckName = deckName;
        selectedDeckText.text = $"Deck {deckName} seleccionado";
        PlayerPrefs.SetString("SelectedDeck", deckName);
        PlayerPrefs.Save();
    }

    public void ConfirmSelection()
    {
        if (!string.IsNullOrEmpty(selectedDeckName))
        {
            SceneManager.LoadScene("MenuInicial"); // Cambia "MenuScene" por el nombre de tu escena de menú
        }
        else
        {
            Debug.LogError("No deck selected!");
        }
    }

    public void VolveraDeck()
    {
        SceneManager.LoadScene("Deck"); // Cambia "MenuScene" por el nombre de tu escena de menú
    }
}
