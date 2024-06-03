/*
Codigo para manejar el panel de zona de jugego (cartas que van a atacar otras cartas)
Panel con TAG "Juego"
29/05/24
*/

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

    // Continúa con la lógica existente si la carta es del jugador
    if (cards.Count >= maxCards)
    {
        Debug.Log("No se pueden agregar más cartas.");
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

