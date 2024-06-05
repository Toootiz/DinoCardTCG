using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class DeckDisplayManager : MonoBehaviour
{
    public GameObject DeckPrefab; // Referencia al prefab del deck
    public GameObject CardPrefab; // Referencia al prefab de la carta
    public Transform DecksPanel; // Panel donde se mostrarán los decks
    public string url = "http://localhost:3000/api/decks/"; // URL base para conectar con la API
    private int id_jugador;

    void Start()
    {
        id_jugador = PlayerPrefs.GetInt("PlayerID", 1); // Obtener el ID del jugador desde PlayerPrefs
        GetDecks();
    }

    void GetDecks()
    {
        string getDecksEndpoint = url ; //+ id_jugador.ToString(); // Construir el endpoint con el ID del jugador
        StartCoroutine(RequestGet(getDecksEndpoint));
    }

    IEnumerator RequestGet(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Request failed: " + www.error);
            }
            else
            {
                string apiResult = www.downloadHandler.text;
                Debug.Log("The response was: " + apiResult);
                DecksData decksData = JsonUtility.FromJson<DecksData>(apiResult);
                DisplayDecks(decksData.decks);
            }
        }
    }

void DisplayDecks(List<Deck> decks)
{
    if (decks == null || decks.Count == 0)
    {
        Debug.Log("No decks found for the player.");
        return;
    }

    // Limpiar el contenido actual del panel antes de agregar nuevos decks
    foreach (Transform child in DecksPanel)
    {
        Destroy(child.gameObject);
    }

    foreach (Deck deck in decks)
    {
        GameObject newDeck = Instantiate(DeckPrefab, DecksPanel);
        newDeck.transform.localScale = Vector3.one;

        DeckUI deckUI = newDeck.GetComponent<DeckUI>();

        if (deckUI == null)
        {
            Debug.LogError("DeckUI script is not attached to the DeckPrefab.");
            continue;
        }

        deckUI.deckNameText.text = deck.nombre_deck;
        deckUI.deckDescriptionText.text = deck.descripcion_deck;

        // Agregar algunas cartas del deck
        foreach (var card in deck.cards.Take(3)) // Mostrar solo las primeras 3 cartas
        {
            GameObject newCard = Instantiate(CardPrefab, deckUI.cardsPanel);
            newCard.transform.localScale = Vector3.one;

            TextMeshProUGUI nameText = newCard.transform.Find("CardName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI lifeText = newCard.transform.Find("CardLife").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI attackText = newCard.transform.Find("CardAttack").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costText = newCard.transform.Find("CardCost").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI habilidadText = newCard.transform.Find("CardHabilidad").GetComponent<TextMeshProUGUI>();
            Image cardImage = newCard.transform.Find("CardImage").GetComponent<Image>();

            if (nameText == null || lifeText == null || attackText == null || costText == null || habilidadText == null || cardImage == null)
            {
                Debug.LogError("Card prefab structure is incorrect. Make sure all required components exist in the prefab.");
                continue;
            }

            nameText.text = card.nombre_carta;
            lifeText.text = card.puntos_de_vida.ToString();
            attackText.text = card.puntos_de_ataque.ToString();
            costText.text = card.coste_en_elixir.ToString();
            habilidadText.text = card.descripcion;

            Sprite cardSprite = Resources.Load<Sprite>($"DinoImages/{card.id_carta}");

            if (cardSprite != null)
            {
                cardImage.sprite = cardSprite;
            }
            else
            {
                Debug.LogError($"Image {card.id_carta} not found in Resources/IMG/");
            }
        }
    }
}

}

[System.Serializable]
public class Deck
{
    public int id_jugador;
    public int id_deck;
    public int cantidad_cartas;
    public string nombre_deck;
    public string descripcion_deck;
    public List<DeckCard> cards;
}

[System.Serializable]
public class DeckCard
{
    public int id_carta;
    public string nombre_carta;
    public int puntos_de_vida;
    public int puntos_de_ataque;
    public int coste_en_elixir;
    public string descripcion;
    public int id_habilidad;
    public int venenodmg;
    public int quemadodmg;
    public int sangradodmg;
    public int mordidadmg;
    public int colatazodmg;
    public int boostvida;
    public int boostataquedmg;
    public int boostcosto;
    public int duracion;
    public string fecha_de_creacion;
    public string fecha_modificacion;
}

[System.Serializable]
public class DecksData
{
    public List<Deck> decks;
}
