using Godot;
using System.Collections.Generic;

public class Weapon : Node2D
{
    private Sprite sprite;
    private Tween tween;

    private List<Position2D> heads = new List<Position2D>();

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        tween = GetNode<Tween>("Tween");

        foreach (var item in GetNode<Node2D>("Heads").GetChildren())
        {
            if (item is Position2D p) heads.Add(p);
        }
    }
}
