using Godot;

public class Component<T> : Node2D where T : Node2D
{
    protected T host;

    public override void _Ready()
    {
        string path = "..";
        while (GetNodeOrNull<T>(path) == null)
        {
            path += "/..";
        }
        host = GetNode<T>(path);
    }
}