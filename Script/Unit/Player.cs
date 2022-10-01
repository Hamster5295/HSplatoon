using Godot;

public class Player : Unit
{
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("move_forward"))
        {
            CurrentSpeed += acceleration * delta;
        }
        else if (Input.IsActionPressed("move_backward"))
        {
            CurrentSpeed -= acceleration * delta;
        }

        if (Input.IsActionPressed("move_left"))
        {
            Rotate(-rotateSpeed * delta);
        }
        if (Input.IsActionPressed("move_right"))
        {
            Rotate(rotateSpeed * delta);
        }

        base._Process(delta);
    }
}