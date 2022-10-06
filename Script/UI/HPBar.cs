using Godot;
using System;

public class HPBar : ProgressBar
{
    [Export] public float animateSpeed, showLength;

    private Unit player;
    private Tween tween;

    public override void _Ready()
    {
        SelfModulate = new Color(1, 1, 1, 0);
        CallDeferred(nameof(InitPlayer));
    }

    private void InitPlayer()
    {
        player = Controller.instance.Host;

        player.Connect(nameof(Unit.OnHPChanged), this, nameof(OnHPChanged));
    }

    private void OnHPChanged(float hp, float maxHP)
    {
        Value = MaxValue * hp / maxHP;
        SelfModulate = Colors.White;

        tween.StopAll();
        tween.InterpolateProperty(this, "modulate:a", 1, 0, animateSpeed, delay: showLength);
    }
}
