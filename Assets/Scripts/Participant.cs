using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Participant : MonoBehaviour
{
    public List<Card> Cards;
    public Transform Container;
    public Text Count;

    public bool IsPlayer { get; protected set; }

    public int Score { get; private set; }

    public void AddCard(Card card)
    {
        if (Cards == null)
        {
            Cards = new List<Card>();
        }
        Cards.Add(card);
        card.SetOrder(Cards.Count);
        bool reveal = IsPlayer || (!IsPlayer && Cards.Count == 1);
        card.Deal(GetNextPosition(), reveal, ()=> 
        {
            //todo: block input when card is animating
            if (reveal)
            {
                Check();
            }
        });
        MoveContainer();
    }

    public void Check()
    {
        int score = 0;
        for (int i = 0; i < Cards.Count; i++)
        {
            int val = Mathf.Clamp((int)Cards[i].Data.Type, 1, 10);
            score += val;
            Count.text = score.ToString();
        }
        Score = score;
        //todo: modify logic to allow for Ace being 1 or 10.
        if (Score > 21)
        {
            if (IsPlayer)
            {
                GameController.Instance.PlayerLost();
            }
            else
            {
                GameController.Instance.PlayerWon();
            }
        }
        else if (Score < 21)
        {
            if (IsPlayer)
            {
                GameController.Instance.AllowInput = true;
            }
            else
            {
                if (GameController.Instance.PlayerStood)
                {
                    if (Score < 17)
                    {
                        GameController.Instance.Hit(this);
                    }
                    else
                    {
                        Stand();
                    }
                }
            }
        }
        if (Score == 21)
        {
            if (IsPlayer)
            {
                GameController.Instance.PlayerWon();
            }
            else
            {
                GameController.Instance.PlayerLost();
            }
        }
    }

    private Vector3 GetNextPosition()
    {
        Vector3 postiion = Container.position + (Vector3.right * (Cards.Count - 1) * 0.45f);
        return postiion;
    }

    private void MoveContainer()
    {
        if (Container.position.x > -2f)
        {
            LeanTween.moveX(Container.gameObject, Container.position.x - 0.45f, 0.15f);
        }
    }

    public void Stand()
    {
        GameController.Instance.Stand(this);
    }

}
