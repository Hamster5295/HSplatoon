using Godot;

public class Gun : PrimaryWeapon
{
    [Export] public float cd;

    private float timer = 0;
    private bool isUsing = false;

    public override void _Ready()
    {
        base._Ready();
        Host.type = WeaponType.Gun;
    }

    public override void _Process(float delta)
    {
        if (!isUsing && timer < cd)
        {
            timer += delta;
        }
    }

    public override void OnUseStay(float delta)
    {
        timer += delta;
        if (timer >= cd)
        {
            timer = 0;
            Host.Fire();
        }
    }

    public override void OnUseBegin()
    {
        isUsing = true;
        if (timer >= cd)
        {
            timer = 0;
            Host.Fire();
        }
    }

    public override void OnUseEnd()
    {
        isUsing = false;
    }
}