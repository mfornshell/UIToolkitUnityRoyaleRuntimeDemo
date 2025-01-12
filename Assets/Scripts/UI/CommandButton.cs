using Unity.Properties;
using UnityEngine.UIElements;

[UxmlElement]
public partial class CommandButton : Button
{
    [UxmlAttribute, CreateProperty] public bool HasClicked { get; set; }

    public CommandButton() => clicked += OnClick;

    private void OnClick()
    {
        HasClicked = true;
        NotifyPropertyChanged(nameof(HasClicked));
    }
}

