using Godot;

public class SelfDestroyer : Particles2D
{
    private float timer = 0;

    public override void _Ready()
    {
        timer = Lifetime;
    }

    public override void _Process(float delta)
    {
        timer -= delta;
        if (timer <= 0)
        {
            QueueFree();
        }
    }
}
