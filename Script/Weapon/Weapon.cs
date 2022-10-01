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
    [Export] public float damage;

    private Sprite sprite;
    private Tween tween;

    private List<Position2D> heads = new List<Position2D>();

    public List<Position2D> Heads { get => heads; private set => heads = value; }

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        tween = GetNode<Tween>("Tween");

        foreach (var item in GetNode<Node2D>("Heads").GetChildren())
        {
            if (item is Position2D p) Heads.Add(p);
        }
    }

    public void Fire()
    {

    }

    public Vector2 GetDirection()
    {
        return Vector2.Up.Rotated(GlobalRotation);
    }
}

public enum WeaponType
{
    Gun, Bucket, Brush
}
