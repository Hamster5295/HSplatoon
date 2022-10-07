using Godot;
using System.Collections.Generic;

public class AIController : Component<Unit>
{
    [Export] public float targetCD = 0.5f, randomCD = 1f;

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

        //锁定目标
        if (targetTimer > 0)
        {
            targetTimer -= delta;
        }
        else
        {
            targetTimer = targetCD;
            UpdateTargets();
        }

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].State != UnitState.Normal) { targets.RemoveAt(i); i--; }
        }
        //------

        //索敌
        if (targets.Count > 0)
        {
            nav.SetTargetLocation(targets[0].GlobalPosition);
            fireTimer = (targets[0].GlobalPosition - GlobalPosition).LengthSquared() <= 22500 ? 0.1f : 1;

            Host.Weapon.LookAt(targets[0].GlobalPosition);
            Host.Weapon.Rotate(Mathf.Tau / 4);
        }
        else//否则就随机找位置涂色
        {
            Host.Weapon.Rotation = 0;
            if (randomTimer <= 0)
            {
                // var rand = GD.Randf() * Mathf.Tau;
                // var targetPos = GlobalPosition + 400 * (Vector2.Right * Mathf.Sin(rand) + Vector2.Down * Mathf.Cos(rand) * GlobalPosition.Normalized() * -1);
                // targetPos.x = Mathf.Clamp(targetPos.x, -map.x / 2, map.x / 2);
                // targetPos.y = Mathf.Clamp(targetPos.y, -map.y / 2, map.y / 2);
                nav.SetTargetLocation(map * (GD.Randf() - 0.5f));
                randomTimer = randomCD;
            }
            else
            {
                randomTimer -= delta;
            }
        }

        //潜水与涂地逻辑：如果在旱地就涂色，水里就潜水
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

        //检查：有目标时不潜水，墨水不够就潜水
        if (targets.Count > 0) Host.Land();
        if (Host.Ink < 10) { Host.Dive(); return; }


        //副武器与大招
        if (targets.Count > 0 && Host.Weapon.secondaryWeapon.inkCost < Host.Ink)
        {
            Host.Weapon.HandleSecondary();
            fireTimer = 0.1f;
            Host.Weapon.HandleBegin();
        }

        if (Host.Energy == Host.maxEnergy)
        {
            Host.Weapon.HandleSpecial();
            fireTimer = 3f;
            Host.Weapon.HandleBegin();
        }

        //开火
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