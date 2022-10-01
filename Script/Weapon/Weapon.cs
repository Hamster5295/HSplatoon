using Godot;
using System;

public class Weapon : Node2D
{
    private Sprite sprite;
    private Tween tween;

    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        tween = GetNode<Tween>("Tween");
    }
}
