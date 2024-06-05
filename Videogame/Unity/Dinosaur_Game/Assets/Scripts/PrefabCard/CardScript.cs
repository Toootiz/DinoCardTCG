using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Variables públicas para configurar las propiedades de la carta.
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad, Cardvenenodmg, Cardquemadodmg, Cardsangradodmg, Cardmordidadmg, Cardcolatazodmg, Cardboostvida, Cardboostataquedmg, Cardboostcosto, Cardduracion;
    public string CardName;
    public Image CardArt;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup; 
    private bool canDrag = false; // Controla si la carta puede ser arrastrada.
    public bool isEnemyCard = false; // Indica si la carta pertenece al enemigo.
    private Vector3 initialPosition;
    private Transform originalParent;

    private bool isPlayed = false; // Indica si la carta ya fue jugada.
    public static CardScript selectedAttacker; // Referencia estática al atacante seleccionado.
    private GameManagement gameManagement;
    private BaseEnemiga baseEnemiga; // Referencia a la base enemiga.

    public TextMeshProUGUI LifeText;

    void Start()
    {
        // Configuración inicial de la carta.
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        baseEnemiga = GameObject.FindGameObjectWithTag("BaseEnemiga").GetComponent<BaseEnemiga>();
        rectTransform = GetComponent<RectTransform>();
        LifeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        originalParent = transform.parent;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
    }

    // Gestiona el inicio del arrastre de la carta.
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEnemyCard)
        {
            Debug.Log("No puedes mover cartas enemigas.");
            return;
        }

        if (!isPlayed && gameManagement.ambar >= CardCost)
        {
            canDrag = true;
            initialPosition = rectTransform.anchoredPosition;
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canDrag = false;
            Debug.Log("¡No tienes suficiente energía!");
        }
    }

    // Actualiza la posición de la carta durante el arrastre.
    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta;
        }
    }

    // Finaliza el arrastre de la carta.
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // Gestión de colocación de la carta en un panel.
        if (canDrag)
        {
            Transform newParent = transform.parent;
            JuegoPanelScript panelScript = newParent.GetComponent<JuegoPanelScript>();
            if (panelScript != null && panelScript.cards.Count >= panelScript.maxCards)
            {
                Debug.Log("El panel ya tiene el máximo de cartas permitidas.");
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
                return;
            }

            if (!isEnemyCard && newParent.CompareTag("JuegoEnemigo"))
            {
                Debug.Log("No puedes colocar cartas del jugador en el panel de juego enemigo.");
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
                return;
            }

            if ((isEnemyCard && newParent.CompareTag("JuegoEnemigo")) || (!isEnemyCard && newParent.CompareTag("Juego")))
            {
                if (gameManagement.SpendEnergy(CardCost))
                {
                    isPlayed = true;
                    originalParent = newParent;
                    initialPosition = rectTransform.anchoredPosition;
                    Debug.Log("La carta se movió correctamente.");
                }
                else
                {
                    rectTransform.anchoredPosition = initialPosition;
                    transform.SetParent(originalParent);
                }
            }
            else
            {
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
            }
            
        }
    }

    // Maneja el evento de clic en la carta.
    public void OnPointerDown(PointerEventData eventData)
    {
        // Gestión de selección y deselección de la carta.
        if (isEnemyCard && selectedAttacker == null)
        {
            Debug.Log("No puedes seleccionar cartas del enemigo sin tener un atacante.");
            return;
        }

        if (selectedAttacker == this)
        {
            DeselectCard();
            return;
        }

        if (!isEnemyCard && transform.parent.CompareTag("Juego") && selectedAttacker == null)
        {
            SelectCard();
        }
        else if (isEnemyCard && transform.parent.CompareTag("JuegoEnemigo") && selectedAttacker != null)
        {
            selectedAttacker.AttackCard(this);
        }
    }

    // Métodos adicionales para gestionar la selección, deselección y ataque de cartas.
    private void SelectCard()
    {
        if (selectedAttacker != null)
        {
            selectedAttacker.DeselectCard();
        }
        selectedAttacker = this;
        canvasGroup.alpha = 0.6f;
        Debug.Log($"Carta {CardName} seleccionada como atacante.");
    }

    public void DeselectCard()
    {
        if (selectedAttacker == this)
        {
            selectedAttacker = null;
            canvasGroup.alpha = 1.0f;
            Debug.Log($"Carta {CardName} ha sido deseleccionada.");
        }
    }

    public void AttackCard(CardScript target)
    {
        if (target != null && gameManagement.ambar >= CardCost)
        {
            if (gameManagement.SpendEnergy(CardCost))
            {
                target.CardLife -= this.CardAttack;
                target.UpdateLifeDisplay();
                Debug.Log($"Atacando a {target.CardName} con {CardName}. Vida restante: {target.CardLife}");

                if (target.CardLife <= 0)
                {
                    Debug.Log($"{target.CardName} ha sido destruida.");
                    Destroy(target.gameObject);
                }

                DeselectCard();
            }
            else
            {
                Debug.Log("No hay suficiente ámbar para atacar.");
            }
        }
        else
        {
            Debug.Log("Objetivo no válido o ámbar insuficiente.");
        }
    }

    public void TakeDamage(int damage)
    {
        // Método para recibir daño.
        CardLife -= damage;
        UpdateLifeDisplay();
    }

    private void UpdateLifeDisplay()
    {
        // Actualiza la UI de la vida de la carta.
        if (LifeText != null)
            LifeText.text = CardLife.ToString();
        if (CardLife <= 0)
            Destroy(gameObject);
    }

    public void OnClic()
    {
        // Este método está vacío, puede ser eliminado o utilizado para otra función.
    }

    void Update()
    {
        // Este método no tiene funcionalidad actualmente, puede ser eliminado o utilizado para actualizar elementos en tiempo real.
    }
}
