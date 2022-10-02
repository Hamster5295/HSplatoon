using Godot;
using System.Collections.Generic;

public class Bullet : Area2D
{
    [Export] public float damage, speed, debuff;

    public Bullet Init(Weapon weapon)
    {
        Position = weapon.GetHead();
        GlobalRotation = weapon.GlobalRotation;
        debuff = weapon.speedDecrease;
        Connect("body_entered", this, nameof(OnBodyEntered));
        return this;
    }

    public override void _Process(float delta)
    {
        Translate(Vector2.Up.Rotated(Rotation) * speed * delta);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    private void OnBodyEntered(Node n)
    {
        QueueFree();

        if (n is Unit u)
        {
            u.TakeDamage(damage, debuff);
        }
    }
}
