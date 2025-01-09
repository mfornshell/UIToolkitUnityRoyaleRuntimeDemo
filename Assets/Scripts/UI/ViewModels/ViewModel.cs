using UnityEngine;
using UnityEngine.UIElements;

public abstract class ViewModel : ScriptableObject, IDataSourceViewHashProvider
{
    public abstract long GetViewHashCode();
}
