using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DeckUI : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI deckNameText;
    public TextMeshProUGUI deckDescriptionText;
    public Transform cardContainer; // Contenedor para las cartas del deck
    public GameObject cardPrefab; // Prefab que representa una carta
    private string deckName;
    private int deckId;

    // Referencia al GameManager o a un script que maneje la lógica del juego
    private DeckSelectionManager deckSelectionManager;

    void Start()
    {
        deckSelectionManager = FindObjectOfType<DeckSelectionManager>();
    }

    public void SetDeckName(string name, int id)
    {
        deckName = name;
        deckId = id;
        deckNameText.text = name;
        Debug.Log(deckName + deckId);
    }

    public void SetDeckDescription(string description)
    {
        deckDescriptionText.text = description;
    }

    public void AddCard(DeckLoader.Card card)
    {
        GameObject cardObject = Instantiate(cardPrefab, cardContainer);
        cardObject.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

        CardScript3 cardScript3 = cardObject.GetComponent<CardScript3>();

        cardScript3.CardId = card.id_carta;
        cardScript3.CardName = card.Nombre;
        cardScript3.CardLife = card.Puntos_de_Vida;
        cardScript3.CardAttack = card.Puntos_de_ataque;
        cardScript3.CardCost = card.Coste_en_elixir;

        int habilidad;
        if (int.TryParse(card.HabilidadDescripcion, out habilidad))
        {
            cardScript3.CardHabilidad = habilidad;
        }
        else
        {
            cardScript3.CardHabilidad = 0;
        }

        cardScript3.cardNameText = cardObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cardScript3.cardLifeText = cardObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        cardScript3.cardAttackText = cardObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        cardScript3.cardCostText = cardObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        cardScript3.cardHabilidadText = cardObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        cardScript3.CardArt = cardObject.transform.GetChild(5).GetComponent<Image>();

        cardScript3.UpdateCardUI();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (deckSelectionManager != null)
        {
            deckSelectionManager.SelectDeck(deckName, deckId);
        }
    }
}
