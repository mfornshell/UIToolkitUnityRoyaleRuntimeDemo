using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Observer<T> where T : struct
{
    [SerializeField] T _value;

    public event Action<T> OnValueChanged;

    [CreateProperty]
    public T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value))
                return;
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }

    [CreateProperty] public string Get => _value.ToString();

    public Observer() { }

    public Observer(T value)
    {
        _value = value;
    }

    public void Update(T value) => Value = value;

    public static implicit operator Action<T>(Observer<T> observer) => observer.Update;
    public static implicit operator T(Observer<T> observer) => observer.Value;

    protected static string Convert(ref Observer<T> value) => value.Get;

    internal void Clear()
    {
        (_value, OnValueChanged) = (default, default);
    }
}

public class IntObserver : Observer<int>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RegisterConverter() => ConverterGroups.RegisterGlobalConverter<Observer<int>, string>(Convert);
}
public class BoolObserver : Observer<bool>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RegisterConverter() => ConverterGroups.RegisterGlobalConverter<Observer<bool>, string>(Convert);
}
public class FloatObserver : Observer<float>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RegisterConverter() => ConverterGroups.RegisterGlobalConverter<Observer<float>, string>(Convert);
}
