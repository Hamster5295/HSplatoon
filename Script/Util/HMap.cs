using Godot;
using System.Collections.Generic;

//用于记录地图是否被颜料占用的Tilemap，本身是替代Area提高性能的玩意，缺点是只能以网格的形式记录
public class HMap : TileMap
{
    private static HMap instance;
    private static Texture circle = GD.Load<Texture>("res://Texture/Paint.png");
    private Dictionary<Vector2, Team> map = new Dictionary<Vector2, Team>();

    public override void _Ready()
    {
        instance = this;
    }

    public override void _Draw()
    {
        foreach (var item in map)
        {
            DrawRect(new Rect2(item.Key * CellSize.x + CellSize / 2, CellSize), TeamUtils.GetDarkColor(item.Value));
        }
    }

    public static bool IsOnTeamColor(Vector2 globalPos, Team t)
    {
        return instance.IsSameTeamInterval(globalPos, t);
    }

    private bool IsSameTeamInterval(Vector2 globalPos, Team t)
    {
        var tilePos = WorldToMap(globalPos);
        if (!map.ContainsKey(tilePos)) return false;
        return map[tilePos] == t;
    }

    public static bool IsOnEmptyCell(Vector2 globalPos)
    {
        return instance.IsOnEmptyCellInterval(globalPos);
    }

    private bool IsOnEmptyCellInterval(Vector2 globalPos)
    {
        return !map.ContainsKey(WorldToMap(globalPos));
    }

    public static void Claim(Vector2 globalPos, Team t)
    {
        instance.ClaimInterval(globalPos, t);
    }

    private void ClaimInterval(Vector2 globalPos, Team t)
    {
        var tilePos = WorldToMap(globalPos);
        if (!map.ContainsKey(tilePos)) map.Add(tilePos, t);
        else map[tilePos] = t;
    }

    private void ClaimWithTileInterval(Vector2 tilePos, Team t)
    {
        if (!map.ContainsKey(tilePos)) map.Add(tilePos, t);
        else map[tilePos] = t;

        Update();
    }

    public static void ClaimCircle(Vector2 globalCenter, int radius, Team t)
    {
        instance.ClaimCircleInterval(globalCenter, radius, t);
    }

    private void ClaimCircleInterval(Vector2 globalCenter, int radius, Team t)
    {
        var centerPos = WorldToMap(globalCenter);

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                Vector2 pos = new Vector2(i, j);
                if (pos.Length() <= radius)
                {
                    ClaimWithTileInterval(centerPos + pos, t);
                }
            }
        }

        Update();
    }
}
