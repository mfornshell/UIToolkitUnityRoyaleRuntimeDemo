using System;
using Unity.Properties;
using UnityEngine;

namespace UnityRoyale
{
    [CreateAssetMenu(fileName = "HealthBarVM", menuName = "ViewModels/HealthBarVM")]
    public partial class HealthBarVM : ViewModel
    {
        [SerializeField, HideInInspector] float _fillAmount;
        [SerializeField] Observer<float> _maxHealth = new(100f);
        [SerializeField] Observer<float> _currentHealth = new(50f);
        [SerializeField] Observer<bool> _isVisible = new(true);

        public Observer<float> OriginalHealth => _maxHealth;
        public Observer<float> CurrentHealth => _currentHealth;
        public Observer<bool> IsVisible => _isVisible;

        [field: SerializeField][CreateProperty] public Color BarColor { get; set; } = Color.red;

        void OnEnable()
        {
            OriginalHealth.OnValueChanged += UpdateFill;
            CurrentHealth.OnValueChanged += UpdateFill;
            UpdateFill();
        }

        public void Initialize(float health, Color barColor)
        {
            (OriginalHealth.Value, CurrentHealth.Value) = (health, health);
            BarColor = barColor;
            IsVisible.Value = false;

            UpdateFill();
        }

        void UpdateFill(float _ = 0) => _fillAmount = (CurrentHealth.Value / OriginalHealth.Value) * 100;

        void OnDisable()
        {
            OriginalHealth.OnValueChanged -= UpdateFill;
            CurrentHealth.OnValueChanged -= UpdateFill;
        }

        public override long GetViewHashCode() => 
            HashCode.Combine(_maxHealth.Value, _currentHealth.Value, _isVisible.Value, BarColor, _fillAmount);
    }
}
