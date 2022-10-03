using Godot;
using System.Collections.Generic;

public class Bullet : Area2D
{
    [Signal] public delegate void OnMove(float delta);
    [Signal] public delegate void OnDestory();

    [Export] public float speed;

    private float damage, range, debuff;
    private int colorSpread;

    private Unit owner;

    private float timer = 0, travelled = 0;

    public float Range { get => range; private set => range = value; }
    public int ColorSpread { get => colorSpread; private set => colorSpread = value; }
    public Unit UnitOwner { get => owner; private set => owner = value; }

    public Bullet Init(Weapon weapon)
    {
        UnitOwner = weapon.Host;

        Position = weapon.GetHead();
        GlobalRotation = weapon.GlobalRotation;

        damage = weapon.damage;
        debuff = weapon.speedDecrease;
        Range = weapon.range;
        ColorSpread = weapon.spread;

        Modulate = weapon.Host.Color;

        Connect("body_entered", this, nameof(OnBodyEntered));
        return this;
    }

    public override void _Process(float delta)
    {
        var deltaDistance = speed * delta;
        Translate(Vector2.Up.Rotated(Rotation) * deltaDistance);
        travelled += deltaDistance;

        EmitSignal(nameof(OnMove), deltaDistance);

        if (travelled >= Range) Release();
    }

    private void OnBodyEntered(Node n)
    {
        if (n == UnitOwner) return;

        Release();

        if (n is Unit u)
        {
            u.TakeDamage(damage, debuff);
        }
    }

    public void Release()
    {
        EmitSignal(nameof(OnDestory));
        QueueFree();
    }
}
