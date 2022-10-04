using Godot;

public class WeaponComponent : Component<Weapon>
{
    [Export] public PackedScene bullet;
    [Export] public float damage, range, inkCost, arc;
    [Export] public int spread;

    protected void Fire()
    {
        if (Host.Host.Ink < inkCost) return;
        Host.Host.Ink -= inkCost;

        Host.Fire(bullet.Instance<Bullet>().Init(this));
    }
}