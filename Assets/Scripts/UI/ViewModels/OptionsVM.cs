using System;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "OptionsVM", menuName = "ViewModels/OptionsVM")]
public class OptionsVM : ViewModel
{
    [CreateProperty] public Observer<bool> MuteMusic { get; set; } = new();
    [CreateProperty] public Observer<int> GameSpeed { get; set; } = new();
    public ButtonCommand BackCommand { get; set; }

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    public static void RegisterConverters()
    {
        var group = new ConverterGroup("Value to Text");
        group.AddConverter((ref bool b) => b ? "ON" : "OFF");
        ConverterGroups.RegisterConverterGroup(group);
    }

    public override long GetViewHashCode() => HashCode.Combine(MuteMusic.Value, GameSpeed.Value);
}
