using Godot;
using System.Collections.Generic;

public class Bullet : Area2D
{
    [Export] public float damage, speed, range, debuff;
    [Export] public int colorSpread;

    private Unit owner;

    private float timer = 0, travelled = 0;

    public Bullet Init(Weapon weapon)
    {
        owner = weapon.Host;

        Position = weapon.GetHead();
        GlobalRotation = weapon.GlobalRotation;
        debuff = weapon.speedDecrease;
        range = weapon.range;

        Modulate = weapon.Host.Color;

        Connect("body_entered", this, nameof(OnBodyEntered));
        return this;
    }

    public override void _Process(float delta)
    {
        var deltaDistance = speed * delta;
        Translate(Vector2.Up.Rotated(Rotation) * deltaDistance);
        travelled += deltaDistance;

        if (travelled >= range) QueueFree();
    }

    public override void _ExitTree()
    {
        // SpreadColor(colorSpread);
    }

    private void OnBodyEntered(Node n)
    {
        if (n == owner) return;

        QueueFree();

        if (n is Unit u)
        {
            u.TakeDamage(damage, debuff);
        }
    }

    public void SpreadColor(int spread)
    {

    }
}
