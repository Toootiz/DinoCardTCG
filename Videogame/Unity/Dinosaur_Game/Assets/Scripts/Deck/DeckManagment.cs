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

        newcard.GetComponent<CardScript2>().CardName = cards.listaCartas.cards[id].Nombre;
        newcard.GetComponent<CardScript2>().CardAttack = cards.listaCartas.cards[id].Puntos_de_ataque;
        newcard.GetComponent<CardScript2>().CardLife = cards.listaCartas.cards[id].Puntos_de_Vida;
        newcard.GetComponent<CardScript2>().CardCost = cards.listaCartas.cards[id].Coste_en_elixir;
        newcard.GetComponent<CardScript2>().CardHabilidad = cards.listaCartas.cards[id].HabilidadDescripcion;
        newcard.GetComponent<CardScript2>().Cardvenenodmg = cards.listaCartas.cards[id].venenodmg;
        newcard.GetComponent<CardScript2>().Cardquemadodmg = cards.listaCartas.cards[id].quemadodmg;
        newcard.GetComponent<CardScript2>().Cardsangradodmg = cards.listaCartas.cards[id].sangradodmg;
        newcard.GetComponent<CardScript2>().Cardmordidadmg = cards.listaCartas.cards[id].mordidadmg;
        newcard.GetComponent<CardScript2>().Cardcolatazodmg = cards.listaCartas.cards[id].colatazodmg;
        newcard.GetComponent<CardScript2>().Cardboostvida = cards.listaCartas.cards[id].boostvida;
        newcard.GetComponent<CardScript2>().Cardboostataquedmg = cards.listaCartas.cards[id].boostataquedmg;
        newcard.GetComponent<CardScript2>().Cardboostcosto = cards.listaCartas.cards[id].boostcosto;
        newcard.GetComponent<CardScript2>().Cardduracion = cards.listaCartas.cards[id].duracion;
        newcard.GetComponent<CardScript2>().CardArt = cardImage;
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
    public class Card
    {
        public int id_carta;
        public string Nombre;
        public int Puntos_de_Vida;
        public int Puntos_de_ataque;
        public int Coste_en_elixir;
        public int HabilidadDescripcion;
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
        public string imagen;
    }

    [System.Serializable]
    public class CardList
    {
        public Card[] cards;
    }

    [System.Serializable]
    public class DeckData
    {
        public int id_jugador;
        public string nombre_deck;
        public string descripcion_deck;
        public CardList cards;
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
                Nombre = cardScript.CardName,
                Puntos_de_Vida = cardScript.CardLife,
                Puntos_de_ataque = cardScript.CardAttack,
                Coste_en_elixir = cardScript.CardCost,
                HabilidadDescripcion = cardScript.CardHabilidad,
                venenodmg = cardScript.Cardvenenodmg,
                quemadodmg = cardScript.Cardquemadodmg,
                sangradodmg = cardScript.Cardsangradodmg,
                mordidadmg = cardScript.Cardmordidadmg,
                colatazodmg = cardScript.Cardcolatazodmg,
                boostvida = cardScript.Cardboostvida,
                boostataquedmg = cardScript.Cardboostataquedmg,
                boostcosto = cardScript.Cardboostcosto,
                duracion = cardScript.Cardduracion
            };
            selectedCardsList.Add(card);
        }

        CardInfo2.CardList cardList = new CardInfo2.CardList { cards = selectedCardsList.ToArray() };
        string json = JsonUtility.ToJson(cardList);
        //Debug.Log(cardList);
        //Debug.Log("Selected Cards JSON: " + json);
        Debug.Log(json);

        PlayerPrefs.SetString("SelectedCards", json);
        PlayerPrefs.Save();

        DeckData deckData = new DeckData
        {
            id_jugador = 1, // Asumiendo que el ID del jugador es 1, reemplazar con el ID real del jugador
            //nombre_deck = "Mi Deck",
            //escripcion_deck = "Este es mi deck personalizado",
            cards = new CardList { cards = selectedCardsList.ConvertAll(card => new DeckManagment.Card {
                id_carta = card.id_carta,
                Nombre = card.Nombre,
                Puntos_de_Vida = card.Puntos_de_Vida,
                Puntos_de_ataque = card.Puntos_de_ataque,
                Coste_en_elixir = card.Coste_en_elixir,
                HabilidadDescripcion = card.HabilidadDescripcion,
                id_habilidad = card.id_habilidad,
                venenodmg = card.venenodmg,
                quemadodmg = card.quemadodmg,
                sangradodmg = card.sangradodmg,
                mordidadmg = card.mordidadmg,
                colatazodmg = card.colatazodmg,
                boostvida = card.boostvida,
                boostataquedmg = card.boostataquedmg,
                boostcosto = card.boostcosto,
                duracion = card.duracion,
                imagen = card.imagen
            }).ToArray() }
        };

        string deckJson = JsonUtility.ToJson(deckData);
        Debug.Log(deckJson);
        StartCoroutine(PostDeckData("http://localhost:3000/api/guardardecks", deckJson));
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
