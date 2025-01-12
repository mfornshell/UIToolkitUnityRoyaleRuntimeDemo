using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityRoyale;

public class AboutController : UIController<AboutVM>
{
    AboutScreenOperator _scroll;

    public event Action BackPressed;

    void Start()
    {
        var root = view.rootVisualElement;
        _scroll = root.Q<AboutScreenOperator>();

        viewModel.BackCommand = new ButtonCommand(OnBackPressed);
        root?.Q("back-button")?.RegisterCallback<ClickEvent>(ev => viewModel.BackCommand.Execute());
    }

    public override void Show()
    {
        base.Show();
        _scroll.Animate();
    }

    public void OnBackPressed()
    {
        BackPressed?.Invoke();
        Hide();
    }
}
