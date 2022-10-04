using Godot;
using System;

public class Rotater : Node2D
{
    [Export] public float rotateSpeed;

    public override void _Process(float delta)
    {
        Rotate(rotateSpeed * delta);
    }
}
