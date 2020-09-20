using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public StatsData Data;

    public Text WonText;
    public Text LostText;
    public Text TiedText;
    public Text MoneyWonText;
    public Text MoneyLostText;
    public Text BlackjackText;

    private const string statsKey = "Stats";

    public void HandWon(int money)
    {
        Data.MoneyWon += money;
        Data.HandsWon++;
        SaveStats();
    }

    public void HandLost(int money)
    {
        Data.MoneyLost += money;
        Data.HandsLost++;
        SaveStats();
    }

    public void HandTied()
    {
        Data.HandsTied++;
        SaveStats();
    }

    public void OnBlackjack()
    {
        Data.BlackjackCount++;
        SaveStats();
    }
    
    public void UpdateData()
    {
        WonText.text = Data.HandsWon.ToString();
        LostText.text = Data.HandsLost.ToString();
        TiedText.text = Data.HandsTied.ToString();
        MoneyWonText.text = string.Format("${0}", Data.MoneyWon.ToString("N0"));
        MoneyLostText.text = string.Format("${0}", Data.MoneyLost.ToString("N0"));
        BlackjackText.text = Data.BlackjackCount.ToString();
    }

    public void ResetStats()
    {
        PlayerPrefs.DeleteKey(statsKey);
        Data = new StatsData();
        SaveStats();
    }

    public void SaveStats()
    {
        string stats = JsonUtility.ToJson(Data);
        Debug.Log("Saving stats: " + stats);
        PlayerPrefs.SetString(statsKey, stats);
        UpdateData();
    }

    public void LoadStats()
    {
        if (PlayerPrefs.HasKey(statsKey))
        {
            try
            {
                Data = JsonUtility.FromJson<StatsData>(PlayerPrefs.GetString(statsKey));
                UpdateData();
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Exception while loading stats: " + ex.Message);
            }
        }
    }

}

[System.Serializable]
public struct StatsData
{
    public int HandsWon;
    public int HandsTied;
    public int HandsLost;
    public int BlackjackCount;
    public double MoneyWon;
    public double MoneyLost;
}
