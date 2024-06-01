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
    List<int> numbers = new List<int>();
    public GameObject CardPrefab;
    GameObject canvas;
    GameObject banca;
    GameObject zonaDeJuego;
    public List<GameObject> enemyCards;
    public BaseEnemiga baseEnemiga;
    public BasePropia basePropia;

    GameObject bancaenemigo;
    GameObject ab;
    CardInfo cards;
    public bool gameActive = true;
    public int randomId;
    public enum turn { Player, Enemy };
    public turn currentTurn;
    public int turnCount;
    public int JugadorContadorTurno;
    public int EnemigoContadorTurno;

    public int ambar;
    public int ambarEnemy;
    public string apiResultDedck1;
    [SerializeField] string url;
    [SerializeField] string getEndpoint;
    [SerializeField] string urlEnemigo;
    [SerializeField] string getEndpointEnemgo;
    [SerializeField] Button endTurnButton;
    [SerializeField] TMP_Text AmbarText;

    void Start()
    {
        JugadorContadorTurno = 1;
        EnemigoContadorTurno = 5;
        ambar = 40;
        ambarEnemy = 40;
        AmbarText.text = "Ambar: " + ambar.ToString();
        currentTurn = turn.Player;
        endTurnButton.onClick.AddListener(EndTurn);
        cards = GameObject.FindGameObjectWithTag("CardData").GetComponent<CardInfo>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        banca = GameObject.FindGameObjectWithTag("Banca");
        zonaDeJuego = GameObject.FindGameObjectWithTag("Juego");
        ab = GameObject.FindGameObjectWithTag("JuegoEnemigo");
        bancaenemigo = GameObject.FindGameObjectWithTag("BancaEnemigo");
        GetData(url, getEndpoint);
        GetDataEnemigo(urlEnemigo, getEndpointEnemgo);
        baseEnemiga = GameObject.FindGameObjectWithTag("BaseEnemiga").GetComponent<BaseEnemiga>();
        basePropia = GameObject.FindGameObjectWithTag("ab").GetComponent<BasePropia>();
        if (basePropia == null)
        {
            Debug.LogError("No se encontró el objeto BasePropia. Asegúrate de que el objeto tiene el tag 'BasePropia'.");
        }
        
    }


    public void GetData(string url, string getEndpoint)
    {
        StartCoroutine(RequestGet(url + getEndpoint));
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
                apiResultDedck1 = www.downloadHandler.text;
                Debug.Log("The response was: " + apiResultDedck1);
                cards.Data = apiResultDedck1;
                cards.MakeList();
                GenerateRandomHand(5);
            }
        }
    }

    public void GetDataEnemigo(string urlEnemigo, string getEndpointEnemgo)
    {
        StartCoroutine(RequestGetEnemigo(url + getEndpoint));
    }

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
                GenerateRandomHandEnemigo(5);
            }
        }
    }

    public void countTourn()
    {
        turnCount++;
    }

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

    public bool SpendEnemyEnergy(int amount)
    {
        if (ambarEnemy >= amount)
        {
            ambarEnemy -= amount;
            Debug.Log($"El enemigo gastó {amount} de ámbar. Ámbar restante del enemigo: {ambarEnemy}");
            return true;
        }
        else
        {
            Debug.Log("El enemigo no tiene suficiente ámbar para realizar esta acción.");
            return false;
        }
    }

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
        cardScript.isEnemyCard = true;

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

    public void InstantiateCard(int id, float posX, float posY)
    {
        GameObject newcard = Instantiate(CardPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        newcard.transform.SetParent(banca.transform, false);
        newcard.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

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
            ambarEnemy = 800;
        }
        else if (turnCount >= 6)
        {
            ambarEnemy = 800;
        }
    }

    public void EndTurn()
    {
        endTurnButton.interactable = false;
        DisablePlayerInteractions();
        currentTurn = turn.Enemy;
        StartCoroutine(EnmyTourn());
    }

    IEnumerator EnmyTourn()
{
    countTourn();
    EnemigoContadorTurno += 1;

    bool enemyWillAttack = UnityEngine.Random.Range(0, 2) == 0;

    if (ambarEnemy < 6)
    {
        Debug.Log("El enemigo no tiene suficiente ámbar para jugar cartas.");
    }
    else
    {
        int cardsToPlay = UnityEngine.Random.Range(1, 5);
        int cardsPlayed = 0;
        List<GameObject> playedCards = new List<GameObject>();

        bool playerHasHighAttackCards = false;
        foreach (Transform child in zonaDeJuego.transform)
        {
            CardScript card = child.GetComponent<CardScript>();
            if (card.CardAttack > 5)
            {
                playerHasHighAttackCards = true;
                break;
            }
        }

        foreach (Transform child in bancaenemigo.transform)
        {
            if (cardsPlayed >= cardsToPlay) break;

            CardScript card = child.GetComponent<CardScript>();

            if (playerHasHighAttackCards && card.CardLife > 5)
            {
                if (SpendEnemyEnergy(card.CardCost))
                {
                    cardsPlayed++;
                    Debug.Log($"Enemigo juega {card.CardName} con alta vida.");
                    playedCards.Add(child.gameObject);
                }
            }
            else if (!playerHasHighAttackCards && SpendEnemyEnergy(card.CardCost))
            {
                cardsPlayed++;
                Debug.Log($"Enemigo juega {card.CardName}.");
                playedCards.Add(child.gameObject);
            }
        }

        foreach (GameObject card in playedCards)
        {
            card.transform.SetParent(ab.transform, false);
        }
    }

    if (enemyWillAttack)
    {
        int attackCount = UnityEngine.Random.Range(1, 4);
        int attacksPerformed = 0;

        foreach (Transform enemyCardTransform in ab.transform)
        {
            if (attacksPerformed >= attackCount) break;

            CardScript enemyCard = enemyCardTransform.GetComponent<CardScript>();
            foreach (Transform playerCardTransform in zonaDeJuego.transform)
            {
                CardScript playerCard = playerCardTransform.GetComponent<CardScript>();
                if (SpendEnemyEnergy(enemyCard.CardCost))
                {
                    playerCard.TakeDamage(enemyCard.CardAttack);
                    Debug.Log($"Enemigo ataca {playerCard.CardName} con {enemyCard.CardName}.");
                    attacksPerformed++;
                    break;
                }
            }
        }

        if (EnemigoContadorTurno > 5 && zonaDeJuego.transform.childCount == 0)
        {
            if (basePropia != null)
            {
                foreach (Transform enemyCardTransform in ab.transform)
                {
                    if (attacksPerformed >= attackCount) break;

                    CardScript enemyCard = enemyCardTransform.GetComponent<CardScript>();
                    if (SpendEnemyEnergy(enemyCard.CardCost))
                    {
                        basePropia.TakeDamage(enemyCard.CardAttack);
                        Debug.Log($"Enemigo ataca la base del jugador con {enemyCard.CardName}.");
                        attacksPerformed++;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("No se encontró el objeto BasePropia. Asegúrate de que el objeto tiene el tag 'BasePropia'.");
            }
        }
    }
    else
    {
        Debug.Log("El enemigo decide no atacar este turno.");
    }

    int enemyCardCount = bancaenemigo.transform.childCount;
    if (enemyCardCount < 5)
    {
        int cardsToGenerate = 5 - enemyCardCount;
        GenerateRandomHandEnemigo(cardsToGenerate);
    }

    yield return new WaitForSeconds(5);

    AmbarEnemyTurn();
    currentTurn = turn.Player;
    EnablePlayerInteractions();
    startPlayerTurn();
}

    public void startPlayerTurn()
    {
        Debug.Log("Turno jugador");
        JugadorContadorTurno += 1;
        countTourn();
        AmbarTurn();
        AmbarText.text = "Ambar: " + ambar.ToString();
        endTurnButton.interactable = true;

        int cardCount = banca.transform.childCount;
        if (cardCount < 5)
        {
            int cardsToGenerate = 5 - cardCount;
            GenerateRandomHand(cardsToGenerate);
        }
    }

    void Update()
    {
    }

    void DisablePlayerInteractions()
    {
        endTurnButton.interactable = false;

        foreach (Transform child in banca.transform)
        {
            CardScript card = child.GetComponent<CardScript>();
            if (card != null)
            {
                card.enabled = false;
            }
        }

        foreach (Transform child in zonaDeJuego.transform)
        {
            CardScript card = child.GetComponent<CardScript>();
            if (card != null)
            {
                card.enabled = false;
            }
        }
    }

    void EnablePlayerInteractions()
    {
        endTurnButton.interactable = true;

        foreach (Transform child in banca.transform)
        {
            CardScript card = child.GetComponent<CardScript>();
            if (card != null)
            {
                card.enabled = true;
            }
        }

        foreach (Transform child in zonaDeJuego.transform)
        {
            CardScript card = child.GetComponent<CardScript>();
            if (card != null)
            {
                card.enabled = true;
            }
        }
    }
}
