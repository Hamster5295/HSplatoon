using Godot;
using System;

public class Bomb : Component<Bullet>
{
    private Area2D area;

    public override void _Ready()
    {
        base._Ready();

        area = GetNode<Area2D>("Area");
        area.GetNode<CollisionShape2D>("Col").Shape.Set("radius", HMap.GetSize() * Host.Host.spread);

        Host.Connect(nameof(Bullet.OnDestory), this, nameof(OnDestory));
    }

    private void OnDestory()
    {
        foreach (var i in area.GetOverlappingBodies())
        {
            if (i is Unit u)
            {
                if (u.team != Host.UnitOwner.team)
                {
                    u.TakeDamage(Host.Damage);
                }
            }
        }
    }
}
