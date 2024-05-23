using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPool : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (DragHandler.objBeingDraged == null) return;
        DragHandler.objBeingDraged.transform.SetParent(transform);
    }

}