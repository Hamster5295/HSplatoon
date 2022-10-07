using Godot;
using System;

public class HPBar : ProgressBar
{
    [Export] public float animateSpeed, showLength;

    private Unit player;
    private Tween tween;

    private bool isShowing = false;

    private float timer = 0;

    public override void _Ready()
    {
        CallDeferred(nameof(InitPlayer));

        tween = GetNode<Tween>("Tween");

        SelfModulate = new Color(1, 1, 1, 0);
    }

    public override void _Process(float delta)
    {
        if (timer > 0)
        {
            timer -= delta;
            if (timer <= 0 && isShowing)
            {
                isShowing = false;
                tween.StopAll();
                tween.InterpolateProperty(this, "self_modulate:a", 1, 0, animateSpeed);
                tween.Start();
            }
        }
    }

    private void InitPlayer()
    {
        player = Controller.instance.Host;

        player.Connect(nameof(Unit.OnHPChanged), this, nameof(OnHPChanged));
    }

    private void OnHPChanged(float hp, float maxHP)
    {
        // GD.Print("gg");
        tween.StopAll();
        timer = showLength;
        isShowing = true;

        Value = MaxValue * hp / maxHP;
        SelfModulate = Colors.White;
    }
}
