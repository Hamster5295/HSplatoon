using Godot;
using System;

public class EnergyBar : TextureProgress
{
    private Unit player;

    public override void _Ready()
    {
        CallDeferred(nameof(ConfigurePlayer));
    }

    private void ConfigurePlayer()
    {
        player = Controller.instance.Host;
        player.Connect(nameof(Unit.OnEnergyChanged), this, nameof(OnEnergyChanged));

        Value = MaxValue * player.Energy / player.maxEnergy;
    }

    private void OnEnergyChanged(float energy, float maxEnergy)
    {
        Value = MaxValue * energy / maxEnergy;
    }
}
