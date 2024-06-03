/*
Código que controla la interacción de la carta, como el drag and drop, o la acción que realiza al dar click.
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;



public class CardScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Variables para guardar los datos que la carta recibe
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad, Cardvenenodmg, Cardquemadodmg, Cardsangradodmg, Cardmordidadmg, Cardcolatazodmg, Cardboostvida, Cardboostataquedmg, Cardboostcosto, Cardduracion;
    public string CardName;
    public Image CardArt;
    // Variables que funcionan para que cuando la carta se juega no se pueda mover y saber que carta ha sido movida.
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup; // para cambiar la opacidad de la carta
    private bool canDrag = false; // saber si se puede mover o no
    public bool isEnemyCard = false;  // Agregar esto al principio de tu script CardScript
    private Vector3 initialPosition;
    private Transform originalParent;
    private bool isPlayed = false; // para indicar si la carta ha sido jugada
    private static CardScript selectedAttacker;
    //private GameManagement gameManagement;

    public TextMeshProUGUI LifeText;
    void Start()
    {
        // Obtener la referencia al componente con tag de GameController
        //gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        rectTransform = GetComponent<RectTransform>();
        LifeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        originalParent = transform.parent; // solicitar el panel padre donde se genero la carta
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>(); // sacar el componente de canvas group
        
    }

    // Cuando inicias el arrastre
    public void OnBeginDrag(PointerEventData eventData)
{
    // Bloquea completamente el arrastre si la carta es del enemigo
    if (isEnemyCard)
    {
        Debug.Log("No puedes mover cartas enemigas.");
        return;  // Termina la ejecución para no permitir arrastrar.
    }

    if (!isPlayed /*&& gameManagement.ambar >= CardCost*/)
    {
        canDrag = true;
        initialPosition = rectTransform.anchoredPosition; // Guardar la posición inicial
        canvasGroup.alpha = 0.6f; // Hacer la carta algo transparente
        canvasGroup.blocksRaycasts = false; // Permitir que las intersecciones con los objetos que tienen colliders pasen a través de la carta mientras se arrastra
    }
    else
    {
        canDrag = false;
        Debug.Log("¡No tienes suficiente energía!");
    }
}


     // Cuando se arrastra la carta
    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            rectTransform.anchoredPosition += eventData.delta; // Actualizar la posición de la carta, se mueve con el mouse
        }
    }

    // Cuando se termina el arrastre
    public void OnEndDrag(PointerEventData eventData)
{
    canvasGroup.alpha = 1.0f; // Restaurar la opacidad de la carta
    canvasGroup.blocksRaycasts = true; // Permitir detección de raycast nuevamente

    if (canDrag)
    {
        Transform newParent = transform.parent; // Obtener el nuevo panel padre después del arrastre

        // Comprobar que la carta del jugador no se coloque en el panel enemigo
        if (!isEnemyCard && newParent.CompareTag("JuegoEnemigo"))
        {
            Debug.Log("No puedes colocar cartas del jugador en el panel de juego enemigo.");
            rectTransform.anchoredPosition = initialPosition; // Revertir a la posición inicial
            transform.SetParent(originalParent); // Revertir al panel padre original
            return;
        }

        if ((isEnemyCard && newParent.CompareTag("JuegoEnemigo")) || (!isEnemyCard && newParent.CompareTag("Juego")))
        {
            /*if (gameManagement.SpendEnergy(CardCost))
            {
                isPlayed = true; // Marcar la carta como jugada
                originalParent = newParent; // Actualizar el panel padre
                initialPosition = rectTransform.anchoredPosition; // Actualizar la posición inicial
                Debug.Log("La carta se movió correctamente.");
            }
            else
            {
                rectTransform.anchoredPosition = initialPosition; // Revertir si no hay suficiente ámbar
                transform.SetParent(originalParent);
            }
            */
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition; // Revertir si se intenta mover a un panel incorrecto
            transform.SetParent(originalParent);
        }
    }
}


  
    void AttackCard(CardScript attacker, CardScript defender)
    {
        defender.CardLife -= attacker.CardAttack;
        Debug.Log($"Ataque! Vida restante de {defender.CardName}: {defender.CardLife}");

        if (defender.CardLife <= 0)
        {
            Debug.Log($"{defender.CardName} ha sido destruida.");
            Destroy(defender.gameObject);
        }

        if (attacker.CardAttack >= defender.CardLife)
        {
            Destroy(attacker.gameObject);
        }

        selectedAttacker = null;  // Des-selecciona el atacante
        canvasGroup.alpha = 1.0f;  // Restablece la opacidad
    }

     // Cuando se suelta la carta
    public void OnPointerDown(PointerEventData eventData)
    {

    // Evitar que se seleccione si es una carta del enemigo
    if (isEnemyCard)
    {
        Debug.Log("No puedes seleccionar cartas del enemigo.");
        return;
    }
        // Si la carta clickeada ya es el atacante seleccionado, deseleccionarla
    if (selectedAttacker == this)
    {
        DeselectCard();
    }
    else if (transform.parent.CompareTag("Juego") && selectedAttacker == null)
    {
        // Seleccionar carta como atacante si ninguna otra carta ha sido seleccionada
        SelectCard();
    }
    else if (transform.parent.CompareTag("JuegoEnemigo") && selectedAttacker != null)
    {
        // Atacar la carta seleccionada si es un enemigo y hay un atacante seleccionado
        selectedAttacker.AttackCard(this);
    }
    else if (selectedAttacker != null)
    {
        // Si hay otro atacante seleccionado y se clickea en otra carta que no es enemigo,
        // se deselecciona el atacante actual y se selecciona la nueva carta
        DeselectCard();
        SelectCard();
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

private void DeselectCard()
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
    if (target != null /*&& gameManagement.ambar >= CardCost*/)
    {
        // Verificar si hay suficiente ámbar para realizar el ataque
        /*
        if (gameManagement.SpendEnergy(CardCost))  // Intentar gastar el ámbar
        {
            target.CardLife -= this.CardAttack;
            target.UpdateLifeDisplay();  // Actualizar la UI de la vida de la carta atacada
            Debug.Log($"Atacando a {target.CardName} con {CardName}. Vida restante: {target.CardLife}");

            if (target.CardLife <= 0)
            {
                Debug.Log($"{target.CardName} ha sido destruida.");
                Destroy(target.gameObject);
            }

            DeselectCard();  // Deselecciona la carta atacante después del ataque
        }
        else
        {
            Debug.Log("No hay suficiente ámbar para atacar.");
        }*/
    }
    else
    {
        Debug.Log("Objetivo no válido o ámbar insuficiente.");
    }
}

    public void TakeDamage(int damage)
    {
        CardLife -= damage;
        UpdateLifeDisplay();  // Actualizar la visualización de la vida
    }

    private void UpdateLifeDisplay()
    {
        if (LifeText != null)
            LifeText.text = CardLife.ToString();  // Actualizar el texto de vida en la UI
        if (CardLife <= 0)
            Destroy(gameObject);  // Destruir la carta si la vida es 0 o menos
    }
    public void OnClic()
    {
        // Aun no se le da una función a este
    }

    void Update()
    {
        // Aun no se necesita  actualizar nada
    }


}