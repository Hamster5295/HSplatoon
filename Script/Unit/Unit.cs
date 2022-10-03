using Godot;
using System.Collections.Generic;

public class Unit : KinematicBody2D
{
    [Export] public float maxHP, speed, acceleration = 50f, rotateSpeed;
    [Export] public Color color;
    [Export] public PackedScene debug_weapon;

    private Weapon weapon;

    private float hp;
    private Vector2 currentSpeed;

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

    public Vector2 CurrentSpeed { get => currentSpeed; set { currentSpeed = value.LimitLength(speed); } }

    public Weapon Weapon { get => weapon; }

    //Buff由Tag控制唯一性
    private Dictionary<string, Buff> buffs = new System.Collections.Generic.Dictionary<string, Buff>();
    private Node2D parent_weapon, parent_buff;

    //第一次进入场景，获取各个Node，血量调节
    public override void _Ready()
    {
        HP = maxHP;

        parent_weapon = GetNode<Node2D>("Weapon");
        parent_buff = GetNode<Node2D>("Buff");

        Modulate = color;
        
        //For test only
        SetWeapon(debug_weapon);

    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(CurrentSpeed);
        var deltaSpeed = acceleration / 2 * delta * CurrentSpeed.Normalized();
        CurrentSpeed -= deltaSpeed.Length() > CurrentSpeed.Length() ? CurrentSpeed : deltaSpeed;
    }

    public void ApplyAccel(Vector2 direction, float delta)
    {
        CurrentSpeed += acceleration * delta * direction;
    }

    public void ChangeDirection(Vector2 axis)
    {
        Vector2 result = axis * currentSpeed;
        if (result.x < 0)
        {
            currentSpeed.x *= -1;
        }
        if (result.y < 0)
        {
            currentSpeed.y *= -1;
        }
    }

    public void TakeDamage(float damage, float speedDecrease)
    {
        HP -= damage;

        if (speedDecrease == 0) return;
        ApplyBuff(Buff.Create("DamageSD", BuffType.SpeedDecrease, speedDecrease, 0.5f));
    }

    public void SetWeapon(PackedScene w)
    {
        weapon = w.Instance<Weapon>();
        parent_weapon.AddChild(Weapon);
    }

    public void ApplyBuff(Buff buff)
    {
        if (HasBuff(buff.tag))
        {
            buff.QueueFree();
        }

        parent_buff.AddChild(buff);
        buff.OnBuffAdded(this);

        if (buffs.ContainsKey(buff.tag)) { buffs.Remove(buff.tag); }

        buffs.Add(buff.tag, buff);
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
