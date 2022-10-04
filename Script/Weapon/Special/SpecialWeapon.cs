using Godot;

public class SpecialWeapon : WeaponComponent
{
    [Export] public float length;
    protected bool isActive = false;
    protected float lengthTimer = 0;

    public override void _Ready()
    {
        base._Ready();

        Host.Connect(nameof(Weapon.OnUseSpecial), this, nameof(OnUseSpecial));
        Host.Connect(nameof(Weapon.OnActivateSpecial), this, nameof(OnActivateSpecial));
    }

    public override void _Process(float delta)
    {
        if (lengthTimer > 0)
        {
            lengthTimer -= delta;
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
        GD.Print(Host.Host.Energy);
        if (Host.Host.Energy < 100) return;
        Host.Host.Energy -= 100;

        Host.SetState(WeaponState.Special, -1);
        lengthTimer = length;
    }
}