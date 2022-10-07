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

    public override void _Process(float delta)
    {
        if (timer > 0) timer -= delta;
    }

    public virtual void OnUseSecondary()
    {
        if (timer > 0) return;
        Fire();
        timer = cd;
    }

    public virtual void OnActivateSecondary()
    {
        if (Host.Host.Ink < inkCost) return;
        Host.SetState(WeaponState.Secondary, count);
    }
}