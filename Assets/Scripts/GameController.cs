using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    [SerializeField] private List<Sprite> clubsSprites;
    [SerializeField] private List<Sprite> diamondsSprites;
    [SerializeField] private List<Sprite> heartsSprites;
    [SerializeField] private List<Sprite> spadesSprites;

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
    }

    public Sprite GetSprite(CardSuit suit, CardType type)
    {
        Sprite result = null;

        int index = (int)type - 1;
        List<Sprite> list = null;

        switch (suit)
        {
            case CardSuit.Clubs:
                list = clubsSprites;
                break;

            case CardSuit.Diamonds:
                list = diamondsSprites;
                break;

            case CardSuit.Hearts:
                list = heartsSprites;
                break;

            case CardSuit.Spades:
                list = spadesSprites;
                break;

        }
        if (list != null && index >= 0 && index < list.Count)
        {
            result = list[index];
        }
        return result;
    }
}
