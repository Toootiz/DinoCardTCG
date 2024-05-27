using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JuegoPanelScript : MonoBehaviour, IDropHandler
{
    public List<GameObject> cards = new List<GameObject>();
    public int maxCards = 5; // Maximum number of cards allowed in the GamePanel

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop to GamePanel");
        if (eventData.pointerDrag != null)
        {
            if (cards.Count >= maxCards)
            {
                Debug.Log("No se pueden agregar mas cartas.");
                return; // Prevent adding more cards if the limit is reached
            }

            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            if (draggedRectTransform != null)
            {
                // Re-parent the dragged card to the new panel (GamePanel)
                draggedRectTransform.SetParent(transform, false);
                // Add the card to the list of cards in this panel
                cards.Add(eventData.pointerDrag);
                // Position the cards in a centered line
                // PositionCardsInLine();
            }
        }
    }

    // private void PositionCardsInLine()
    // {
    //     float totalWidth = (cards.Count - 1);
    //     float startX = -totalWidth / 2;

    //     for (int i = 0; i < cards.Count; i++)
    //     {
    //         RectTransform cardRectTransform = cards[i].GetComponent<RectTransform>();
    //         cardRectTransform.anchoredPosition = new Vector2(startX + i, 0);
    //     }
    // }
}
