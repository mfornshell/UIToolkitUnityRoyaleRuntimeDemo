using System;

public class ButtonCommand
{
    Action _onClick;

    bool _hasClicked;

    public bool HasClicked
    {
        get => _hasClicked;
        set
        {
            _hasClicked = value;
            if (_hasClicked)
            {
                Execute();
                _hasClicked = false;
            }
        }
    }

    public ButtonCommand(Action onClick) => _onClick = onClick;

    public void Execute() => _onClick?.Invoke();

    //public static implicit operator Action(ButtonCommand command) => command.Execute;
}
