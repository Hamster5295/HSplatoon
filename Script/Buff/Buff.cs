using Godot;
using System;

//Buff生效时，会作为玩家的子物体
public class Buff : Node2D
{
    [Export] public BuffType type;
    [Export] public float intensity;
    [Export] public float duration;

    //使用代码实现Timer，方便之后拓展出时间伸缩
    private float timer = 0;
    //变化量，此处Buff的算法是，先计算 总量*增减百分数 获取增减的真实值，然后记录在此处。当Buff结束时返还给Unit
    private float deltaValue = 0;
    private Unit owner;

    //由Unit调用
    public void OnBuffAdded(Unit target)
    {
        owner = target;

        switch (type)
        {
            case BuffType.SpeedDecrease:
                deltaValue = owner.speed * intensity;
                owner.speed += deltaValue;
                break;
        }
    }

    public void OnUpdate(float delta)
    {
        timer += delta;
        if (timer >= duration)
        {
            OnBuffReleased();
            QueueFree();
        }
    }

    public void OnBuffReleased()
    {
        switch (type)
        {
            case BuffType.SpeedDecrease:
                owner.speed -= deltaValue;
                break;
        }
    }
}

public enum BuffType
{
    SpeedDecrease
}
