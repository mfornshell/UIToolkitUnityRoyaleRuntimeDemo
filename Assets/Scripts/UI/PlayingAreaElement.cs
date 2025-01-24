using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityRoyale;

[UxmlElement]
public partial class PlayingAreaElement : VisualElement
{
    static int _cardCount = 3;

    Dictionary<int, CardElement> _cards = new();

    public Dictionary<int, CardElement> Cards => _cards;

    public PlayingAreaElement()
    {
        for (var i = 0; i < _cardCount; i++)
            _cards.Add(i, null);
    }

    public bool HasEmpty(out int index)
    {
        foreach (var card in _cards)
        {
            if (card.Value == null)
            {
                index = card.Key;
                return true;
            }
        }
        index = 0;
        return false;
    }

    internal void AddCard(int index, CardElement card)
    {
        var pos = card.worldTransform.GetPosition();
        Add(card);
        _cards[index] = card;

        card.MoveTo(card.WorldToLocal(pos));
        card.MoveAndScaleIntoPosition(index, ComputeCardPosition(index));
    }

    private Vector2 ComputeCardPosition(int cardId) => 
        new Vector2((10f + cardId * (layout.width - 2 * 10f) / _cardCount), 0);
}
