using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class PlayingAreaUI : UIController<ViewModel>
    {
        static int _cardCount;

        public PlayingAreaElement Element { get; private set; }

        public Dictionary<int, CardUI> Cards { get; private set; } = new();

        void Start()
        {
            for (var i = 0; i < _cardCount; i++)
                Cards.Add(i, null);
        }

        public void Initialize(PlayingAreaElement element)
        {
            Element = element;
        }

        public bool CanAdd()
        {
            foreach (var card in Cards)
            {
                if (card.Value == null)
                    return true;
            }
            return false;
        }

        internal void Add(CardUI cardUI)
        {
            foreach (var card in Cards)
            {
                if (card.Value == null)
                {
                    Cards[card.Key] = cardUI;
                    card.Value.Index = card.Key;

                    Element.AddCard(card.Key, card.Value.CardElement);
                    return;
                }
            }
        }
    }
}
