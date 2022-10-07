using Godot;
using System;

public class Btn_ChangeScene : TextureButton
{
    [Export] public PackedScene nextScene;

    public override void _Ready()
    {
        Connect("pressed", this, nameof(OnClick));
    }

    private void OnClick()
    {
        GetTree().ChangeSceneTo(nextScene);
    }
}
