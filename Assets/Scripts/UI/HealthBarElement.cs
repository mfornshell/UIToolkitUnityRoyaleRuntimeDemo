using System;
using Unity.Properties;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using UnityEngine;
using System.Linq;
using UnityEditor;

[UxmlElement]
public partial class HealthBarElement : VisualElement
{
    VisualElement _fill;

    float _maxHealth;

    [UxmlAttribute, CreateProperty]
    public float MaxHealth
    {
        get => _maxHealth; set
        {
            _maxHealth = value;
            //OnHealthChanged();
        }
    }
    [UxmlAttribute, CreateProperty] public float CurrentHealth { get; set; }


    public HealthBarElement()
    {
        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
    }


    void OnGeometryChanged(GeometryChangedEvent evt)
    {
        
    }

    void OnHealthChanged()
    {
        //if (_fill == null)
        //    _fill = this.Q("Fill");
        //_fill.style.width = (CurrentHealth / MaxHealth) * 100;
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void RegisterConverters()
    {
        var group = new ConverterGroup("HealthBar Converters");
        group.AddConverter((ref bool b) => b ? new StyleEnum<Visibility>(Visibility.Visible) 
                                             : new StyleEnum<Visibility>(Visibility.Hidden));
        group.AddConverter((ref float f) => new StyleLength { value = new Length(f, LengthUnit.Percent) });
        ConverterGroups.RegisterConverterGroup(group);
    }
}
