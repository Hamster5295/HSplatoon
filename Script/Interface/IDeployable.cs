using Godot;

public interface IDeployable
{
    void Deploy(Vector2 pos, Team t);
    Node2D GetNode();
}