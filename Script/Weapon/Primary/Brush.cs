using Godot;
using System;

public class Brush : PrimaryWeapon
{
    [Export] public float longPressLength;

    private bool isBrushing = false;
    private float brushTimer = 0;

    public override void OnUseBegin()
    {
        brushTimer = 0;
    }

    public override void OnUseEnd()
    {
        if (timer <= longPressLength)
        {
            Fire();
        }

        isBrushing = false;
    }

    public override void OnUseStay(float delta)
    {
        brushTimer += delta;

        if (timer > longPressLength)
        {
            isBrushing = true;
        }

        if (isBrushing)
        {
            
        }
    }
}
