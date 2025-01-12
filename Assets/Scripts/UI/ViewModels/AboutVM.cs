using UnityEngine;

[CreateAssetMenu(fileName = "AboutVM", menuName = "ViewModels/AboutVM")]
public class AboutVM : ViewModel
{
    public ButtonCommand BackCommand { get; set; }

    public override long GetViewHashCode()
    {
        return GetHashCode();
    }
}
