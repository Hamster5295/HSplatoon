using Godot;

public class Gun : Component<Weapon>
{
    [Export] public float cd;

    private float timer;

    public override void _Ready()
    {
        base._Ready();

        host.type = WeaponType.Gun;
        host.Connect(nameof(Weapon.OnUseStay), this, nameof(OnUseStay));
    }

    public void OnUseStay(float delta)
    {
        timer += delta;
        if (timer >= cd)
        {
            timer = 0;
            host.Fire();
        }
    }
}