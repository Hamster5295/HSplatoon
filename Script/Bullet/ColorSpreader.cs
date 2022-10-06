using Godot;
using System;

public class ColorSpreader : Component<Bullet>
{
    // private static Texture circle;

    [Export] public int count;
    [Export] public float rand;
    [Export] public bool spreadOnCreate = true;

    private float deltaDistance = 0, travelled = 0;

    private Node2D paintParent;

    private float offset;

    public override void _Ready()
    {
        base._Ready();
        Host.Connect(nameof(Bullet.OnMove), this, nameof(OnMove));
        Host.Connect(nameof(Bullet.OnDestory), this, nameof(OnDestory));
        deltaDistance = Host.Range / (float)count;

        paintParent = Host.UnitOwner.GetParent().GetParent().GetNode<Node2D>("ColorMap");

        offset = (GD.Randf() - 0.5f) * 2 * rand * HMap.GetSize().x;

        if (spreadOnCreate) Spread();
    }

    private void OnMove(float delta)
    {
        travelled += delta;
        if (travelled > deltaDistance + offset)
        {
            travelled = 0;
            Spread();
        }
    }

    private void OnDestory()
    {
        Spread();
    }

    private void Spread()
    {
        HMap.ClaimCircle(GlobalPosition, Host.ColorSpread, Host.UnitOwner.team);

        // circle = GD.Load<Texture>("res://Texture/Paint.png");

        // var paint_rid = VisualServer.CanvasItemCreate();
        // VisualServer.CanvasItemSetParent(paint_rid, paintParent.GetCanvasItem());
        // VisualServer.CanvasItemAddTextureRect(paint_rid, new Rect2(circle.GetSize() / 2, circle.GetSize()), circle.GetRid(), modulate: Host.UnitOwner.Color);
        // VisualServer.CanvasItemSetTransform(paint_rid, new Transform2D().Scaled(Vector2.One * Host.ColorSpread / 4));
    }
}