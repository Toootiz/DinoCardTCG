using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardScript2 : MonoBehaviour, IPointerClickHandler
{
    // Variables para guardar los datos que la carta recibe
    public int CardId, CardLife, CardAttack, CardCost, CardHabilidad, Cardvenenodmg, Cardquemadodmg, Cardsangradodmg, Cardmordidadmg, Cardcolatazodmg, Cardboostvida, Cardboostataquedmg, Cardboostcosto, Cardduracion;
    public string CardName;
    public Image CardArt;
    private RectTransform rectTransform;
    private Transform originalParent;

    // Referencia al script de gestión del juego
    DeckManagment gameManagement;

    void Start()
    {
        // Obtener la referencia al componente con tag de GameController
        gameManagement = GameObject.FindGameObjectWithTag("DeckManagment").GetComponent<DeckManagment>();
        rectTransform = GetComponent<RectTransform>();
        originalParent = transform.parent;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.CompareTag("CardsDesck"))
        {
            if (gameManagement.selectedCards.childCount < 10)
            {
                MoveToSelected();
            }
            else
            {
                Debug.Log("No puedes seleccionar más de 10 cartas.");
            }
        }
        else if (transform.parent.CompareTag("SelectedCards"))
        {
            MoveToAll();
        }
    }

    void MoveToSelected()
    {
        originalParent = transform.parent;
        transform.SetParent(GameObject.FindGameObjectWithTag("SelectedCards").transform);
        gameManagement.AddCardToSelected(this);
    }

    void MoveToAll()
    {
        originalParent = transform.parent;
        transform.SetParent(GameObject.FindGameObjectWithTag("CardsDesck").transform);
        gameManagement.RemoveCardFromSelected(this);
    }
}
