using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    private bool revealed = false;
    private bool animating = false;

    public CardType Type = CardType.Ace;
    public CardSuit Suit;

    [SerializeField] private SpriteRenderer front;
    [SerializeField] private SpriteRenderer back;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Refresh();
        }
    }

    private void Awake()
    {
        front.enabled = revealed;
        back.enabled = !revealed;
    }

    private void OnMouseDown()
    {
        if (!animating)
        {
            Flip();
        }
    }

    public void Set(CardType type, CardSuit suit)
    {
        Type = type;
        Suit = suit;
    }

    public void Flip()
    {
        animating = true;
        LeanTween.rotateY(gameObject, 90f, 0.15f).setEaseOutSine().setOnComplete(() =>
        {
            front.enabled = !revealed;
            back.enabled = revealed;
            LeanTween.rotateY(gameObject, 0f, 0.15f).setEaseOutBack().setOnComplete(() =>
            {
                revealed = !revealed;
                animating = false;
            });
        });
    }


    private void Refresh()
    {
        try
        {
            front.sprite = GameController.Instance.GetSprite(Suit, Type);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception occurred: {ex.Message}");
        }
    }
}
