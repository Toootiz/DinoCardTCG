using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card: MonoBehaviour
{
    public int id;
    public string cardName;
    public int attack;
    public int life;
    public int cost;
    public int skill;
}

[System.Serializable]
public class CardCollection
{
    public List<Card> cards;
}