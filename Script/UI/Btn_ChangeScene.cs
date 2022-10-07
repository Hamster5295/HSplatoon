using Godot;
using System;

public class Btn_ChangeScene : TextureButton
{
    [Export(PropertyHint.File)] public string nextScene;

    public override void _Ready()
    {
        Connect("pressed", this, nameof(OnClick));
    }

    private void OnClick()
    {
        GetTree().ChangeScene(nextScene);
    }
}
