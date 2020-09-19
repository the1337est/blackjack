using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    [SerializeField] private AssetController assets;
    [SerializeField] private Card cardPrefab;

    public GameState State = GameState.Menu;

    [Header("Gameplay")]
    public int DeckCount = 4;
    public Deck CardDeck;
    public Transform DeckTransform;
    public float CardSpeed = 3f;

    public Dealer Dealer;
    public Player Player;

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
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Deal(Player);
        }
    }

    public void StartGame()
    {
        Invoke("Deal", 2f);
    }

    public void Deal(Participant participant)
    {
        Card card = Instantiate(cardPrefab, participant.Container).GetComponent<Card>();
        card.Set(CardDeck.DrawCard());
        card.transform.position = DeckTransform.position;
        participant.AddCard(card);
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
