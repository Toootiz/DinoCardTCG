using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BancaScript : MonoBehaviour, IDropHandler
{
    public List<GameObject> cards = new List<GameObject>();

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop to BancaPanel");
        if (eventData.pointerDrag != null)
        {
            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            if (draggedRectTransform != null)
            {
                // Prevent the card from being moved back to the Banca panel
                Debug.Log("Cards cannot be moved back to the Banca panel");
            }
        }
    }
}

