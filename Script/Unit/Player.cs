using Godot;

public class Player : Unit
{

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("move_forward"))
        {
            CurrentSpeed += acceleration * GetProcessDeltaTime();
        }
        else if (@event.IsActionPressed("move_backward"))
        {
            CurrentSpeed -= acceleration * GetProcessDeltaTime();
        }
    }
}