using System;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityRoyale
{
    [CreateAssetMenu(fileName = "HealthBarVM", menuName = "ViewModels/HealthBarVM")]
    public class HealthBarVM : ViewModel
    {
        [SerializeField, HideInInspector] float _fillAmount;

        [CreateProperty] public Observer<bool> IsHidden { get; set; } = new();
        [field: SerializeField, CreateProperty] public Observer<float> OriginalHealth { get; set; } = new(200f);
        [field: SerializeField, CreateProperty] public Observer<float> CurrentHealth { get; set; } = new();
        [field: SerializeField][CreateProperty] public Color BarColor { get; set; } = new();

        private void OnEnable()
        {
            OriginalHealth.Value = 50f;
            CurrentHealth.Value = 50f;

            OriginalHealth.OnValueChanged += (float f) => UpdateFill();
            CurrentHealth.OnValueChanged += (float f) => UpdateFill();
            UpdateFill();
        }

        private void OnValidate()
        {
            //if (_fillAmount < 0f) _fillAmount.Value = 0f;
            //if (_fillAmount > 100f) _fillAmount.Value = 100f;
            if (CurrentHealth.Value > OriginalHealth.Value) CurrentHealth.Value = OriginalHealth.Value;
            if (CurrentHealth.Value < 0f) CurrentHealth.Value = 0f;
            UpdateFill();
        }

        public void Initialize(float health, Color barColor)
        {
            (OriginalHealth.Value, CurrentHealth.Value) = (health, health);
            BarColor = barColor;
            IsHidden.Value = true;

            
            UpdateFill();
            
        }

        void UpdateFill()
        {
            _fillAmount = (CurrentHealth.Value / OriginalHealth.Value) * 100;
        }


        public override long GetViewHashCode() => 
            HashCode.Combine(IsHidden.Value, OriginalHealth.Value, CurrentHealth.Value, BarColor, _fillAmount);

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        public static void RegisterConverters()
        {
            var group = new ConverterGroup("Float to Fill");
            group.AddConverter<float, StyleLength>(FromFloat);
            ConverterGroups.RegisterConverterGroup(group);
        }

        static StyleLength FromFloat(ref float value)
        {
            var s =  new StyleLength { value = new Length(value, LengthUnit.Percent) };
            return s;
        }

    }
}
