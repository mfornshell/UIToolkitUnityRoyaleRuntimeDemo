using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    public class HealthUIManager : MonoBehaviour
    {
        private const string HiddenHealthBarStyleClass = "health-bar--hidden";

        [SerializeField] HealthBarController _healthBarPrefab;
        [Space]
        [SerializeField] Vector3 unitAnchorPosition;
        [SerializeField] Vector3 nonUnitAnchorPosition;
        [SerializeField] Vector2 worldSize;
        [SerializeField] Color red = new Color32(252, 35, 13, 255);
        [SerializeField] Color blue = new Color32(31, 132, 255, 255);

        internal void AddHealthBar(ThinkingPlaceable p)
        {
            var healthBar = Instantiate(_healthBarPrefab, transform);
            //p.healthBar = healthBar;
            //p.HealthChanged += healthBar.OnHealthChanged;
            var anchor = p.pType == Placeable.PlaceableType.Unit ? unitAnchorPosition : nonUnitAnchorPosition;
            var color = p.faction == Placeable.Faction.Player ? red : blue;
            healthBar.Initialize(p, anchor, p.hitPoints, color);
        }

        void Start()
        {
        
        }
    }
}
