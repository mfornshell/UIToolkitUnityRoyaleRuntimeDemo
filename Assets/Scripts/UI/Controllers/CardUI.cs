using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class CardUI : UIController<CardVM>
    {
        [SerializeField] VisualTreeAsset _cardAsset;

        Action<int> IndexChanged;

        CardElement _cardElement;
        int _index;

        public CardElement CardElement => _cardElement; //temp

        public int Index
        {
            get => _index; set
            {
                _index = value; 
                IndexChanged?.Invoke(_index);
            } 
        }

        internal void Initialize(CardData cardData)
        {
            _cardElement = _cardAsset.Instantiate().Q<CardElement>();
            _cardElement.dataSource = viewModel;
            _cardElement.Init(cardData);

            viewModel.Initialize(cardData);
        }

        private void OnEnable()
        {
            viewModel = ScriptableObject.CreateInstance<CardVM>();
            IndexChanged += viewModel.Index.Update;
        }

        void Start()
        {
            
        }

        private void OnDisable()
        {
            IndexChanged -= viewModel.Index.Update;
        }

        internal IEnumerator AnimatedMove(PlayingAreaUI playingAreaUI)
        {
            var endPos = playingAreaUI.Element.CalculatePosAndSpeed(Index);
            var anim = CardElement.AnimatedMoveTo(endPos, Index);

            while (anim.isRunning)
                yield return null;
        }
    }
}
