using Godot;
using System.Collections.Generic;

public class Bullet : Area2D
{
    [Export] public float speed;

    private float damage, range, debuff;
    private int colorSpread;

    private Unit owner;

    private float timer = 0, travelled = 0;

    public Bullet Init(Weapon weapon)
    {
        owner = weapon.Host;

        Position = weapon.GetHead();
        GlobalRotation = weapon.GlobalRotation;

        damage = weapon.damage;
        debuff = weapon.speedDecrease;
        range = weapon.range;
        colorSpread = weapon.spread;

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
