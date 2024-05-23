/*
Código que controla la interacción de la carta, como el drag and drop, o la acción que realiza al dar click.
*/

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Variables para guardar los datos que la carta recibe
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad;
    public string CardName;
    public Image CardArt;
    // Variables que funcionan para que cuando la carta se juega no se pueda mover y saber que carta ha sido movida.
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup; // para cambiar la opacidad de la carta
    private bool canDrag = false; // saber si se puede mover o no
    private Vector3 initialPosition;
    private Transform originalParent;
    private bool isPlayed = false; // para indicar si la carta ha sido jugada

    // Referencia al script de gestión del juego
    GameManagement gameManagement;

    void Start()
    {
        // Obtener la referencia al componente con tag de GameController
        gameManagement = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagement>();
        rectTransform = GetComponent<RectTransform>();
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
        // Se checa si la carta no ha sido jugada y hay suficiente energía
        if (!isPlayed && gameManagement.energy >= CardCost)
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
        canvasGroup.alpha = 1.0f; // La carta deja de ser medio transparente
        canvasGroup.blocksRaycasts = true; // Se vuele a tener colisiones

        if (canDrag)
        {
            Transform newParent = transform.parent; // Ver el nuevo padre de la carta

            // Verificar si la carta se mueve al panel de tag "Juego"
            if (newParent.CompareTag("Juego"))
            {
                // Revisar que se tenga suficiente energía
                if (gameManagement.SpendEnergy(CardCost))
                {
                    isPlayed = true; // Marcar la carta como jugada
                    originalParent = newParent; // Cambiar el panel padre al de juego
                    initialPosition = rectTransform.anchoredPosition; // Actualizar la posición inicial
                    Debug.Log("La carta se movió al tablero");
                }
                /*
                else
                {
                    // Revertir a la posición y el padre originales
                    rectTransform.anchoredPosition = initialPosition;
                    transform.SetParent(originalParent);
                    Debug.Log("No se tiene energía para jugar la carta");
                }
                */ //al implementar el script de los paneles esto creo que ya no es necesario pero se deja por si se rompe algo.
            }
            // Verificar si la carta se mueve de vuelta a la zona de banca
            else if (newParent.CompareTag("Banca"))
            {
                // Mostrar mensaje de que no se puede mover la carta de vuelta a la banca
                Debug.Log("La carta no puede volver a la zona de jugo");
                // Revertir a la posición y el padre originales
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
            }
            else if (newParent != originalParent)
            {
                Debug.Log("Card moved to another valid drop zone");
            }
            else
            {
                // Si no se mueve a una zona válida, revertir a la posición original
                rectTransform.anchoredPosition = initialPosition;
                transform.SetParent(originalParent);
            }
        }
        else
        {
            // Si no hay suficiente energía para arrastrar o la carta ya ha sido jugada, revertir a la posición original
            rectTransform.anchoredPosition = initialPosition;
            transform.SetParent(originalParent);
            Debug.Log("Not enough energy to drag the card or card has already been played");
        }
    }

     // Cuando se suelta la carta
    public void OnPointerDown(PointerEventData eventData)
    {
        // Aun no se le da una función a este
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

