using Godot;
using System;

public class CombatUIRoot : Control
{
    public static CombatUIRoot instance;
    public Control combatUI;
    public CombatLogo combatLogo;
    public EndUI end;

    public override void _Ready()
    {
        instance = this;
        combatLogo = GetNode<CombatLogo>("CombatLogo");
        combatUI = GetNode<Control>("CombatUI");
        end = GetNode<EndUI>("EndUI");

        combatLogo.Visible = true;
        combatUI.Visible = false;
    }
}
