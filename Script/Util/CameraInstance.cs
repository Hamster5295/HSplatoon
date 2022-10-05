using Godot;

public class CameraInstance : Component<Camera2D>
{
    public static Camera2D instance;

    public override void _Ready()
    {
        base._Ready();
        if (Host.Current) instance = Host;
    }
}