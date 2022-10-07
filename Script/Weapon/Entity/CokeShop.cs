using Godot;
using System.Collections.Generic;

public class CokeShop : Area2D, IDeployable
{
    private Sprite coke1, coke2;

    private Team team;
    private List<Unit> already = new List<Unit>();

    private int count = 2;
    private float removeTimer = -1;

    public void Deploy(Vector2 pos, Team t)
    {
        Position = pos;
        team = t;
        Modulate = TeamUtils.GetColor(t);
        Connect("body_entered", this, nameof(OnBodyEntered));

        coke1 = GetNode<Sprite>("Coke1");
        coke2 = GetNode<Sprite>("Coke2");
    }

    public Node2D GetNode()
    {
        return this;
    }

    public override void _Process(float delta)
    {
        if (removeTimer > 0)
        {
            removeTimer -= delta;
            if (removeTimer <= 0)
            {
                QueueFree();
            }
        }
    }

    private void OnBodyEntered(Node b)
    {
        if (b is Unit u)
        {
            if (u.team != team) return;
            if (count <= 0) return;
            if (already.Contains(u)) return;

            already.Add(u);
            count--;

            u.ApplyBuff("Coke", BuffType.Speed, 0.3f, 5);

            UpdateCoke();
        }
    }

    private void UpdateCoke()
    {
        switch (count)
        {
            case 2:
                coke2.Visible = true;
                coke1.Visible = true;
                break;

            case 1:
                coke2.Visible = true;
                coke1.Visible = false;
                break;

            case 0:
                coke2.Visible = false;
                coke1.Visible = false;

                removeTimer = 3;
                break;
        }
    }
}
