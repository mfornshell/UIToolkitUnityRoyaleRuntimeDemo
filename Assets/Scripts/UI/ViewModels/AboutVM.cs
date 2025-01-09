using UnityEngine;

[CreateAssetMenu(fileName = "AboutVM", menuName = "ViewModels/AboutVM")]
public class AboutVM : ViewModel
{
    public override long GetViewHashCode()
    {
        return GetHashCode();
    }
}
