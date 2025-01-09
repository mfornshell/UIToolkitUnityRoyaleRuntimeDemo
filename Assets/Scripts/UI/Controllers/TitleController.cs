using System;
using UnityEngine.UIElements;

public class TitleController : UIController<TitleScreenVM>
{
    public event Action StartPressed;
    public event Action OptionsPressed;
    public event Action AboutPressed;

    private void Start()
    {
        viewModel.StartCommand = new ButtonCommand(OnStartPressed);
        viewModel.OptionsCommand = new ButtonCommand(OnOptionsPressed);
        viewModel.AboutCommand = new ButtonCommand(OnAboutPressed);

        var root = view.rootVisualElement;

        root?.Q("start")?.RegisterCallback<ClickEvent>(ev => viewModel.StartCommand.Execute());
        root?.Q("options")?.RegisterCallback<ClickEvent>(ev => viewModel.OptionsCommand.Execute());
        root?.Q("about")?.RegisterCallback<ClickEvent>(ev => viewModel.AboutCommand.Execute());
    }

    public void OnStartPressed()
    {
        StartPressed?.Invoke();
    }
    public void OnOptionsPressed()
    {
        OptionsPressed?.Invoke();
        Hide();
    }
    public void OnAboutPressed()
    {
        AboutPressed?.Invoke();
        Hide();
    }
}
