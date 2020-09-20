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
    public bool DealingInitial = true;
    public bool PlayerStood = false;

    [Header("Gameplay")]
    public int DeckCount = 4;
    public Deck CardDeck;
    public Transform DeckTransform;
    public float CardSpeed = 3f;



    public GameSettings Settings;

    public int Money = 0;
    //public int StartingMoney = 100;
    //public int MinBet = 10;
    //public int MaxBet = 1000;

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
    [SerializeField] private GameObject gameoverUI;

    [SerializeField] private Button continueButton;
    [SerializeField] private Text continueText;

    [SerializeField] private Color enabledColor;
    [SerializeField] private Color disabledColor;

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

        ToggleContinue(PlayerPrefs.HasKey("SavedGame"));

        AllowInput = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
    }

    public void StartGame()
    {

        GenerateDeck();
        State = GameState.Betting;
        bet = Settings.Data.MinBet;
        RefreshBetText();
        Money = Settings.Data.StartingMoney;
        RefreshMoneyText();
        bettingUI.SetActive(true);
        gameplayUI.SetActive(false);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            Settings.Data = JsonUtility.FromJson<SettingsData>(PlayerPrefs.GetString("SavedGame"));
            StartGame();
        }
    }

    private void GenerateDeck()
    {
        CardDeck = new Deck();
        Debug.Log("Deck count: " + Settings.Data.DeckCount);
        CardDeck.CreateDeck(Settings.Data.DeckCount);
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
        int max = Math.Min(Settings.Data.MaxBet, Money);
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
        if (bet - decrement >= Settings.Data.MinBet)
        {
            bet -= decrement;
        }
        else
        {
            bet = Settings.Data.MinBet;
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
        DealingInitial = false;
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

    public void PlayerHit()
    {
        Hit(Player);
    }

    public void Hit(Participant participant, float delay = 0f)
    {
        if (State == GameState.Playing)
        {
            Debug.Log("Hitting for " + participant.name);
            bool allow = !participant.IsPlayer || (AllowInput && participant.IsPlayer);
            if (allow)
            {
                AllowInput = false;
                StartCoroutine(DealWithDelay(participant, delay));
            }
        }
    }

    private IEnumerator DealWithDelay(Participant participant, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        Deal(participant);
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
        StartCoroutine(ShowResult("You won $" + bet.ToString("N0"), 3f));
    }


    public void PlayerLost()
    {
        State = GameState.Finished;
        Money -= bet;
        if (Money < bet && bet >= Settings.Data.MinBet)
        {
            bet = Money;
            RefreshBetText();
        }
        RefreshMoneyText();
        if (Money < Settings.Data.MinBet)
        {
            GameOver();
        }
        else
        {
            ResultText.color = LoseColor;
            StartCoroutine(ShowResult("You lost $" + bet.ToString("N0"), 3f));
        }
    }

    public void PlayerTie()
    {
        State = GameState.Finished;
        ResultText.color = TieColor;
        StartCoroutine(ShowResult("You tied with the dealer!", 3f));
    }

    private void RefreshMoneyText()
    {
        moneyText.text = string.Format("${0}", Money.ToString("N0"));
    }

    private IEnumerator ShowResult(string text, float time)
    {
        SaveGame();
        ResultText.gameObject.SetActive(true);
        ResultText.text = text;
        yield return new WaitForSeconds(time);
        ResultText.gameObject.SetActive(false);

        //todo: reset game
        ResetGame();

        AllowInput = false;
        bettingUI.SetActive(true);
        gameplayUI.SetActive(false);
    }

    private void ResetGame(bool gameover = false)
    {
        if (CardDeck != null)
        {
            CardDeck.ResetDeck();
        }
        Player.Reset();
        Dealer.Reset();
        PlayerStood = false;

        State = gameover ? GameState.Finished : GameState.Betting;
    }

    public void EndPlayerTurn()
    {
        AllowInput = false;
        PlayerStood = true;
        Dealer.BeginTurn();
    }

    public void EndDealerTurn()
    {
        //don't have to check for Score > 21 here because Participant class handles it already
        if (Player.Score > Dealer.Score)
        {
            PlayerWon();
        }
        else if (Player.Score == Dealer.Score)
        {
            PlayerTie();
        }
        else
        {
            PlayerLost();
        }
    }

    public void SaveGame()
    {
        Settings.Data.StartingMoney = Money;
        string json = JsonUtility.ToJson(Settings.Data);
        Debug.Log("Saving: " + json);
        PlayerPrefs.SetString("SavedGame", json);
        ToggleContinue(true);
    }

    public void GameOver()
    {
        ResetGame(gameover:true);
        PlayerPrefs.DeleteAll();
        gameoverUI.SetActive(true);
        ToggleContinue(false);
    }

    private void ToggleContinue(bool enabled)
    {
        continueButton.interactable = enabled;
        continueText.color = enabled ? enabledColor : disabledColor; 
    }
}
