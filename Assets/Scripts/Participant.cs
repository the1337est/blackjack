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
        bool reveal = IsPlayer || (!IsPlayer && Cards.Count != 2);
        card.Deal(GetNextPosition(), reveal, ()=> 
        {
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
                //only allow input if it's not the initial deal animation
                if (!GameController.Instance.DealingInitial)
                {
                    GameController.Instance.AllowInput = true;
                }
            }
            else
            {
                if (GameController.Instance.PlayerStood)
                {
                    if (Score < 17)
                    {
                        GameController.Instance.Hit(this, 1f);
                    }
                    else
                    {
                        Stand();
                    }
                }
            }
        }
        else if (Score == 21)
        {
            if (IsPlayer)
            {
                GameController.Instance.Stats.OnBlackjack();
                GameController.Instance.PlayerWon();
            }
            else
            {
                GameController.Instance.PlayerLost();
            }
        }
    }

    public void Reset()
    {
        Score = 0;
        Count.text = "";
        if (Cards != null)
        {
            for (int index = Cards.Count - 1; index >= 0; index--)
            {
                Destroy(Cards[index].gameObject);
            }
            Cards = new List<Card>();
        }
        
        Container.position = new Vector3(0f, Container.position.y, 0f);
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
