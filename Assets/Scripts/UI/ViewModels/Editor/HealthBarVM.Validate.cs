namespace UnityRoyale
{
    public partial class HealthBarVM
    {
        private void OnValidate()
        {
            if (OriginalHealth.Value < 0) OriginalHealth.Value = 0;

            if (CurrentHealth.Value > OriginalHealth.Value) CurrentHealth.Value = OriginalHealth.Value;
            if (CurrentHealth.Value < 0f) CurrentHealth.Value = 0f;
            UpdateFill();
        }

        void Reset() => UpdateFill();
    }
}
