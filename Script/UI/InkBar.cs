using Godot;

public class InkBar : ProgressBar
{
    [Export] public float animationSpeed;

    private Tween tween;
    private Unit player;
    private ColorRect line;

    private bool isShowing = false;


    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");
        line = GetNode<ColorRect>("Line");

        Modulate = new Color(1, 1, 1, 0);

        CallDeferred(nameof(ConfigurePlayer));
    }

    private void ConfigurePlayer()
    {
        player = Controller.instance.Host;
        player.Connect(nameof(Unit.OnInkChanged), this, nameof(OnInkChanged));
        player.Connect(nameof(Unit.OnDive), this, nameof(OnDive));
        player.Connect(nameof(Unit.OnLand), this, nameof(OnLand));

        SelfModulate = player.Color;

        var sec = player.Weapon.secondaryWeapon;
        line.RectPosition = Vector2.Right * (RectSize.y * (1 - sec.inkCost / player.maxInk));

        Value = MaxValue * player.Ink / player.maxInk;
    }

    private void OnInkChanged(float ink, float maxInk)
    {
        Value = MaxValue * ink / maxInk;
    }

    private void OnDive()
    {
        if (isShowing) return;
        isShowing = true;

        tween.StopAll();
        tween.InterpolateProperty(this, "modulate:a", 0, 1, animationSpeed);
        tween.Start();
    }

    private void OnLand()
    {
        if (!isShowing) return;
        isShowing = false;

        tween.StopAll();
        tween.InterpolateProperty(this, "modulate:a", 1, 0, animationSpeed);
        tween.Start();
    }
}
