using Godot;
using System.Collections.Generic;

public class Weapon : Component<Unit>
{
    [Signal] public delegate void OnUseBegin();
    [Signal] public delegate void OnUseStay(float delta);
    [Signal] public delegate void OnUseEnd();

    [Export] public string weaponName;
    [Export] public PackedScene bullet;
    [Export] public WeaponType type;
    [Export] public float damage, speedDecrease, range, inkCost, recoil;

    private Node2D parent_bullet;
    private Sprite sprite;
    private Tween tween;

    private int headIndex = 0;

    private List<Position2D> heads = new List<Position2D>();

    public List<Position2D> Heads { get => heads; private set => heads = value; }

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<Sprite>("Sprite");
        tween = GetNode<Tween>("Tween");
        parent_bullet = Host.GetNode<Node2D>("..");

        foreach (var item in GetNode<Node2D>("Heads").GetChildren())
        {
            if (item is Position2D p) Heads.Add(p);
        }
    }

    public void Fire()
    {
        if (Host.Ink < inkCost) return;
        Host.Ink -= inkCost;

        headIndex++;
        if (headIndex >= heads.Count) headIndex = 0;
        parent_bullet.AddChild(bullet.Instance<Bullet>().Init(this));

        tween.StopAll();
        tween.InterpolateProperty(this, "position:y", recoil, 0, 0.2f);
        tween.Start();
    }

    public Vector2 GetHead()
    {
        return heads[headIndex].GlobalPosition;
    }

    public Vector2 GetDirection()
    {
        return Vector2.Up.Rotated(GlobalRotation);
    }

    public void HandleBegin()
    {
        EmitSignal(nameof(OnUseBegin));
    }

    public void HandleStay(float delta)
    {
        EmitSignal(nameof(OnUseStay), delta);
    }

    public void HandleEnd()
    {
        EmitSignal(nameof(OnUseEnd));
    }
}

public enum WeaponType
{
    Gun, Bucket, Brush
}
