using Godot;

public class SpecialWeapon : WeaponComponent
{
    [Export] public int count = -1;
    [Export] public float length;
    protected bool isActive = false;
    protected float lengthTimer = 0;

    private Unit u;

    public override void _Ready()
    {
        base._Ready();

        if (u == null) u = Host.Host;

        Host.Connect(nameof(Weapon.OnUseSpecial), this, nameof(OnUseSpecial));
        Host.Connect(nameof(Weapon.OnActivateSpecial), this, nameof(OnActivateSpecial));
        Host.specialWeapon = this;
    }

    public override void _Process(float delta)
    {
        if (length != -1)
            if (lengthTimer > 0)
            {
                lengthTimer -= delta;

                u.Energy = u.maxEnergy * lengthTimer / length;

                if (lengthTimer <= 0)
                {
                    Host.SetState(WeaponState.Primary, -1);
                }
            }

        if (timer > 0)
        {
            timer -= delta;
        }
    }

    public virtual void OnUseSpecial()
    {
        if (timer <= 0)
        {
            Fire();
            timer = cd;
        }
    }

    public virtual void OnActivateSpecial()
    {
        // GD.Print(Host.Host.Energy);
        if (u == null) u = Host.Host;

        if (u.Energy < u.maxEnergy) return;
        u.Energy -= u.maxEnergy;

        Host.SetState(WeaponState.Special, count);
        lengthTimer = length;
    }
}