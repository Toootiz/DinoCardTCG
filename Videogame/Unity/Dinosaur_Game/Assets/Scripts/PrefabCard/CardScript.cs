using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    //private BasePropia basePropia;

    public TextMeshProUGUI LifeText;

    void Start()
    {
        gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        baseEnemiga = GameObject.FindGameObjectWithTag("BaseEnemiga").GetComponent<BaseEnemiga>();
        //basePropia = GameObject.FindGameObjectWithTag("BasePropia").GetComponent<BasePropia>();
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
            Transform newParent = transform.parent;

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
        // Aun no se le da una función a este
    }

    void Update()
    {
        // Aun no se necesita actualizar nada
    }
}
