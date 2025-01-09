using System;

public class ButtonCommand
{
    Action _onClick;

    public ButtonCommand(Action onClick) => _onClick = onClick;

    public void Execute() => _onClick?.Invoke();

    //public static implicit operator Action(ButtonCommand command) => command.Execute;
}
