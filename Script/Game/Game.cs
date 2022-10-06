using Godot;
using System.Collections.Generic;

public class Game : Node2D
{
    private static float REVIVE_TIME = 3;

    public static Game instance;

    private Timer timer;
    private Team playerTeam;
    private Team enemyTeam;

    private GameState state;
    private Dictionary<Unit, GamePlayer> players = new Dictionary<Unit, GamePlayer>();

    public GameState State
    { get => state; }
    public Team PlayerTeam { get => playerTeam; }
    public Team EnemyTeam { get => enemyTeam; }

    public override void _Ready()
    {
        instance = this;
        state = GameState.Ready;
        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, nameof(FinishGame));

        CallDeferred(nameof(StartBeginningAnimation));

        // StartGame();
    }

    public void StartGame()
    {
        state = GameState.Running;

        foreach (var item in players)
        {
            item.Key.State = UnitState.Normal;
        }

        timer.Start();
    }

    public void FinishGame()
    {
        state = GameState.Finished;
        foreach (var item in players)
        {
            item.Key.State = UnitState.Freeze;
        }

        int p = 0, e = 0;
        foreach (var item in HMap.GetMap())
        {
            if (item.Value == playerTeam) p++;
            else if (item.Value == enemyTeam) e++;
        }

        CombatUIRoot.instance.end.Start(p, e);
    }

    public void AddUnit(Unit u, GamePlayer player)
    {
        players.Add(u, player);
        if (player.type == TeamType.Player) playerTeam = player.team;
        if (player.type == TeamType.Enemy) enemyTeam = player.team;
        GetParent().CallDeferred("add_child", u);
    }

    public void RegisterRevive(Unit u)
    {
        if (!players.ContainsKey(u))
        {
            GD.PrintErr("出现了没注册的Unit, 厚礼蟹");
            return;
        }

        players[u].Revive(REVIVE_TIME);
    }

    private void StartBeginningAnimation()
    {
        var logo = CombatUIRoot.instance.combatLogo;
        logo.Connect(nameof(CombatLogo.OnAnimationFinished), this, nameof(StartGame));
        logo.Start();
    }
}

public enum GameState
{
    Ready, Running, Finished
}
