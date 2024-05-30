/*
Codigo que maneja el moviento de la carta al panel (Zona de juego)
TAG del panel "Juego"
29/05/24
*/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BancaScript : MonoBehaviour, IDropHandler //manejo de eventos drop
{
    // Lista que almacena las cartas que se colocan en este panel.
    public List<GameObject> cards = new List<GameObject>();

    // Método que se utiliza cuando un objeto es soltado sobre este panel.
    public void OnDrop(PointerEventData eventData)
    {
        // Registra un mensaje en la consola cuando se suelta un objeto en el panel.
        Debug.Log("OnDrop to BancaPanel");

        // Verifica si el objeto arrastrado es no nulo.
        if (eventData.pointerDrag != null)
        {
            // Obtiene el componente RectTransform del objeto arrastrado.
            RectTransform draggedRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

            // Si el objeto arrastrado tiene un RectTransform, bloquea su movimiento.
            if (draggedRectTransform != null)
            {
                // Registra un mensaje indicando que no se permite mover la carta de regreso al panel Banca.
                Debug.Log("Ya pasaste esta carta a la zona de juego");
            }
        }
    }
}
