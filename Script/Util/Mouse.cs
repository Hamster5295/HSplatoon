using Godot;

public class Mouse : Node2D
{
    private static Mouse instance;

    public override void _Ready()
    {
        instance = this;
    }

    public static Vector2 GetGlobalPos()
    {
        return instance.GetGlobalPosInternal();
    }

    private Vector2 GetGlobalPosInternal()
    {
        return CameraInstance.current.Position - GetViewportRect().Size / 2 + GetViewport().GetMousePosition();
    }
}