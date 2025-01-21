using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class CardUI : UIController<CardVM>
    {
        [SerializeField] VisualTreeAsset _cardAsset;

        CardElement _cardElement;

        public CardElement CardElement => _cardElement; //temp

        internal void Initialize(CardData cardData, VisualElement backupPanel)
        {
            _cardElement = _cardAsset.Instantiate().Q<CardElement>();
            backupPanel.Add(_cardElement);
            _cardElement.dataSource = viewModel;

            viewModel.Initialize(cardData);

            _cardElement.Init(cardData);
        }

        private void OnEnable()
        {
            viewModel = ScriptableObject.CreateInstance<CardVM>();
        }

        void Start()
        {
            
        }
    }
}
