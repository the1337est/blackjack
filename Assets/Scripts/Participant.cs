using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Participant : MonoBehaviour
{
    public List<Card> Cards;
    public Transform Container;

    protected bool isPlayer = false;

    public void AddCard(Card card)
    {
        if (Cards == null)
        {
            Cards = new List<Card>();
        }
        Cards.Add(card);
        card.SetOrder(Cards.Count);
        bool reveal = isPlayer || (!isPlayer && Cards.Count == 1);
        card.Deal(GetNextPosition(), reveal);
    }

    public void Check()
    {

    }

    private Vector3 GetNextPosition()
    {
        Vector3 postiion = Container.position + (Vector3.right * (Cards.Count - 1) * 0.45f);
        return postiion;
    }
}
