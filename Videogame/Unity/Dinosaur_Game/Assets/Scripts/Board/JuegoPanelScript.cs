using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JuegoPanelScript : MonoBehaviour, IDropHandler //manejo de eventos drop
{
    // Lista para almacenar las cartas en el panel.
    public List<GameObject> cards = new List<GameObject>();
    // Número máximo de cartas permitidas en el panel.
    public int maxCards = 5;

    // Lo que pasa al soltar la carta en el panel
    public void OnDrop(PointerEventData eventData)
    {
        CardScript card = eventData.pointerDrag.GetComponent<CardScript>();
        if (card != null && card.isEnemyCard)
        {
            Debug.Log("No se pueden soltar cartas enemigas aquí.");
            return;  // Impide que las cartas enemigas se suelten en el panel de juego.
        }

        if (cards.Count >= maxCards)
        {
            Debug.Log("No se pueden agregar más cartas.");
            return;
        }

        // Verificar si el jugador tiene suficiente ambar para pagar el coste de la carta
        GameManagement gameManagement = GameObject.FindGameObjectWithTag("GameManagement").GetComponent<GameManagement>();
        if (gameManagement.ambar < card.CardCost)
        {
            Debug.Log("¡No tienes suficiente energía!");
            return;
        }

        RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
        if (draggedRectTransform != null)
        {
            draggedRectTransform.SetParent(transform, false);
            cards.Add(eventData.pointerDrag);
            Debug.Log("Carta agregada al panel de juego.");
        }
    }
}
