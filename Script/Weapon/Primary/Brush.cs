using Godot;
using System;

public class Brush : PrimaryWeapon
{
    [Export] public float longPressLength, brushInkCost;
    [Export] public int brushRadius;

    private Unit u;

    private bool isBrushing = false;
    private float brushTimer = 0, brushCDTimer = 0.1f;

    public override void _Ready()
    {
        base._Ready();
        u = Host.Host;
    }

    public override void _Process(float delta)
    {
        if (timer < cd) timer += delta;
    }

    public override void OnUseBegin()
    {
        brushTimer = 0;
        if (u == null) u = Host.Host;
    }

    public override void OnUseEnd()
    {
        if (timer >= cd && brushTimer <= longPressLength)
        {
            Fire();
            timer = 0;
            return;
        }

        StopBrush();
    }

    public override void OnUseStay(float delta)
    {
        brushTimer += delta;

        if (brushTimer > longPressLength)
        {
            isBrushing = true;
            Host.IsRotationLocked = true;
            Host.Rotation = 0;
        }

        if (isBrushing)
        {
            brushCDTimer += delta;
            if (brushCDTimer >= 0.1f)
            {
                brushCDTimer = 0;

                if (u.Ink >= brushInkCost * delta)
                {
                    u.ApplyBuff("Brush", BuffType.Speed, 0.5f, -1);
                    HMap.ClaimCircle(GlobalPosition, brushRadius, u.team, true);
                    u.Ink -= brushInkCost * delta;
                }
                else
                {
                    StopBrush();
                }

            }
        }
    }

    private void StopBrush()
    {
        isBrushing = false;
        u.RemoveBuff("Brush");
        Host.IsRotationLocked = false;
    }
}
