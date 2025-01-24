using System;
using Unity.Properties;
using UnityEngine;

namespace UnityRoyale
{
    [CreateAssetMenu(fileName = "CardVM", menuName = "ViewModels/CardVM")]
    public class CardVM : ViewModel
    {
        [SerializeField] Sprite _cardArt;
        [SerializeField] Observer<int> _damage = new();
        [SerializeField] Observer<int> _health = new();

        public Sprite CardArt => _cardArt;
        public Observer<int> Damage => _damage;
        public Observer<int> Health => _health;

        [CreateProperty] public Observer<int> Index { get; set; } = new();
        [CreateProperty] public Observer<Vector2> Position { get; set; } = new();

        internal void Initialize(CardData cardData)
        {
            _cardArt = cardData.cardImage;
            _damage.Value = (int)cardData.placeablesData[0].damagePerAttack;
            _health.Value = (int)cardData.placeablesData[0].hitPoints;
        }
        public override long GetViewHashCode() => 
            HashCode.Combine(CardArt.GetHashCode(), _damage.Value, _health.Value);
    }
}
