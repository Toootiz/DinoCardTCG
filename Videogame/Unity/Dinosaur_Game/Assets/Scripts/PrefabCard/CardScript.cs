using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad, Cardvenenodmg, Cardquemadodmg, Cardsangradodmg, Cardmordidadmg, Cardcolatazodmg, Cardboostvida, Cardboostataquedmg, Cardboostcosto, Cardduracion;
    public string CardName;
    public Image CardArt;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool canDrag = false;
    public bool isEnemyCard = false;
    private Vector3 initialPosition;
    private Transform originalParent;

    private bool isPlayed = false;
    public static CardScript selectedAttacker;
    private GameManagement gameManagement;
    private BaseEnemiga baseEnemiga;

    public TextMeshProUGUI LifeText;

    void Start()
    {
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

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta;
        }
    }

public void OnEndDrag(PointerEventData eventData)
{
    canvasGroup.alpha = 1.0f;
    canvasGroup.blocksRaycasts = true;

    if (canDrag)
    {
        Transform newParent = eventData.pointerEnter != null ? eventData.pointerEnter.transform : originalParent;
        JuegoPanelScript panelScript = newParent.GetComponent<JuegoPanelScript>();

        // Verificar si el panel tiene espacio antes de mover la carta
        if (panelScript != null && panelScript.cards.Count > panelScript.maxCards)
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
                if (originalParent != newParent)
                {
                    JuegoPanelScript oldPanelScript = originalParent.GetComponent<JuegoPanelScript>();
                    if (oldPanelScript != null)
                    {
                        oldPanelScript.RemoveCard(gameObject);
                    }
                }

                // Mover la carta al nuevo panel
                transform.SetParent(newParent, false);
                initialPosition = rectTransform.anchoredPosition;
                originalParent = newParent;

                // Luego agregarla a la lista del nuevo panel si tiene JuegoPanelScript
                if (panelScript != null && !panelScript.cards.Contains(gameObject))
                {
                    panelScript.cards.Add(gameObject);
                }

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




    public void OnPointerDown(PointerEventData eventData)
    {
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
                    JuegoEnemigoPanelScript enemyPanel = GameObject.FindGameObjectWithTag("JuegoEnemigo").GetComponent<JuegoEnemigoPanelScript>();
                    if (enemyPanel != null && target.isEnemyCard)
                    {
                        enemyPanel.RemoveEnemyCard(target.gameObject);
                    }
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
        CardLife -= damage;
        UpdateLifeDisplay();
    }

    private void UpdateLifeDisplay()
    {
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
