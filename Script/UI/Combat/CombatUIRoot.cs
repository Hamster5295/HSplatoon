using Godot;
using System;

public class CombatUIRoot : Control
{
    public static CombatUIRoot instance;
    private Control combatLogo;

    public override void _Ready()
    {
        instance = this;
        combatLogo = GetNode<Control>("CombatLogo");
    }
}
