using Godot;
using System;

public class TimeText : Label
{

    public override void _Ready()
    {
        Game.instance.Connect(nameof(Game.OnTimeTick), this, nameof(OnTimeTick));
    }

    private void OnTimeTick(int second)
    {
        Text = Format(second);
    }

    private string Format(int second)
    {
        int m = second / 60;
        int s = second % 60;

        return (m < 10 ? "0" : "") + m + ":" + (s < 10 ? "0" : "") + s;
    }
}
