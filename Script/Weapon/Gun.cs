using Godot;

public class Gun : Component<Weapon>
{
    [Export] public float cd;

    private float timer;

    public override void _Ready()
    {
        base._Ready();

        Host.type = WeaponType.Gun;
        Host.Connect(nameof(Weapon.OnUseBegin), this, nameof(OnUseBegin));
        Host.Connect(nameof(Weapon.OnUseStay), this, nameof(OnUseStay));
    }

    public void OnUseStay(float delta)
    {
        timer += delta;
        if (timer >= cd)
        {
            timer = 0;
            Host.Fire();
        }
    }

    public void OnUseBegin()
    {
        timer = cd;
    }
}