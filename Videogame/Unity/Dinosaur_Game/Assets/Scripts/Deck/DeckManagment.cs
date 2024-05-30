using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckManagment : MonoBehaviour
{
    public GameObject CardPrefab; // Referencia al prefab de la carta
    public Transform selectedCards; // Panel para cartas seleccionadas
    public Transform allCards; // Panel para todas las cartas
    CardInfo2 cards;
    public string apiResult;  // Resultado de la llamada a la API

    void Start()
    {
        cards = GameObject.FindGameObjectWithTag("CardData").GetComponent<CardInfo2>();
        selectedCards = GameObject.FindGameObjectWithTag("SelectedCards").transform;
        allCards = GameObject.FindGameObjectWithTag("CardsDesck").transform;
        LoadCardData();
        GenerateAllCards(); // Generar todas las cartas
    }

    void LoadCardData()
    {
        if (PlayerPrefs.HasKey("CardData"))
        {
            apiResult = PlayerPrefs.GetString("CardData");
            cards.Data = apiResult;
            cards.MakeList();
        }
        else
        {
            Debug.LogError("No card data found in PlayerPrefs");
        }
    }

    public void GenerateAllCards()
    {
        for (int i = 0; i < cards.listaCartas.cards.Length-1; i++)
        {
            InstantiateCard(i, 0, 0);
        }
    }

    public void InstantiateCard(int id, float posX, float posY)
    {
        GameObject newCard = Instantiate(CardPrefab, allCards);
        newCard.transform.localScale = Vector3.one; // Ajustar la escala a 1 para evitar problemas de escala

        // Asignar textos y datos a la carta
        TextMeshProUGUI nameText = newCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI lifeText = newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attackText = newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI costText = newCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI habilidadText = newCard.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        Image cardImage = newCard.transform.GetChild(5).GetComponent<Image>();

        nameText.text = cards.listaCartas.cards[id].nombre;
        lifeText.text = cards.listaCartas.cards[id].puntos_de_vida.ToString();
        attackText.text = cards.listaCartas.cards[id].puntos_de_ataque.ToString();
        costText.text = cards.listaCartas.cards[id].coste_en_elixir.ToString();
        habilidadText.text = cards.listaCartas.cards[id].habilidad.ToString();

        // Cargar la imagen desde Resources usando el ID como nombre del archivo
        Sprite cardSprite = Resources.Load<Sprite>($"DinoImages/{cards.listaCartas.cards[id].id_carta}");

        if (cardSprite != null)
        {
            cardImage.sprite = cardSprite;
        }
        else
        {
            Debug.LogError($"Image {cards.listaCartas.cards[id].id_carta} not found in Resources/IMG/");
        }

        // Asignar datos a los atributos de la carta
        CardScript2 cardScript = newCard.GetComponent<CardScript2>();
        cardScript.CardId = cards.listaCartas.cards[id].id_carta;
        cardScript.CardName = cards.listaCartas.cards[id].nombre;
        cardScript.CardAttack = cards.listaCartas.cards[id].puntos_de_ataque;
        cardScript.CardLife = cards.listaCartas.cards[id].puntos_de_vida;
        cardScript.CardCost = cards.listaCartas.cards[id].coste_en_elixir;
        cardScript.CardHabilidad = cards.listaCartas.cards[id].habilidad;
        cardScript.CardArt = cardImage;
    }

    public void AddCardToSelected(CardScript2 card)
    {
        // Opcional: realiza alguna acción adicional cuando se agrega una carta a la selección
    }

    public void RemoveCardFromSelected(CardScript2 card)
    {
        // Opcional: realiza alguna acción adicional cuando se elimina una carta de la selección
    }

    public void SaveSelectedCards()
    {
        List<CardInfo2.Card> selectedCardsList = new List<CardInfo2.Card>();
        foreach (Transform cardTransform in selectedCards)
        {
            CardScript2 cardScript = cardTransform.GetComponent<CardScript2>();
            CardInfo2.Card card = new CardInfo2.Card
            {
                id_carta = cardScript.CardId,
                nombre = cardScript.CardName,
                puntos_de_vida = cardScript.CardLife,
                puntos_de_ataque = cardScript.CardAttack,
                coste_en_elixir = cardScript.CardCost,
                habilidad = cardScript.CardHabilidad
            };
            selectedCardsList.Add(card);
        }

        CardInfo2.CardList cardList = new CardInfo2.CardList { cards = selectedCardsList.ToArray() };
        string json = JsonUtility.ToJson(cardList);
        Debug.Log("Selected Cards JSON: " + json);

        PlayerPrefs.SetString("SelectedCards", json);
        PlayerPrefs.Save();
    }

    public void ClearSelectedCards()
    {
        foreach (Transform child in selectedCards)
        {
            child.SetParent(allCards);
        }
    }

     public void SaveAndChangeScene()
    {
        SaveSelectedCards();
        SceneManager.LoadScene("SelectedDeck");
    }



    public void DeckScene()
    {
        SceneManager.LoadScene("SelectedDeck");
    }



}