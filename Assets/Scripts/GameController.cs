using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    [SerializeField] private AssetController assets;


    [Header("Gameplay")]
    public int DeckCount = 4;
    public Deck CardDeck;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (Instance != this)
        {
            DestroyImmediate(this);
        }
        CardDeck = new Deck();
        CardDeck.CreateDeck(DeckCount);
    }

    public Sprite GetSprite(CardSuit suit, CardType type)
    {
        Sprite result = null;
        if (assets != null)
        {
            result = assets.GetSprite(suit, type);
        }
        return result;
    }
}
