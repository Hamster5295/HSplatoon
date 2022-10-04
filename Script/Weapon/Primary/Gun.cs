using Godot;

public class Gun : PrimaryWeapon
{
    private bool isUsing = false;

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
            Fire();
        }
    }

    public override void OnUseBegin()
    {
        isUsing = true;
        if (timer >= cd)
        {
            timer = 0;
            Fire();
        }
    }

    public override void OnUseEnd()
    {
        isUsing = false;
    }
}