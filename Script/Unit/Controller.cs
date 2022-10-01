using Godot;

public class Controller : Component<Unit>
{
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionPressed("move_forward"))
        {
            host.CurrentSpeed += host.acceleration * delta;
        }
        else if (Input.IsActionPressed("move_backward"))
        {
            host.CurrentSpeed -= host.acceleration * delta;
        }

        if (Input.IsActionPressed("move_left"))
        {
            host.Rotate(-host.rotateSpeed * delta);
        }
        if (Input.IsActionPressed("move_right"))
        {
            host.Rotate(host.rotateSpeed * delta);
        }
    }
}