using Godot;

public class Component<T> : Node2D where T : Node2D
{
    private T host;

    public T Host { get => host; private set => host = value; }

    public override void _Ready()
    {
        string path = "..";
        while (GetNodeOrNull<T>(path) == null)
        {
            path += "/..";
        }
        Host = GetNode<T>(path);
    }
}