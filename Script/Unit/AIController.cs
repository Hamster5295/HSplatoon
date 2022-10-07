using Godot;
using System.Collections.Generic;

public class AIController : Component<Unit>
{
    private Team t;

    private Vector2 next, map;
    private NavigationAgent2D nav;
    private Area2D area;

    private float targetTimer = 0, fireTimer = 0, randomTimer = 0;
    private List<Unit> targets = new List<Unit>();

    public override void _Ready()
    {
        base._Ready();

        area = GetNode<Area2D>("Area");
        map = HMap.GetMapSizeV();

        CallDeferred(nameof(Init));
    }

    private void Init()
    {
        nav = Host.Nav;
        nav.SetTargetLocation(Vector2.Zero);
    }

    public override void _PhysicsProcess(float delta)
    {
        next = nav.GetNextLocation();
        Host.ApplyAccel(next - GlobalPosition, delta);
        nav.SetVelocity(Host.CurrentSpeed);

        if (targetTimer > 0)
        {
            targetTimer -= delta;
        }
        else
        {
            targetTimer = 1;
            UpdateTargets();
        }

        if (targets.Count > 0)
        {
            nav.SetTargetLocation(targets[0].GlobalPosition);
            fireTimer = 2;
        }
        else
        {
            if (randomTimer <= 0)
            {
                // var rand = GD.Randf() * Mathf.Tau;
                // var targetPos = GlobalPosition + 400 * (Vector2.Right * Mathf.Sin(rand) + Vector2.Down * Mathf.Cos(rand) * GlobalPosition.Normalized() * -1);
                // targetPos.x = Mathf.Clamp(targetPos.x, -map.x / 2, map.x / 2);
                // targetPos.y = Mathf.Clamp(targetPos.y, -map.y / 2, map.y / 2);
                nav.SetTargetLocation(map * (GD.Randf() - 0.5f));
                randomTimer = 1;
            }
            else
            {
                randomTimer -= delta;
            }
        }

        if (Host.IsOnTeamColor())
        {
            if (fireTimer <= 0)
                Host.Dive();
        }
        else
        {
            Host.Land();
            if (fireTimer <= 0)
            {
                fireTimer = 1;
                Host.Weapon.HandleBegin();
            }
        }

        if (targets.Count > 0) Host.Land();
        if (Host.Ink < 10) { Host.Dive(); return; }

        if (fireTimer > 0)
        {
            fireTimer -= delta;
            Host.Weapon.HandleStay(delta);

            if (fireTimer <= 0)
            {
                Host.Weapon.HandleEnd();
            }
        }
    }

    private void UpdateTargets()
    {
        targets.Clear();

        foreach (var item in area.GetOverlappingBodies())
        {
            if (item is Unit u)
            {
                if (u.team != Host.team)
                {
                    targets.Add(u);
                }
            }
        }
    }
}