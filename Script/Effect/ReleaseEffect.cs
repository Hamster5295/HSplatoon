using Godot;

public class ReleaseEffect : Node2D
{
    [Export] public PackedScene effect;
    [Export] public float scale = 1;

    private Node2D parent;

    public override void _Ready()
    {
        var p = GetParent();

        switch (p)
        {
            case Unit u:
                Modulate = TeamUtils.GetColor(u.team);
                parent = u;
                break;

            case Bullet b:
                Modulate = TeamUtils.GetColor(b.UnitOwner.team);
                parent = b.UnitOwner;
                break;
        }
    }

    public override void _ExitTree()
    {
        var e = effect.Instance<Particles2D>();
        e.GlobalPosition = GlobalPosition;
        e.Scale *= scale;
        e.Modulate = Modulate;
        parent.GetParent().AddChild(e);
        e.Restart();
    }
}