using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityRoyale;

[UxmlElement]
public partial class DeckPileElement : VisualElement
{
    static float _scale = 0.1f;
    static float _animateScale = 0.7f;
    static float _animateDuration = 0.2f;
    static Vector2 _previewPosition = new Vector2(10f, 10f);

    public CardElement Card { get; private set; }

    public void AddCard(CardElement cardElement)
    {
        Add(cardElement);
        Card = cardElement;

        cardElement.MoveTo(_previewPosition);
        cardElement.Scale(_scale);
        cardElement.AnimatedScale(_animateScale, _animateDuration);
    }
}
