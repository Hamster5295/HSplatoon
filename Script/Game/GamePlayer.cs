using Godot;

public class GamePlayer : Component<Game>
{
    [Export] public PackedScene unit, weapon;
    [Export] public Team team;
    [Export] public bool isPlayer = false;
    [Export] public TeamType type;

    private Unit u;
    private float reviveTimer = 0;
    private Vector2 point;

    public override void _Ready()
    {
        base._Ready();
        point = GetNode<Position2D>("Point").GlobalPosition;

        if (isPlayer)
        {
            weapon = GlobalData.playerSelected;
        }

        u = unit.Instance<Unit>();
        u.team = team;
        u.State = UnitState.Freeze;
        u.SetWeapon(weapon);
        u.GlobalPosition = point;

        Host.AddUnit(u, this);
    }

    public override void _Process(float delta)
    {
        if (reviveTimer > 0)
        {
            reviveTimer -= delta;

            if (reviveTimer <= 0)
            {
                u.GlobalPosition = point;
                u.Revive();
            }
        }
    }

    public void Revive(float time)
    {
        reviveTimer = time;
    }
}

public enum TeamType
{
    Player, Enemy
}