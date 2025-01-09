using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityRoyale;

public class AboutController : UIController<AboutVM>
{
    AboutScreenOperator _operator;

    public event Action BackPressed;

    void Start()
    {
        var _aboutView = view.rootVisualElement;
        _operator = _aboutView.Q<AboutScreenOperator>();

        _aboutView?.Q("back-button")?.RegisterCallback<ClickEvent>(ev => OnBackPressed());
    }

    public override void Show()
    {
        base.Show();
        _operator.Animate();
    }

    public void OnBackPressed()
    {
        BackPressed?.Invoke();
        Hide();
    }
}
