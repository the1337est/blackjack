using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Deck
{
    public List<CardData> Cards;

    public void CreateDeck(int deckCount)
    {
        Cards = new List<CardData>();
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
}