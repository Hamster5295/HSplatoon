using Godot;

public class Controller : Component<Unit>
{

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("move_up"))
        {
            host.ApplyAccel(Vector2.Up, delta);
        }
        else if (Input.IsActionPressed("move_down"))
        {
            host.ApplyAccel(Vector2.Down, delta);
        }
        if (Input.IsActionPressed("move_left"))
        {
            host.ApplyAccel(Vector2.Left, delta);
        }
        if (Input.IsActionPressed("move_right"))
        {
            host.ApplyAccel(Vector2.Right, delta);
        }

        host.LookAt(cam.Position - GetViewportRect().Size / 2 + GetViewport().GetMousePosition());
    }
}