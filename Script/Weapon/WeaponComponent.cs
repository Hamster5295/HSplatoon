using Godot;

public class WeaponComponent : Component<Weapon>
{
    [Export] public PackedScene bullet;
    [Export] public float damage, cd, range, inkCost, arc, ammoCount = 1;
    [Export] public int spread;

    protected float timer = 0;

    protected virtual void Fire()
    {
        if (Host.Host.Ink < inkCost) return;
        Host.Host.Ink -= inkCost;

        for (int i = 0; i < ammoCount; i++)
            Host.Fire(bullet.Instance<Bullet>().Init(this));
    }
}