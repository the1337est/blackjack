using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public SettingsData Data;

    [SerializeField] private Slider minBetSlider;
    [SerializeField] private Slider maxBetSlider;
    [SerializeField] private Slider startingMoneySlider;
    [SerializeField] private Slider deckCountSlider;

    [SerializeField] private Text minBetText;
    [SerializeField] private Text maxBetText;
    [SerializeField] private Text startingMoneyText;
    [SerializeField] private Text deckCountText;


    [Header("Modifiers")]
    [SerializeField] private int minBetFactor = 10;
    [SerializeField] private int maxBetFactor = 100;

    [SerializeField] private int moneyFactor = 200;

    void Start()
    {
        minBetSlider.onValueChanged.AddListener((val) => 
        {
            OnMinBetValueChanged((int)val);
        });

        maxBetSlider.onValueChanged.AddListener((val) =>
        {
            OnMaxBetValueChanged((int)val);
        });

        startingMoneySlider.onValueChanged.AddListener((val) =>
        {
            OnStartingMoneyValueChanged((int)val);
        });

        deckCountSlider.onValueChanged.AddListener((val) =>
        {
            OnDeckCountValueChanged((int)val);
        });
        RefreshUI();
    }

    private void RefreshUI()
    {
        OnMinBetValueChanged((int)minBetSlider.value);
        OnMaxBetValueChanged((int)maxBetSlider.value);
        OnStartingMoneyValueChanged((int)startingMoneySlider.value);
        OnDeckCountValueChanged((int)deckCountSlider.value);
    }

    public void OnMinBetValueChanged(int value)
    {
        int final = value * minBetFactor;
        Data.MinBet = final;
        minBetText.text = final.ToString("N0");
    }

    public void OnMaxBetValueChanged(int value)
    {
        int final = value * maxBetFactor;
        Data.MaxBet = final;
        maxBetText.text = final.ToString("N0");
    }

    public void OnStartingMoneyValueChanged(int value)
    {
        int final = value * moneyFactor;
        Data.StartingMoney = final;
        startingMoneyText.text = final.ToString("N0");
    }

    public void OnDeckCountValueChanged(int value)
    {
        deckCountText.text = value.ToString("N0");
        Data.DeckCount = value;
    }
}

[System.Serializable]
public struct SettingsData
{
    public int MinBet;
    public int MaxBet;
    public int StartingMoney;
    public int DeckCount;
}