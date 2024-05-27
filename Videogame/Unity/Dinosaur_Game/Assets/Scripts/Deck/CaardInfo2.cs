/*
Código que guarda la información del api.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo2 : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab de la carta que se va a instanciar
    public Transform cardParent; // Padre que se tomara para instanciar las cartas 
    public string Data; // Texto que contiene los datos JSON recibidos de la API

    // Clase interna para representar una carta
    [System.Serializable]
    public class Card
    {
        // Atributos de la carta
        public int id_carta;
        public string nombre;
        public int puntos_de_vida;
        public int puntos_de_ataque;
        public int coste_en_elixir;
        public int habilidad;
        public string imagen;
    }

    // Clase interna para representar una lista de cartas
    [System.Serializable]
    public class CardList 
    {
        // Lista de objetos de las cartas
        public Card[] cards;
    }

    // Instancia de la lista de cartas
    public CardList listaCartas = new CardList();

    void Start()
    {
        // Nada de momento
    }

    // Método para convertir el JSON recibido en una lista de cartas
    public void MakeList()
    {
       // Debug.Log("TEST: " + Data); // Imprime el JSON recibido en la consola para verificación
        listaCartas = JsonUtility.FromJson<CardList>(Data); // Convierte el JSON en una instancia de CardList
    }

    // Método para instanciar las cartas en el juego
    public void CreateCards()
    {
        // Recorre cada carta en la lista de cartas
        foreach (var cardData in listaCartas.cards)
        {
            // Instancia el prefab de la carta como hijo del cardParent
            GameObject newCard = Instantiate(cardPrefab, cardParent);
        }
    }
}

