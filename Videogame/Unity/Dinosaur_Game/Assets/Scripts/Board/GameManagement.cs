/*
Código que controla las mecanicas del juego y recoge los datos del api.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Unity.VisualScripting;
using System;
using UnityEngine.Networking;

public class GameManagement : MonoBehaviour
{
    // Lista para almacenar números usados para generar manos de cartas
    List<int> numbers = new List<int>();
    public GameObject CardPrefab; // Referencia al prefab de la carta
   // Referencias a varios objetos del juego
    GameObject canvas;
    GameObject banca;

    GameObject bancaenemigo;

    GameObject ab;
    CardInfo cards;
    // APIConection aPIConection; de momento esto no se ocupa pero se puede ocupar para mandar datos.
    // Variables para controlar el estado del juego
    public bool gameActive = true;
    public int randomId;
    public enum turn { Player, Enemy }; // Enumeración para los turnos del jugador y del enemigo
    public turn currentTurn; // Turno actual
    public int turnCount; // Contador de turno
    public int ambar; // Ambar lo que se utiliza como energía

    public int ambarEnemy;
    public string apiResultDedck1;  // Resultado de la llamada a la API
    // URLs y elementos de la UI
    [SerializeField] string url; // URL para conectar con el api
    [SerializeField] string getEndpoint; // Endpoint del api para sacar las cartas

    [SerializeField] string urlEnemigo; // URL para conectar con el api
    [SerializeField] string getEndpointEnemgo;
    [SerializeField] Button endTurnButton; // Botón para terminar turno
    [SerializeField] TMP_Text AmbarText; // Texto que muestra el ámbar que se tiene

    void Start()
    {
        // Inicializar variables y UI
        ambar = 40; //establecer un valor inicial para pruebas
        ambarEnemy = 40;
        AmbarText.text = "Ambar: " + ambar.ToString(); //mostrar el valor inical de ambar
        currentTurn = turn.Player; // Se inicia en jugador para las pruebas pero se cambia a random
        endTurnButton.onClick.AddListener(EndTurn); // Saber si se dio clic en el botón de turno
        cards = GameObject.FindGameObjectWithTag("CardData").GetComponent<CardInfo>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        banca = GameObject.FindGameObjectWithTag("Banca");
        ab = GameObject.FindGameObjectWithTag("JuegoEnemigo"); // Change the variable type from GameObject to GameObject[]
        bancaenemigo = GameObject.FindGameObjectWithTag("BancaEnemigo");
        GetData(url, getEndpoint); //Solicitar los datos del a base de datos.
        GetDataEnemigo(urlEnemigo, getEndpointEnemgo);
        // Cargar los datos de las cartas desde PlayerPrefs
        //LoadCardData();

        //GenerateRandomHand(5); // Dar 5 cartas random del deck
    }

    // Cargar datos de las cartas desde PlayerPrefs
    // void LoadCardData()
    // {
    //     if (PlayerPrefs.HasKey("CardData"))
    //     {
    //         apiResultDedck1 = PlayerPrefs.GetString("CardData");
    //         cards.Data = apiResultDedck1;
    //         cards.MakeList();
    //     }
    //     else
    //     {
    //         Debug.LogError("No card data found in PlayerPrefs");
    //     }
    // }

    public void GetData(string url, string getEndpoint)
    {
        StartCoroutine(RequestGet(url + getEndpoint));
    }

    // Realizar una petición GET a la API
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
                 apiResultDedck1 = www.downloadHandler.text;
                 Debug.Log("The response was: " + apiResultDedck1);
                 cards.Data = apiResultDedck1;
                 cards.MakeList();
                 GenerateRandomHand(5); // Dar 5 cartas random del deck
             }
        }
     }

    public void GetDataEnemigo(string urlEnemigo, string getEndpointEnemgo)
    {
        StartCoroutine(RequestGetEnemigo(url + getEndpoint));
    }

    // Realizar una petición GET a la API
    IEnumerator RequestGetEnemigo(string url)
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
                 apiResultDedck1 = www.downloadHandler.text;
                 Debug.Log("The response was: " + apiResultDedck1);
                 cards.Data = apiResultDedck1;
                 cards.MakeList();
                 GenerateRandomHandEnemigo(5); // Dar 5 cartas random del deck
             }
        }
     }

    
    // Contador de turnos
    public void countTourn()
    {
        turnCount++;
    }



    // Método para gastar energía
    public bool SpendEnergy(int amount)
    {
        if (ambar >= amount)
        {
            ambar -= amount;
            AmbarText.text = "Ambar: " + ambar.ToString();
            Debug.Log($"Se gastaron {amount} de ámbar. Ámbar restante: {ambar}");
            return true;
        }
        else
        {
            Debug.Log("No hay suficiente ámbar para realizar esta acción.");
            return false;
        }
    }

    // Generar una mano de cartas aleatoria
    public void GenerateRandomHand(int numberOfCards)
    {
        numbers.Clear();
        for (int i = 0; i < numberOfCards; i++)
        {
            int number;
            do
            {
                number = UnityEngine.Random.Range(0, 10);
                Debug.Log(cards.listaCartas.cards.Length);
            } while (numbers.Contains(number));
            numbers.Add(number);
        }
        for (int i = 0; i < numberOfCards; i++)
        {
            InstantiateCard(numbers[i], 0, 0);
        }
    }

     public void GenerateRandomHandEnemigo(int numberOfCards)
    {
        numbers.Clear();
        for (int i = 0; i < numberOfCards; i++)
        {
            int number;
            do
            {
                number = UnityEngine.Random.Range(0, 10);
                Debug.Log(cards.listaCartas.cards.Length);
            } while (numbers.Contains(number));
            numbers.Add(number);
        }
        for (int i = 0; i < numberOfCards; i++)
        {
            InstantiateCardEnemigo(numbers[i], 0, 0);
        }
    }

    

    public void InstantiateCardEnemigo(int id, float posX, float posY)
    {
        GameObject newcard = Instantiate(CardPrefab, new Vector3(posX, posY, 0), Quaternion.Euler(180, 180, 0));
        newcard.transform.SetParent(bancaenemigo.transform, false);
        newcard.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        CardScript cardScript = newcard.GetComponent<CardScript>();
        cardScript.isEnemyCard = true;  // Asegurarte de que se marca como carta enemiga
        //cardScript.isEnemyCard = true;  // Asegurarte de que se marca como carta enemiga

        // Asignar textos y datos a la carta
        TextMeshProUGUI nameText = newcard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI lifeText = newcard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attackText = newcard.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI costText = newcard.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI HabilidadText = newcard.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        Image cardImage = newcard.transform.GetChild(5).GetComponent<Image>();

        //Debug.Log(id);
        nameText.text = cards.listaCartas.cards[id].Nombre;
        lifeText.text = cards.listaCartas.cards[id].Puntos_de_Vida.ToString();
        attackText.text = cards.listaCartas.cards[id].Puntos_de_ataque.ToString();
        costText.text = cards.listaCartas.cards[id].Coste_en_elixir.ToString();
        HabilidadText.text = cards.listaCartas.cards[id].HabilidadDescripcion.ToString();

        // Cargar la imagen desde Resources usando el ID como nombre del archivo
        Sprite cardSprite = Resources.Load<Sprite>($"DinoImages/{id}");

        if (cardSprite != null)
        {
            cardImage.sprite = cardSprite;
        }
        else
        {
            Debug.LogError($"Image {id} not found in Resources/IMG/");
        }

        // Asignar datos a los atributos de la carta
        //newcard.GetComponent<CardScript>().CardId = id;
        newcard.GetComponent<CardScript>().CardName = cards.listaCartas.cards[id].Nombre;
        newcard.GetComponent<CardScript>().CardAttack = cards.listaCartas.cards[id].Puntos_de_ataque;
        newcard.GetComponent<CardScript>().CardLife = cards.listaCartas.cards[id].Puntos_de_Vida;
        newcard.GetComponent<CardScript>().CardCost = cards.listaCartas.cards[id].Coste_en_elixir;
        newcard.GetComponent<CardScript>().CardHabilidad = cards.listaCartas.cards[id].HabilidadDescripcion;
        newcard.GetComponent<CardScript>().Cardvenenodmg = cards.listaCartas.cards[id].venenodmg;
        newcard.GetComponent<CardScript>().Cardquemadodmg = cards.listaCartas.cards[id].quemadodmg;
        newcard.GetComponent<CardScript>().Cardsangradodmg = cards.listaCartas.cards[id].sangradodmg;
        newcard.GetComponent<CardScript>().Cardmordidadmg = cards.listaCartas.cards[id].mordidadmg;
        newcard.GetComponent<CardScript>().Cardcolatazodmg = cards.listaCartas.cards[id].colatazodmg;
        newcard.GetComponent<CardScript>().Cardboostvida = cards.listaCartas.cards[id].boostvida;
        newcard.GetComponent<CardScript>().Cardboostataquedmg = cards.listaCartas.cards[id].boostataquedmg;
        newcard.GetComponent<CardScript>().Cardboostcosto = cards.listaCartas.cards[id].boostcosto;
        newcard.GetComponent<CardScript>().Cardduracion = cards.listaCartas.cards[id].duracion;
        newcard.GetComponent<CardScript>().CardArt = cardImage;
    }



     // Instanciar una carta en la posición especificada
    public void InstantiateCard(int id, float posX, float posY)
    {
        GameObject newcard = Instantiate(CardPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        newcard.transform.SetParent(banca.transform, false);
        newcard.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        // Asignar textos y datos a la carta
        TextMeshProUGUI nameText = newcard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI lifeText = newcard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI attackText = newcard.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI costText = newcard.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI HabilidadText = newcard.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        Image cardImage = newcard.transform.GetChild(5).GetComponent<Image>();

        //Debug.Log(id);
        nameText.text = cards.listaCartas.cards[id].Nombre;
        lifeText.text = cards.listaCartas.cards[id].Puntos_de_Vida.ToString();
        attackText.text = cards.listaCartas.cards[id].Puntos_de_ataque.ToString();
        costText.text = cards.listaCartas.cards[id].Coste_en_elixir.ToString();
        HabilidadText.text = cards.listaCartas.cards[id].HabilidadDescripcion.ToString();

        // Cargar la imagen desde Resources usando el ID como nombre del archivo
        Sprite cardSprite = Resources.Load<Sprite>($"DinoImages/{id}");

        if (cardSprite != null)
        {
            cardImage.sprite = cardSprite;
        }
        else
        {
            Debug.LogError($"Image {id} not found in Resources/IMG/");
        }

        // Asignar datos a los atributos de la carta
        //newcard.GetComponent<CardScript>().CardId = id;
        newcard.GetComponent<CardScript>().CardName = cards.listaCartas.cards[id].Nombre;
        newcard.GetComponent<CardScript>().CardAttack = cards.listaCartas.cards[id].Puntos_de_ataque;
        newcard.GetComponent<CardScript>().CardLife = cards.listaCartas.cards[id].Puntos_de_Vida;
        newcard.GetComponent<CardScript>().CardCost = cards.listaCartas.cards[id].Coste_en_elixir;
        newcard.GetComponent<CardScript>().CardHabilidad = cards.listaCartas.cards[id].HabilidadDescripcion;
        newcard.GetComponent<CardScript>().Cardvenenodmg = cards.listaCartas.cards[id].venenodmg;
        newcard.GetComponent<CardScript>().Cardquemadodmg = cards.listaCartas.cards[id].quemadodmg;
        newcard.GetComponent<CardScript>().Cardsangradodmg = cards.listaCartas.cards[id].sangradodmg;
        newcard.GetComponent<CardScript>().Cardmordidadmg = cards.listaCartas.cards[id].mordidadmg;
        newcard.GetComponent<CardScript>().Cardcolatazodmg = cards.listaCartas.cards[id].colatazodmg;
        newcard.GetComponent<CardScript>().Cardboostvida = cards.listaCartas.cards[id].boostvida;
        newcard.GetComponent<CardScript>().Cardboostataquedmg = cards.listaCartas.cards[id].boostataquedmg;
        newcard.GetComponent<CardScript>().Cardboostcosto = cards.listaCartas.cards[id].boostcosto;
        newcard.GetComponent<CardScript>().Cardduracion = cards.listaCartas.cards[id].duracion;
        newcard.GetComponent<CardScript>().CardArt = cardImage;
    }

    // Maneja la energía por turno da 3 y va aumentando
    public void AmbarTurn()
    {
        if (turnCount <= 3)
        {
            ambar = 130;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
        else if (turnCount <= 6)
        {
            ambar = 6;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
    }

    public void AmbarEnemyTurn()
    {
        if (turnCount <= 3)
        {
            ambar = 130;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
        else if (turnCount <= 6)
        {
            ambar = 6;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
    }

    public void EndTurn()
    {
        endTurnButton.interactable = false; // Desactivar el botón de fin de turno
        currentTurn = turn.Enemy;
        StartCoroutine(EnmyTourn());
    }

    IEnumerator EnmyTourn()
    {
        // countTourn();
        // yield return new WaitForSeconds(5); // Como no hace nada se le da un temporizador de 5 seg
        // currentTurn = turn.Player;
        // startPlayerTurn();
        countTourn();
        int cardsPlayed = 0;

        List<GameObject> playedCards = new List<GameObject>(); // Lista para almacenar cartas jugadas

        foreach (Transform child in bancaenemigo.transform)
        {
            if (cardsPlayed >= 2) break;

            CardScript card = child.GetComponent<CardScript>();

            if (ambarEnemy >= card.CardCost)
            {
                ambarEnemy -= card.CardCost;
                cardsPlayed++;
                Debug.Log($"Enemigo juega {card.CardName} que cuesta {card.CardCost} de ámbar.");
                playedCards.Add(child.gameObject); // Añadir carta a la lista de jugadas
            }
        }

        foreach (GameObject card in playedCards)
        {
            card.transform.SetParent(ab.transform, false); // Cambiar el padre de la carta al panel de juego del enemigo
            //card.transform.localScale = Vector3.one; // Restablecer la escala si es necesario
        }

        yield return new WaitForSeconds(5); // Esperar 5 segundos para simular acciones del enemigo

        AmbarEnemyTurn();
        currentTurn = turn.Player;
        startPlayerTurn();
    }

    // Iniciar el turno del jugador
    public void startPlayerTurn()
    {
        Debug.Log("Turno jugador");
        countTourn();
        AmbarTurn();
        AmbarText.text = "Ambar: " + ambar.ToString();
        endTurnButton.interactable = true; // Reactivar el botón de fin de turno

        // Verificar cuántas cartas hay en la banca y añadir las que faltan
        int cardCount = banca.transform.childCount;
        if (cardCount < 5)
        {
            int cardsToGenerate = 5 - cardCount;
            GenerateRandomHand(cardsToGenerate);
        }
    }

    // Nada aquís
    void Update()
    {
    }
}

