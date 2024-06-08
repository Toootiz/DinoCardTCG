using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class JuegoEnemigoPanelScript : MonoBehaviour, IDropHandler
{
    // Lista que almacena las cartas que se colocan en este panel.
    public List<GameObject> cards = new List<GameObject>();
    public int maxCards = 4; // Máximo número de cartas permitidas en este panel.

    // Método que se utiliza cuando un objeto es soltado sobre este panel.
    public void OnDrop(PointerEventData eventData)
    {
        // Registra un mensaje en la consola cuando se suelta un objeto en el panel.
        Debug.Log("OnDrop to JuegoEnemigoPanel");

        // Verifica si el objeto arrastrado es no nulo.
        if (eventData.pointerDrag != null)
        {
            // Obtiene el componente RectTransform del objeto arrastrado.
            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

            // Si el objeto arrastrado tiene un RectTransform y un componente CardScript.
            if (draggedRectTransform != null && draggedRectTransform.GetComponent<CardScript>().isEnemyCard)
            {
                // Verifica si el panel ya tiene el máximo de cartas permitidas.
                if (cards.Count >= maxCards)
                {
                    Debug.Log("El panel ya tiene el máximo de cartas permitidas.");
                }
                else
                {
                    // Agrega la carta a la lista de cartas del panel.
                    cards.Add(draggedRectTransform.gameObject);
                    // Mueve la carta al panel.
                    draggedRectTransform.SetParent(transform, false);
                    // Ajusta la posición de la carta en el panel.
                    draggedRectTransform.anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                Debug.Log("No puedes colocar cartas del jugador en el panel de juego enemigo.");
            }
        }
    }

    // Método para agregar una carta enemiga a la lista y al panel.
    public void AddEnemyCard(GameObject card)
    {
        if (cards.Count < maxCards)
        {
            cards.Add(card);
            card.transform.SetParent(transform, false);
            card.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
        {
            Debug.Log("El panel ya tiene el máximo de cartas permitidas.");
        }
    }

    // Método para eliminar una carta enemiga de la lista.
    public void RemoveEnemyCard(GameObject card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
        }
    }
}
