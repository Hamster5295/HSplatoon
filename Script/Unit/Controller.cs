using Godot;

public class Controller : Component<Unit>
{
    private bool isMouseHolded = false;

    public override void _UnhandledInput(InputEvent @event)
    {
        if(Host.State != UnitState.Normal) return;

        if (@event is InputEventMouseButton e)
        {
            if (e.ButtonIndex != (int)ButtonList.Left) return;

            if (e.Pressed) { Host.Weapon.HandleBegin(); isMouseHolded = true; }
            else { Host.Weapon.HandleEnd(); isMouseHolded = false; }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Host.State != UnitState.Normal) return;

        CameraInstance.instance.GlobalPosition = GlobalPosition;

        if (Input.IsActionPressed("move_up"))
        {
            Host.ChangeDirection(Vector2.Up);
            Host.ApplyAccel(Vector2.Up, delta);
        }
        if (Input.IsActionPressed("move_down"))
        {
            Host.ChangeDirection(Vector2.Down);
            Host.ApplyAccel(Vector2.Down, delta);
        }
        if (Input.IsActionPressed("move_left"))
        {
            Host.ChangeDirection(Vector2.Left);
            Host.ApplyAccel(Vector2.Left, delta);
        }
        if (Input.IsActionPressed("move_right"))
        {
            Host.ChangeDirection(Vector2.Right);
            Host.ApplyAccel(Vector2.Right, delta);
        }

        if (Input.IsActionJustPressed("move_dive"))
            Host.Dive();

        if (Input.IsActionJustReleased("move_dive"))
            Host.Land();


        Host.Weapon.LookAt(Mouse.GetGlobalPos());
        Host.Weapon.Rotate(Mathf.Tau / 4);

        if (isMouseHolded) Host.Weapon.HandleStay(delta);

        if (Input.IsActionJustPressed("use_secondary"))
        {
            Host.Weapon.HandleSecondary();
        }

        if (Input.IsActionJustPressed("use_special"))
        {
            Host.Weapon.HandleSpecial();
        }
    }
}