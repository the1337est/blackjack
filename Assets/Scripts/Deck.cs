using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

[System.Serializable]
public class Deck
{
    public List<CardData> Cards;
    public List<CardData> UsedCards;

    public void CreateDeck(int deckCount)
    {
        Cards = new List<CardData>();
        UsedCards = new List<CardData>();
        for (int d = 0;  d < deckCount; d++)
        {
            for (int i = 1; i <= 4; i++)
            {
                CardSuit type = (CardSuit)i;
                List<CardData> cards = new List<CardData>();
                for (int c = 1; c <= 13; c++)
                {
                    CardData card = new CardData((CardType)c, type);
                    cards.Add(card);
                }
                Cards.AddRange(cards);
            }
        }
    }

    public void Shuffle()
    {
        Cards = Cards.OrderBy(c => Guid.NewGuid()).ToList();
    }

    public CardData DrawCard()
    {
        int index = Random.Range(0, Cards.Count);
        CardData data = Cards[index];
        UsedCards.Add(data);
        Cards.RemoveAt(index);
        return data;
    }

    public void ResetDeck()
    {
        if (UsedCards != null && UsedCards.Count > 0)
        {
            for (int i = UsedCards.Count - 1; i >= 0;  i--)
            {
                CardData data = UsedCards[i];
                int randomIndex = Random.Range(0, Cards.Count);
                Cards.Insert(randomIndex, data);
                UsedCards.RemoveAt(i);
            }
        }
    }
}