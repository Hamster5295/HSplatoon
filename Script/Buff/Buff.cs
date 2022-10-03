using Godot;
using System.Collections.Generic;

//Buff生效时，会作为玩家的子物体
//Godot自带有对象池，直接创建、销毁此物体的开销不算太大
public class Buff : Node2D, IInfomatable
{
    private static PackedScene buff = ResourceLoader.Load<PackedScene>("res://Prefab/Buff.tscn");

    [Export] public string tag;
    [Export] public BuffType type;
    [Export] public float intensity;
    [Export] public float duration;

    //使用代码实现Timer，方便之后拓展出时间伸缩
    private float timer = 0;
    //变化量，此处Buff的算法是，先计算 总量*增减百分数 获取增减的真实值，然后记录在此处。当Buff结束时返还给Unit
    private float deltaValue = 0;
    private Unit owner;

    public override void _Process(float delta)
    {
        OnUpdate(delta);
    }

    public static Buff Create(string tag, BuffType type, float intensity, float duration)
    {
        Buff b = buff.Instance<Buff>();
        b.tag = tag;
        b.type = type;
        b.intensity = intensity;
        b.duration = duration;

        return b;
    }

    //由Unit调用
    public void OnBuffAdded(Unit target)
    {
        owner = target;

        switch (type)
        {
            case BuffType.SpeedDecrease:
            case BuffType.Dive:
                deltaValue = owner.speed * (intensity + 1);
                owner.speed += deltaValue;
                break;
        }
    }

    public void OnUpdate(float delta)
    {
        if (duration == -1) return;

        timer += delta;
        if (timer >= duration)
        {
            owner.RemoveBuff(tag);
        }
    }

    public override void _ExitTree()
    {
        if (!IsInstanceValid(owner)) return;

        switch (type)
        {
            case BuffType.SpeedDecrease:
                owner.speed -= deltaValue;
                break;
        }
    }

    public Dictionary<string, string> GetInfo()
    {
        string buffName = "";

        switch (type)
        {
            case BuffType.SpeedDecrease:
                buffName = "减速";
                break;

            case BuffType.Dive:
                buffName = "潜水";
                break;
        }

        return new Dictionary<string, string>
        {
            {buffName,Mathf.RoundToInt(intensity*100)+"%"},
            {"持续时间",duration+"秒"}
        };
    }
}

public enum BuffType
{
    SpeedDecrease, Dive
}
