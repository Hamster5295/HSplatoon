using Godot;

public class Brush : PrimaryWeapon
{
    [Export] public float longPressLength, brushInkCost, brushSpeedBoost = 0.2f;
    [Export] public int brushRadius;

    private Unit u;

    private bool isBrushing = false;
    private float brushTimer = 0, brushCDTimer = 0.1f;

    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(Init));
    }

    private void Init()
    {
        u = Host.Host;
        u.Connect(nameof(Unit.OnDead), this, nameof(StopBrush));
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
            u.IsDestructive = true;
        }

        if (isBrushing)
        {
            brushCDTimer += delta;
            if (brushCDTimer >= 0.1f)
            {
                brushCDTimer = 0;

                if (u.Ink >= brushInkCost)
                {
                    u.ApplyBuff("Brush", BuffType.Speed, brushSpeedBoost, -1);
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
        u.IsDestructive = false;
        Host.IsRotationLocked = false;
    }
}
