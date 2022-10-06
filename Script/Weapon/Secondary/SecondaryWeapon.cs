using Godot;

public class SecondaryWeapon : WeaponComponent
{
    [Export] public int count = 1;

    public override void _Ready()
    {
        base._Ready();

        Host.Connect(nameof(Weapon.OnUseSecondary), this, nameof(OnUseSecondary));
        Host.Connect(nameof(Weapon.OnActivateSecondary), this, nameof(OnActivateSecondary));

        Host.secondaryWeapon = this;
    }

    public virtual void OnUseSecondary()
    {
        Fire();
    }

    public virtual void OnActivateSecondary()
    {
        if (Host.Host.Ink < inkCost) return;
        Host.SetState(WeaponState.Secondary, count);
    }
}