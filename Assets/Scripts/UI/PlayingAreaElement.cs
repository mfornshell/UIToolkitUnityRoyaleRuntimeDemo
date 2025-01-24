using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityRoyale;

[UxmlElement]
public partial class PlayingAreaElement : VisualElement
{
    static int _cardCount = 3;

    internal void AddCard(int index, CardElement card)
    {
        var pos = card.worldTransform.GetPosition();
        Add(card);

        card.MoveTo(card.WorldToLocal(pos));
        //card.MoveAndScaleIntoPosition(index, ComputeCardPosition(index));
    }

    public Vector2 CalculatePosAndSpeed(int cardId) => 
        new Vector2((10f + cardId * (layout.width - 2 * 10f) / _cardCount), 0);
}
