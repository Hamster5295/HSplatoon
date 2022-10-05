using Godot;
using System.Collections.Generic;

public class Game : Node2D
{
    private static float REVIVE_TIME = 3;

    public static Game instance;

    private Timer timer;

    private GameState state;
    private Dictionary<Unit, GamePlayer> players = new Dictionary<Unit, GamePlayer>();

    public GameState State
    { get => state; }

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
    }

    public void AddUnit(Unit u, GamePlayer player)
    {
        players.Add(u, player);
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
