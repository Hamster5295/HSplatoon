using Godot;
using System.Collections.Generic;

//用于记录地图是否被颜料占用的Tilemap，本身是替代Area提高性能的玩意，缺点是只能以网格的形式记录
public class HMap : TileMap
{
    private static HMap instance;

    private Dictionary<Vector2, Team> map = new Dictionary<Vector2, Team>();

    public override void _Ready()
    {
        instance = this;
    }

    public static bool IsSameTeam(Vector2 globalPos, Team t)
    {
        return instance.IsSameTeamInterval(globalPos, t);
    }

    public bool IsSameTeamInterval(Vector2 globalPos, Team t)
    {
        var tilePos = WorldToMap(globalPos);
        if (!map.ContainsKey(tilePos)) return false;
        return map[tilePos] == t;
    }

    public static void Claim(Vector2 globalPos, Team t)
    {
        instance.ClaimInterval(globalPos, t);
    }

    public void ClaimInterval(Vector2 globalPos, Team t)
    {
        var tilePos = WorldToMap(globalPos);
        if (!map.ContainsKey(tilePos)) map.Add(tilePos, t);
        else map[tilePos] = t;
    }
}
