using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    private bool revealed = false;
    private bool animating = false;

    public CardData Data;

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
        RefreshVisibility();
    }

    private void OnMouseDown()
    {
        if (!animating)
        {
            Flip();
        }
    }

    /// <summary>
    /// Sets cards data and graphics
    /// </summary>
    /// <param name="type"></param>
    /// <param name="suit"></param>
    public void Set(CardData data)
    {
        Data = data;
        Refresh();
    }

    public void SetOrder(int order)
    {
        front.sortingOrder = order;
        back.sortingOrder = order;
    }

    /// <summary>
    /// Flips the card over revealing the underside
    /// </summary>
    public void Flip()
    {
        animating = true;
        LeanTween.scale(gameObject, Vector3.one * 1.2f, 0.15f).setEaseOutSine().setOnComplete(() => 
        {
            LeanTween.scale(gameObject, Vector3.one, 0.15f).setEaseOutBack();
        });
        LeanTween.rotateY(gameObject, 90f, 0.15f).setEaseOutSine().setOnComplete(() =>
        {
            revealed = !revealed;
            RefreshVisibility();
            LeanTween.rotateY(gameObject, 0f, 0.15f).setEaseOutBack().setOnComplete(() =>
            {
                animating = false;
            });
        });
    }

    /// <summary>
    /// Animates / "deals" the cards to a position
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="endPosition"></param>
    public void Deal(Vector3 position, bool reveal = false, System.Action complete = null)
    {
        Vector3 currentPosition = transform.position;
        float time = Vector3.Distance(currentPosition, position) / GameController.Instance.CardSpeed;
        LeanTween.move(gameObject, position, time).setEaseInOutQuad().setOnComplete(()=> 
        {
            complete?.Invoke();
            if (reveal)
            {
                Flip();
            }
        });
    }

    /// <summary>
    /// Refreshes Card sprite and visible side
    /// </summary>
    private void Refresh()
    {
        try
        {
            front.sprite = GameController.Instance.GetSprite(Data.Suit, Data.Type);
            RefreshVisibility();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Refreshes Card visibility
    /// </summary>
    private void RefreshVisibility()
    {
        front.enabled = revealed;
        back.enabled = !revealed;
    }
}

[System.Serializable]
public struct CardData
{
    public CardType Type;
    public CardSuit Suit;

    public CardData(CardType type, CardSuit suit)
    {
        Type = type;
        Suit = suit;
    }
}