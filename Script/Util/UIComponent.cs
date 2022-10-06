using Godot;

public class UIComponent<T> : Control where T : Control
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