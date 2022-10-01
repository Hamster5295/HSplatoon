using Godot;

public class CameraInstance : Component<Camera2D>
{
    public static Camera2D current;

    public override void _Ready()
    {
        base._Ready();
        if (host.Current) current = host;
    }
}