using System;
using Unity.Properties;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using UnityEngine;
using System.Linq;

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
}
