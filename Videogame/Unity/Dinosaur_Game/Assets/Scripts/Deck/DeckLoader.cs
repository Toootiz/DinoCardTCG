using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckLoader : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab de la carta que se va a instanciar
    public Transform cardParent; // Padre que se tomara para instanciar las cartas 

    [System.Serializable]
    public class Card
    {
        public int id_carta;
        public string nombre;
        public int puntos_de_vida;
        public int puntos_de_ataque;
        public int coste_en_elixir;
        public int habilidad;
        public string imagen;
    }

    [System.Serializable]
    public class CardList
    {
        public Card[] cards;
    }

    void Start()
    {
        LoadDeck();
    }

    void LoadDeck()
    {
        if (PlayerPrefs.HasKey("SelectedCards"))
        {
            string json = PlayerPrefs.GetString("SelectedCards");
            CardList cardList = JsonUtility.FromJson<CardList>(json);
            DisplayCards(cardList);
        }
        else
        {
            Debug.LogError("No saved deck found in PlayerPrefs");
        }
    }

    void DisplayCards(CardList cardList)
    {
        foreach (var cardData in cardList.cards)
        {
            GameObject newCard = Instantiate(cardPrefab);
            newCard.transform.localScale = Vector3.one; // Ajustar la escala a 1 para evitar problemas de escala

            // Asignar textos y datos a la carta
            TextMeshProUGUI nameText = newCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI lifeText = newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI attackText = newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costText = newCard.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI habilidadText = newCard.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            Image cardImage = newCard.transform.GetChild(5).GetComponent<Image>();

            nameText.text = cardData.nombre;
            lifeText.text = cardData.puntos_de_vida.ToString();
            attackText.text = cardData.puntos_de_ataque.ToString();
            costText.text = cardData.coste_en_elixir.ToString();
            habilidadText.text = cardData.habilidad.ToString();

            // Cargar la imagen desde Resources usando el ID como nombre del archivo
            Sprite cardSprite = Resources.Load<Sprite>($"DinoImages/{cardData.id_carta}");

            if (cardSprite != null)
            {
                cardImage.sprite = cardSprite;
            }
            else
            {
                Debug.LogError($"Image {cardData.id_carta} not found in Resources/IMG/");
            }
        }
    }
}
