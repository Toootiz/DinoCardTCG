using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class DeckManagment : MonoBehaviour
{
    public GameObject CardPrefab; // Referencia al prefab de la carta
    public Transform selectedCards; // Panel para cartas seleccionadas
    public Transform allCards; // Panel para todas las cartas
    public TMP_InputField deckNameInput; // Campo de texto para el nombre del deck
    public TMP_InputField deckDescriptionInput; // Campo de texto para la descripción del deck
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
        for (int i = 0; i < cards.listaCartas.cards.Length; i++)
        {
            InstantiateCard(i, 0, 0);
        }
    }

    public void InstantiateCard(int id, float posX, float posY)
    {
        GameObject newcard = Instantiate(CardPrefab, allCards);
        newcard.transform.localScale = Vector3.one; // Ajustar la escala a 1 para evitar problemas de escala

        TextMeshProUGUI nameText = newcard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI lifeText = newcard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attackText = newcard.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI costText = newcard.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI HabilidadText = newcard.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        Image cardImage = newcard.transform.GetChild(5).GetComponent<Image>();

        nameText.text = cards.listaCartas.cards[id].Nombre;
        lifeText.text = cards.listaCartas.cards[id].Puntos_de_Vida.ToString();
        attackText.text = cards.listaCartas.cards[id].Puntos_de_ataque.ToString();
        costText.text = cards.listaCartas.cards[id].Coste_en_elixir.ToString();
        HabilidadText.text = cards.listaCartas.cards[id].HabilidadDescripcion.ToString();

        Sprite cardSprite = Resources.Load<Sprite>($"DinoImages/{id}");

        if (cardSprite != null)
        {
            cardImage.sprite = cardSprite;
        }
        else
        {
            Debug.LogError($"Image {id} not found in Resources/IMG/");
        }

        CardScript2 cardScript = newcard.GetComponent<CardScript2>();
        cardScript.CardId = cards.listaCartas.cards[id].id_carta; // Asigna el id de la carta
        cardScript.CardName = cards.listaCartas.cards[id].Nombre;
        cardScript.CardAttack = cards.listaCartas.cards[id].Puntos_de_ataque;
        cardScript.CardLife = cards.listaCartas.cards[id].Puntos_de_Vida;
        cardScript.CardCost = cards.listaCartas.cards[id].Coste_en_elixir;
        cardScript.CardHabilidad = cards.listaCartas.cards[id].HabilidadDescripcion;
        cardScript.Cardvenenodmg = cards.listaCartas.cards[id].venenodmg;
        cardScript.Cardquemadodmg = cards.listaCartas.cards[id].quemadodmg;
        cardScript.Cardsangradodmg = cards.listaCartas.cards[id].sangradodmg;
        cardScript.Cardmordidadmg = cards.listaCartas.cards[id].mordidadmg;
        cardScript.Cardcolatazodmg = cards.listaCartas.cards[id].colatazodmg;
        cardScript.Cardboostvida = cards.listaCartas.cards[id].boostvida;
        cardScript.Cardboostataquedmg = cards.listaCartas.cards[id].boostataquedmg;
        cardScript.Cardboostcosto = cards.listaCartas.cards[id].boostcosto;
        cardScript.Cardduracion = cards.listaCartas.cards[id].duracion;
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

    [System.Serializable]
    public class DeckData
    {
        public int id_jugador;
        public string nombre_deck;
        public string descripcion_deck;
        public int id_carta1;
        public int id_carta2;
        public int id_carta3;
        public int id_carta4;
        public int id_carta5;
        public int id_carta6;
        public int id_carta7;
        public int id_carta8;
        public int id_carta9;
        public int id_carta10;
    }

    public void SaveSelectedCards()
    {
        string deckName = deckNameInput.text;
        string deckDescription = deckDescriptionInput.text;

        if (string.IsNullOrEmpty(deckName))
        {
            Debug.LogError("Falta nombre del deck");
            return;
        }

        if (string.IsNullOrEmpty(deckDescription))
        {
            Debug.LogError("Falta descripción del deck");
            return;
        }

        if (selectedCards.childCount != 10)
        {
            Debug.LogError("Debe seleccionar exactamente 10 cartas");
            return;
        }

        List<int> selectedCardIds = new List<int>();

        foreach (Transform cardTransform in selectedCards)
        {
            CardScript2 cardScript = cardTransform.GetComponent<CardScript2>();
            if (cardScript != null)
            {
                selectedCardIds.Add(cardScript.CardId);
            }
        }

        DeckData deckData = new DeckData
        {
            id_jugador = 1, // Asigna el id del jugador según corresponda
            nombre_deck = deckName,
            descripcion_deck = deckDescription,
            id_carta1 = selectedCardIds[0],
            id_carta2 = selectedCardIds[1],
            id_carta3 = selectedCardIds[2],
            id_carta4 = selectedCardIds[3],
            id_carta5 = selectedCardIds[4],
            id_carta6 = selectedCardIds[5],
            id_carta7 = selectedCardIds[6],
            id_carta8 = selectedCardIds[7],
            id_carta9 = selectedCardIds[8],
            id_carta10 = selectedCardIds[9]
        };

        string json = JsonUtility.ToJson(deckData);
        Debug.Log("Selected Cards JSON: " + json);

        PlayerPrefs.SetString("SelectedCards", json);
        PlayerPrefs.Save();

        StartCoroutine(PostDeckData("http://localhost:3000/api/guardardeck", json));
    }

    IEnumerator PostDeckData(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
            Debug.Log("Response: " + request.downloadHandler.text); // Imprimir la respuesta del servidor
        }
        else
        {
            Debug.Log("Deck saved successfully: " + request.downloadHandler.text);
        }
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
