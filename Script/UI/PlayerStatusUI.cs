using Godot;
using System;

public class PlayerStatusUI : TextureRect
{
    [Export] public int index;
    [Export(PropertyHint.ResourceType, "Texture")] public Texture normal, dead;

    private Unit u;

    public override void _Ready()
    {
        CallDeferred(nameof(InitUnit));
    }

    private void InitUnit()
    {
        u = Game.instance.GetUnit(index);

        SelfModulate = u.Color;
        if (u == Controller.instance.Host) SelfModulate = Colors.GreenYellow;

        Texture = normal;

        u.Connect(nameof(Unit.OnDead), this, nameof(OnDead));
        u.Connect(nameof(Unit.OnRevived), this, nameof(OnRevived));
    }

    private void OnDead()
    {
        Texture = dead;
        SelfModulate = Colors.Gray;
    }

    private void OnRevived()
    {
        Texture = normal;
        SelfModulate = u.Color;
        if (u == Controller.instance.Host) SelfModulate = Colors.GreenYellow;
    }
}
