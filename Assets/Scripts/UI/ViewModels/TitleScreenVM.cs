using UnityEngine;

[CreateAssetMenu(fileName = "TitleScreenVM", menuName = "ViewModels/TitleScreenVM")]
public class TitleScreenVM : ViewModel
{
    public ButtonCommand StartCommand { get; set; }
    public ButtonCommand OptionsCommand { get; set; }
    public ButtonCommand AboutCommand { get; set; }

    public override long GetViewHashCode()
    {
        return GetHashCode();
    }
}
