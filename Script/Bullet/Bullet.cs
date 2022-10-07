using Godot;
using System.Collections.Generic;

public class Bullet : Area2D
{
    [Signal] public delegate void OnMove(float delta);
    [Signal] public delegate void OnDestory();

    [Export] public float speed, speedDecrease, lifeTime = -1;

    private float damage, range, deltaAngle, lifeTimer = 0;
    private int colorSpread;

    private Unit owner;
    private WeaponComponent host;

    private float timer = 0, travelled = 0;

    public float Range { get => range; private set => range = value; }
    public int ColorSpread { get => colorSpread; private set => colorSpread = value; }
    public Unit UnitOwner { get => owner; private set => owner = value; }
    public float Damage { get => damage; set => damage = value; }
    public WeaponComponent Host { get => host; set => host = value; }

    public Bullet Init(WeaponComponent weapon)
    {
        Host = weapon;
        UnitOwner = weapon.Host.Host;

        deltaAngle = Mathf.Deg2Rad(weapon.arc) * (GD.Randf() - 0.5f);

        Damage = weapon.damage;
        Range = weapon.range;
        ColorSpread = weapon.spread;

        Modulate = TeamUtils.GetLightColor(weapon.Host.Host.team);

        Connect("body_entered", this, nameof(OnBodyEntered));
        return this;
    }

    public override void _Process(float delta)
    {
        if (lifeTime == -1) return;
        if (lifeTimer < lifeTime)
        {
            lifeTimer += delta;
        }
        else Release();
    }

    public override void _PhysicsProcess(float delta)
    {
        var deltaDistance = speed * delta;
        Translate(Vector2.Up.Rotated(Rotation) * deltaDistance);
        travelled += deltaDistance;
        speed -= speedDecrease * delta;
        if (speed < 0) speed = 0;

        EmitSignal(nameof(OnMove), deltaDistance);

        if (travelled >= Range) Release();
    }

    private void OnBodyEntered(Node n)
    {
        if (n == UnitOwner) return;

        Release();

        if (n is Unit u)
        {
            if (u.team == owner.team) return;
            bool killed = u.TakeDamage(Damage);
            if (killed)
            {
                owner.OnKillEnemy();
            }
        }
    }

    public void ApplyDeltaAngle()
    {
        Rotate(deltaAngle);
    }

    public void Release()
    {
        EmitSignal(nameof(OnDestory));
        QueueFree();
    }
}
