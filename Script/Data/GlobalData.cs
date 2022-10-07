using Godot;

public class GlobalData
{
    public static PackedScene gun = GD.Load<PackedScene>("res://Prefab/Weapon/Gun-MK1.tscn");
    public static PackedScene brush = GD.Load<PackedScene>("res://Prefab/Weapon/Brush-MK1.tscn");

    public static PackedScene playerSelected = gun;
}