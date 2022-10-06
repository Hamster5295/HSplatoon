using Godot;

public class CombatLogo : Control
{
    [Signal] public delegate void OnAnimationFinished();

    [Export] public float speed;

    private Tween tween;
    private TextureRect logo, ready, go;

    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");

        logo = GetNode<TextureRect>("StartImg");
        ready = GetNode<TextureRect>("ReadyImg");
        go = GetNode<TextureRect>("GoImg");

        tween.Connect("tween_all_completed", this, nameof(OnFinished));
    }

    public void Start()
    {
        tween.StopAll();
        tween.InterpolateProperty(logo, "modulate:a", 1, 0, speed, delay: speed * 2);
        tween.InterpolateProperty(ready, "modulate:a", 0, 1, speed / 2, delay: speed * 3);
        tween.InterpolateProperty(ready, "modulate:a", 1, 0, speed / 4, delay: speed * 4.5f);
        tween.InterpolateProperty(ready, "rect_scale", Vector2.One, Vector2.One * 1.3f, speed / 4, delay: speed * 4.5f);
        tween.InterpolateProperty(go, "modulate:a", 0, 1, speed / 2, delay: speed * 5f);
        tween.InterpolateProperty(go, "modulate:a", 1, 0, speed / 4, delay: speed * 6f);
        tween.InterpolateProperty(go, "rect_scale", Vector2.One, Vector2.One * 1.3f, speed / 4, delay: speed * 6f);
        tween.InterpolateCallback(this, speed * 6.25f, "hide");
        tween.Start();
    }

    private void OnFinished()
    {
        EmitSignal(nameof(OnAnimationFinished));
    }
}
