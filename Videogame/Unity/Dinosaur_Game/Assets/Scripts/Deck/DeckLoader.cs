using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DeckLoader : MonoBehaviour
{
    public GameObject deckPrefab; // Prefab que representa un deck
    public Transform deckContainer; // Contenedor donde se instanciarán los decks
    public int userId; // ID del usuario

    void Start()
    {
        StartCoroutine(LoadDecks());
    }

    IEnumerator LoadDecks()
    {
        string url = $"http://localhost:3000/api/decks/{userId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            DeckList deckList = JsonUtility.FromJson<DeckList>(json);
            DisplayDecks(deckList.decks);
        }
    }

    void DisplayDecks(List<Deck> decks)
    {
        foreach (Deck deck in decks)
        {
            GameObject deckObject = Instantiate(deckPrefab, deckContainer);
            deckObject.transform.localScale = new Vector3(5, 5, 5); // Escala el panel a 5

            DeckUI deckUI = deckObject.GetComponent<DeckUI>();

            deckUI.SetDeckName(deck.nombre_deck);
            deckUI.SetDeckDescription(deck.descripcion_deck);

            for (int i = 0; i < deck.cards.Count && i < 5; i++)
            {
                deckUI.AddCard(deck.cards[i]);
            }
        }
    }

    [System.Serializable]
    public class Deck
    {
        public string nombre_deck;
        public string descripcion_deck;
        public List<Card> cards;
    }

    [System.Serializable]
    public class DeckList
    {
        public List<Deck> decks;
    }

    [System.Serializable]
    public class Card
    {
        public int id_carta;
        public string Nombre;
        public int Puntos_de_Vida;
        public int Puntos_de_ataque;
        public int Coste_en_elixir;
        public string HabilidadDescripcion;
        public string imagen;
    }
}
