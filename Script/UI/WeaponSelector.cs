using Godot;
using Godot.Collections;
// using System;

public class WeaponSelector : Control
{
    private TextureButton btn_gun, btn_brush;

    public override void _Ready()
    {
        btn_gun = GetNode<TextureButton>("GunSelect");
        btn_brush = GetNode<TextureButton>("BrushSelect");

        btn_gun.Connect("pressed", this, nameof(OnClick), new Array() { 0 });
        btn_brush.Connect("pressed", this, nameof(OnClick), new Array() { 1 });
    }

    private void OnClick(int what)
    {
        switch (what)
        {
            case 0:
                btn_gun.Pressed = true;
                btn_brush.Pressed = false;
                GlobalData.playerSelected = GlobalData.gun;
                break;

            case 1:
                btn_gun.Pressed = false;
                btn_brush.Pressed = true;
                GlobalData.playerSelected = GlobalData.brush;
                break;
        }
    }
}
