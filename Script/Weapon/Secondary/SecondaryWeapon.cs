using Godot;

public class SecondaryWeapon : WeaponComponent
{
    [Export] public int count = 1;

    public override void _Ready()
    {
        base._Ready();

        Host.Connect(nameof(Weapon.OnUseSecondary), this, nameof(OnUseSecondary));
        Host.Connect(nameof(Weapon.OnActivateSecondary), this, nameof(OnActivateSecondary));
    }

    public virtual void OnUseSecondary()
    {
        Fire();
    }

    public virtual void OnActivateSecondary()
    {
        Host.SetState(WeaponState.Secondary, count);
    }
}