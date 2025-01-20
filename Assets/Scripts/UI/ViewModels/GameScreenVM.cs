using System;
using Unity.Properties;
using UnityEngine;

namespace UnityRoyale
{
    [CreateAssetMenu(fileName = "GameScreenVM", menuName = "ViewModels/GameScreenVM")]
    public class GameScreenVM : ViewModel
    {
        [CreateProperty] public Observer<bool> IsVisible { get; set; } = new(true);

        private void Reset()
        {
            IsVisible = new(true);
        }

        public override long GetViewHashCode() => HashCode.Combine(IsVisible.Value);
    }
}
