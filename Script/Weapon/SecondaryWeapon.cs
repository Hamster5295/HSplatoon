public abstract class SecondaryWeapon : Component<Weapon>
{
    public override void _Ready()
    {
        base._Ready();

        Host.Connect(nameof(Weapon.OnUseSecondary), this, nameof(OnUseSecondary));
    }

    public abstract void OnUseSecondary(float delta);
}