using Godot;

public class Gun : Component<Weapon>
{
    [Export] public float cd;

    private float timer = 0;
    private bool isUsing = false;

    public override void _Ready()
    {
        base._Ready();

        Host.type = WeaponType.Gun;
        Host.Connect(nameof(Weapon.OnUseBegin), this, nameof(OnUseBegin));
        Host.Connect(nameof(Weapon.OnUseEnd), this, nameof(OnUseEnd));
        Host.Connect(nameof(Weapon.OnUseStay), this, nameof(OnUseStay));
    }

    public override void _Process(float delta)
    {
        if (!isUsing && timer < cd)
        {
            timer += delta;
        }
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
        isUsing = true;
        if (timer >= cd)
        {
            timer = 0;
            Host.Fire();
        }
    }

    public void OnUseEnd()
    {
        isUsing = false;
    }
}