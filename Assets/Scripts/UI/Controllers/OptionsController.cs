using System;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsController : UIController<OptionsVM>
{
    public const string MuteMusicKey = "muteMusic";
    public const string GameSpeedKey = "gameSpeed";

    public event Action BackPressed;

    void Start()
    {
        viewModel.MuteMusic.Value = PlayerPrefs.GetInt(MuteMusicKey, 1) switch
        {
            1 => true,
            0 => false,
            _ => throw new Exception("Out of Range!")
        };
        viewModel.GameSpeed.Value = PlayerPrefs.GetInt(GameSpeedKey, 1);

        viewModel.MuteMusic.OnValueChanged += OnMuteMusic;
        viewModel.GameSpeed.OnValueChanged += OnGameSpeedChanged;

        view.rootVisualElement.Q("back-button")?.RegisterCallback<ClickEvent>(ev => OnBackPressed());
    }

    void OnMuteMusic(bool b) => PlayerPrefs.SetInt(MuteMusicKey, b ? 1 : 0);
    void OnGameSpeedChanged(int value) => PlayerPrefs.SetInt(GameSpeedKey, value);

    public void OnBackPressed()
    {
        BackPressed?.Invoke();
        Hide();
    }
}
