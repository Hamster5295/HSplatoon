using Godot;
using System.Collections.Generic;

public class Bullet : Area2D
{
    [Export] public float damage, speed, lifeTime, debuff;
    [Export] public int colorSpread;

    private float timer = 0;

    public Bullet Init(Weapon weapon)
    {
        Position = weapon.GetHead();
        GlobalRotation = weapon.GlobalRotation;
        debuff = weapon.speedDecrease;

        Modulate = weapon.Host.color;

        Connect("body_entered", this, nameof(OnBodyEntered));
        return this;
    }

    public override void _Process(float delta)
    {
        Translate(Vector2.Up.Rotated(Rotation) * speed * delta);
    }

    public override void _ExitTree()
    {
        SpreadColor(colorSpread);
    }

    private void OnBodyEntered(Node n)
    {
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
