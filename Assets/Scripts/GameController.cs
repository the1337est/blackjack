using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    [SerializeField] private AssetController assets;
    [SerializeField] private Card cardPrefab;

    public GameState State = GameState.Menu;
    public bool AllowInput;
    public bool PlayerStood = false;

    [Header("Gameplay")]
    public int DeckCount = 4;
    public Deck CardDeck;
    public Transform DeckTransform;
    public float CardSpeed = 3f;

    public int Money = 0;
    public int StartingMoney = 100;
    public int MinBet = 10;
    public int MaxBet = 1000;

    [Header("References")]

    public Dealer Dealer;
    public Player Player;

    public Color WinColor;
    public Color LoseColor;
    public Color TieColor;

    [SerializeField] private Text ResultText;
    [SerializeField] private Text betText;
    [SerializeField] private Text moneyText;

    [SerializeField] private GameObject bettingUI;
    [SerializeField] private GameObject gameplayUI;

    private int bet = 0;

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
       
        AllowInput = false;
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        //todo: check if it's continue

        GenerateDeck();
        State = GameState.Betting;
        bet = MinBet;
        RefreshBetText();
        Money = StartingMoney;
        RefreshMoneyText();
        bettingUI.SetActive(true);
        gameplayUI.SetActive(false);
    }

    private void GenerateDeck()
    {
        CardDeck = new Deck();
        CardDeck.CreateDeck(DeckCount);
    }

    public void Bet()
    {
        bettingUI.SetActive(false);
        gameplayUI.SetActive(true);
        State = GameState.Playing;
        StartCoroutine(InitialDeal());
    }

    public void IncreaseBet()
    {
        int increment = bet < 100 ? 10 : bet < 200 ? 20 : 50;
        int targetBet = bet + increment;
        int max = Math.Min(MaxBet, Money);
        if (targetBet <= max)
        {
            bet += increment;
        }
        else
        {
            bet = max;
        }
        RefreshBetText();
    }

    public void DecreaseBet()
    {
        int decrement = bet < 100 ? 10 : bet < 200 ? 20 : 50;
        if (bet - decrement >= MinBet)
        {
            bet -= decrement;
        }
        else
        {
            bet = MinBet;
        }
        RefreshBetText();
    }

    private void RefreshBetText()
    {
        betText.text = bet.ToString();
    }

    private IEnumerator InitialDeal()
    {
        yield return new WaitForSeconds(1f);
        Deal(Player);
        yield return new WaitForSeconds(0.75f);
        Deal(Player);
        yield return new WaitForSeconds(0.75f);
        Deal(Dealer);
        yield return new WaitForSeconds(0.75f);
        Deal(Dealer);
        yield return new WaitForSeconds(0.75f);
        AllowInput = true;
    }

    public void Deal(Participant participant)
    {
        if (State == GameState.Playing)
        {
            Card card = Instantiate(cardPrefab, participant.Container).GetComponent<Card>();
            card.Set(CardDeck.DrawCard());
            card.transform.position = DeckTransform.position;
            participant.AddCard(card);
        }
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

    public void Hit(Participant participant)
    {
        if (State == GameState.Playing)
        {
            Debug.Log("Hitting for " + participant.name);
            bool allow = !participant.IsPlayer || (AllowInput && participant.IsPlayer);
            if (allow)
            {
                AllowInput = false;
                Deal(participant);
            }
        }
    }

    public void Stand(Participant participant)
    {
        if (State == GameState.Playing)
        {
            if (participant.IsPlayer && AllowInput)
            {
                EndPlayerTurn();
            }
            else if (!participant.IsPlayer)
            {
                EndDealerTurn();
            }
        }
    }

    public void PlayerWon()
    {
        State = GameState.Finished;
        Money += bet;
        ResultText.color = WinColor;
        RefreshMoneyText();
        ShowResult("You won $" + bet.ToString("N0"), 2f);
    }


    public void PlayerLost()
    {
        State = GameState.Finished;
        Money -= bet;
        RefreshMoneyText();
        ResultText.color = LoseColor;
        StartCoroutine(ShowResult("You lost $" + bet.ToString("N0"), 2f));
    }

    public void PlayerTie()
    {
        State = GameState.Finished;
        ResultText.color = TieColor;
        StartCoroutine(ShowResult("You tied with the dealer!", 2f));
    }

    private void RefreshMoneyText()
    {
        moneyText.text = string.Format("${0}", Money.ToString("N0"));
    }

    private IEnumerator ShowResult(string text, float time)
    {
        ResultText.gameObject.SetActive(true);
        ResultText.text = text;
        yield return new WaitForSeconds(time);
        ResultText.gameObject.SetActive(false);

        //todo: reset game

        AllowInput = false;
        bettingUI.SetActive(true);
        gameplayUI.SetActive(false);        
    }

    private void ResetGame()
    {
        //todo: write reset logic
    }

    public void EndPlayerTurn()
    {
        AllowInput = false;
        //todo: make dealer play
        Dealer.BeginTurn();

    }

    public void EndDealerTurn()
    {
        //todo: check who won
        if (Player.Score > Dealer.Score)
        {
            PlayerWon();
        }
    }
}
