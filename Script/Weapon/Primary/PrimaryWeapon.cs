public abstract class PrimaryWeapon : WeaponComponent
{
    public override void _Ready()
    {
        base._Ready();

        Host.Connect(nameof(Weapon.OnUseBegin), this, nameof(OnUseBegin));
        Host.Connect(nameof(Weapon.OnUseEnd), this, nameof(OnUseEnd));
        Host.Connect(nameof(Weapon.OnUseStay), this, nameof(OnUseStay));
    }

    public abstract void OnUseStay(float delta);

    public abstract void OnUseBegin();

    public abstract void OnUseEnd();
}