using Godot;
using System;

public class CombatUIRoot : Control
{
    public static CombatUIRoot instance;
    public CombatLogo combatLogo;
    public EndUI end;

    public override void _Ready()
    {
        instance = this;
        combatLogo = GetNode<CombatLogo>("CombatLogo");
        end = GetNode<EndUI>("EndUI");
    }
}
