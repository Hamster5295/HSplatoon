using Godot;
using System;

public class Bullet : Area2D
{
    public Bullet Init(Vector2 position, float rotation)
    {
        Position = position;
        GlobalRotation = rotation;
        return this;
    }
}
