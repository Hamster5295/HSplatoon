using Godot;

public class Controller : Component<Unit>
{

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("move_up"))
        {
            host.ChangeDirection(Vector2.Up);
            host.ApplyAccel(Vector2.Up, delta);
        }
        else if (Input.IsActionPressed("move_down"))
        {
            host.ChangeDirection(Vector2.Down);
            host.ApplyAccel(Vector2.Down, delta);
        }
        if (Input.IsActionPressed("move_left"))
        {
            host.ChangeDirection(Vector2.Left);
            host.ApplyAccel(Vector2.Left, delta);
        }
        if (Input.IsActionPressed("move_right"))
        {
            host.ChangeDirection(Vector2.Right);
            host.ApplyAccel(Vector2.Right, delta);
        }

        host.LookAt(CameraInstance.current.Position - GetViewportRect().Size / 2 + GetViewport().GetMousePosition());
        host.Rotate(Mathf.Tau / 4);
    }
}