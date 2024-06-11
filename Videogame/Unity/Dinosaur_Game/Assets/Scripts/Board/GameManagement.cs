using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

// Esta clase maneja la lógica principal del juego TCG, incluyendo turnos, energía, y las cartas en juego.
public class GameManagement : MonoBehaviour
{
    // Declaración de variables y referencias a objetos y componentes.
    List<int> numbers = new List<int>(); // Lista para almacenar números aleatorios (usados para seleccionar cartas).
    public GameObject CardPrefab; // Prefab de la carta para instanciar cartas en el juego.
    GameObject canvas; // Referencia al canvas principal del juego.
    GameObject banca; // Referencia al panel del jugador donde se almacenan cartas.
    GameObject zonaDeJuego; // Referencia al área de juego donde las cartas interactúan.
    public List<GameObject> enemyCards; // Lista de cartas enemigas.
    public BaseEnemiga baseEnemiga; // Referencia a la base enemiga.
    public BasePropia basePropia; // Referencia a la base propia del jugador.

    GameObject bancaenemigo; // Referencia al panel de la banca enemiga.
    GameObject ab; // Área de juego del enemigo.
    CardInfo cards; // Información sobre las cartas disponibles.
    public bool gameActive = true; // Estado del juego, activo o no.
    public int randomId; // ID aleatorio para la selección de cartas.
    public enum turn { Player, Enemy }; // Enumeración para controlar el turno actual.
    public turn currentTurn; // Variable para el turno actual.
    public int turnCount; // Contador de turnos totales.
    public int JugadorContadorTurno; // Contador de turnos del jugador.
    public int EnemigoContadorTurno; // Contador de turnos del enemigo.

    public int ambar; // Energía de ambar del jugador.
    public int ambarEnemy; // Energía de ambar del enemigo.
    public string apiResultDedck1; // Resultado de la API para las cartas.
    public string apiResultDedck2; // Resultado de la API para las cartas.
    [SerializeField] string url; // URL base para la API.
    [SerializeField] string getEndpoint; // Endpoint para obtener las cartas del jugador.
    [SerializeField] string urlEnemigo; // URL para la API de cartas del enemigo.
    [SerializeField] string getEndpointEnemgo; // Endpoint para obtener las cartas del enemigo.
    [SerializeField] Button endTurnButton; // Botón para terminar el turno.
    [SerializeField] TMP_Text AmbarText; // Texto UI para mostrar el ambar del jugador.
    [SerializeField] TMP_Text AmbarEnemyText; // Texto UI para mostrar el ambar del enemigo.
    [SerializeField] TMP_Text TurnoActualText;

    // Inicialización de componentes y configuración inicial.
    void Start()
    {
        JugadorContadorTurno = 1;
        EnemigoContadorTurno = 1;
        ambar = 3;
        ambarEnemy = 3;
        AmbarText.text = "Ambar: " + ambar.ToString();
        AmbarEnemyText.text = "Ambar: " + ambarEnemy.ToString(); 
        currentTurn = turn.Player;
        TurnoActualText.text = "Turno Actual: Jugador";
        endTurnButton.onClick.AddListener(EndTurn);
        cards = GameObject.FindGameObjectWithTag("CardData").GetComponent<CardInfo>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        banca = GameObject.FindGameObjectWithTag("Banca");
        zonaDeJuego = GameObject.FindGameObjectWithTag("Juego");
        ab = GameObject.FindGameObjectWithTag("JuegoEnemigo");
        bancaenemigo = GameObject.FindGameObjectWithTag("BancaEnemigo");

        // Obtén el ID del deck seleccionado desde PlayerPrefs
        int selectedDeckId = PlayerPrefs.GetInt("SelectedDeckId", 0);
        Debug.Log(selectedDeckId);

        int selectedDeckIdEnemigo = Random.Range(4, 9);
        Debug.Log(selectedDeckIdEnemigo);

        // Construye la URL completa utilizando el ID del deck seleccionado
        string deckUrl = $"{url}/api/deckjugador/{selectedDeckId}";
        string deckUrlEnemigo = $"{url}/api/deckjugador/{selectedDeckIdEnemigo}";
        GetData(deckUrl);
        GetDataEnemigo(deckUrlEnemigo);

        baseEnemiga = GameObject.FindGameObjectWithTag("BaseEnemiga").GetComponent<BaseEnemiga>();
        basePropia = GameObject.FindGameObjectWithTag("ab").GetComponent<BasePropia>();
    }

    public void GetData(string fullUrl)
    {
        StartCoroutine(RequestGet(fullUrl));
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

        public void GetDataEnemigo(string fullUrlEnemigo)
        {
            StartCoroutine(RequestGetEnemigo(fullUrlEnemigo));
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
                apiResultDedck2 = www.downloadHandler.text;
                Debug.Log("The response was: " + apiResultDedck2);
                cards.Data = apiResultDedck2;
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
        if (JugadorContadorTurno <= 4)
        {
            ambar = ambar + 3;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
        else if (JugadorContadorTurno <= 8)
        {
            ambar = ambar + 6;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
        else if (JugadorContadorTurno >= 14)
        {
            ambar = ambar + 8;
            AmbarText.text = "Ambar: " + ambar.ToString();
        }
    }

    public void AmbarEnemyTurn()
    {
        if (EnemigoContadorTurno <= 4)
        {
            ambarEnemy = ambarEnemy + 3;
            AmbarEnemyText.text = "Ambar: " + ambarEnemy.ToString();
        }
        else if (EnemigoContadorTurno <= 8)
        {
            ambarEnemy = ambarEnemy + 6;
            AmbarEnemyText.text = "Ambar: " + ambarEnemy.ToString();
        }
        else if (EnemigoContadorTurno >= 14)
        {
            ambarEnemy = ambarEnemy + 8;
            AmbarEnemyText.text = "Ambar: " + ambarEnemy.ToString();
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

            JuegoEnemigoPanelScript enemyPanel = GameObject.FindGameObjectWithTag("JuegoEnemigo").GetComponent<JuegoEnemigoPanelScript>();

            foreach (Transform child in bancaenemigo.transform)
            {
                if (cardsPlayed >= cardsToPlay || enemyPanel.cards.Count >= enemyPanel.maxCards) break;

                CardScript card = child.GetComponent<CardScript>();

                if (playerHasHighAttackCards && card.CardLife > 5)
                {
                    if (SpendEnemyEnergy(card.CardCost))
                    {
                        cardsPlayed++;
                        Debug.Log($"Enemigo juega {card.CardName} con alta vida.");
                        playedCards.Add(child.gameObject);
                        if (enemyPanel != null)
                        {
                            enemyPanel.AddEnemyCard(child.gameObject);
                        }
                    }
                }
                else if (!playerHasHighAttackCards && SpendEnemyEnergy(card.CardCost))
                {
                    cardsPlayed++;
                    Debug.Log($"Enemigo juega {card.CardName}.");
                    playedCards.Add(child.gameObject);
                    if (enemyPanel != null)
                    {
                        enemyPanel.AddEnemyCard(child.gameObject);
                    }
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

                        if (playerCard.CardLife <= 0)
                        {
                            JuegoPanelScript playerPanel = GameObject.FindGameObjectWithTag("Juego").GetComponent<JuegoPanelScript>();
                            if (playerPanel != null)
                            {
                                playerPanel.RemoveCard(playerCard.gameObject);
                            }
                            Destroy(playerCard.gameObject);
                            Debug.Log($"{playerCard.CardName} ha sido destruida.");
                        }

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

        yield return new WaitForSeconds(1.5f);

        AmbarEnemyTurn();
        currentTurn = turn.Player;
        TurnoActualText.text = "Turno Actual: Jugador"; // Indica que el turno es del jugador.
        EnablePlayerInteractions();
        startPlayerTurn();
    }

    public void startPlayerTurn()
    {
        TurnoActualText.text = "Turno actual: Jugador";
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
