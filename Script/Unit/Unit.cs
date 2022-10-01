using Godot;
using System.Collections.Generic;

public class Unit : KinematicBody2D
{
    [Export] public float maxHP, speed, acceleration = 50f, rotateSpeed;

    private float hp;
    private float currentSpeed;

    public float HP
    {
        get => hp;
        private set
        {
            hp = value;
            if (hp < 0)
            {
                //死亡逻辑
            }
        }
    }

    public float CurrentSpeed { get => currentSpeed; protected set { currentSpeed = Mathf.Clamp(value, -speed, speed); } }

    //Buff由Tag控制唯一性
    private Dictionary<string, Buff> buffs = new System.Collections.Generic.Dictionary<string, Buff>();
    private Node2D parent_weapon, parent_buff;

    //第一次进入场景，获取各个Node，血量调节
    public override void _Ready()
    {
        HP = maxHP;

        parent_weapon = GetNode<Node2D>("Weapon");
        parent_buff = GetNode<Node2D>("Buff");
    }

    //和Unity的Update()等效，不过Time.deltaTime变成了这里的delta
    // public override void _Process(float delta)
    // {
    //     Translate(Vector2.Up.Rotated(Rotation) * CurrentSpeed * delta);
    //     CurrentSpeed -= acceleration / 2 * delta * Mathf.Sign(CurrentSpeed);
    // }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(Vector2.Up.Rotated(Rotation) * CurrentSpeed);
        CurrentSpeed -= acceleration / 2 * delta * Mathf.Sign(CurrentSpeed);
    }


    public void TakeDamage(float damage, float speedDecrease)
    {
        HP -= damage;
    }

    public void ApplyBuff(PackedScene buff)
    {
        var b = buff.Instance<Buff>();
        if (HasBuff(b.tag))
        {
            b.QueueFree();
        }

        parent_buff.AddChild(b);
        b.OnBuffAdded(this);

        buffs.Add(b.tag, b);
    }

    public void RemoveBuff(string tag)
    {
        if (!HasBuff(tag)) return;

        buffs[tag].QueueFree();
        buffs.Remove(tag);
    }

    public bool HasBuff(string tag)
    {
        return buffs.ContainsKey(tag);
    }
}
