using Godot;
using System.Collections.Generic;

public class Unit : KinematicBody2D
{
    [Export] public float maxHP, speed, acceleration = 50f, rotateSpeed, maxInk = 100, inkGainSpeed, landBuffer;
    [Export] public Team team;
    [Export(PropertyHint.ResourceType, "Texture")] public Texture diveTexture;
    [Export] public PackedScene debug_weapon;

    private Sprite sprite;
    private Weapon weapon;
    private Color color;
    private Texture normalTexture;

    private float hp, ink, targetRotation, landTimer = 0;
    private Vector2 currentSpeed;
    private bool isDiving = false, isLanding = false;

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

    public float Ink { get => ink; set => ink = Mathf.Clamp(value, 0, maxInk); }

    public Vector2 CurrentSpeed { get => currentSpeed; set { currentSpeed = value.LimitLength(speed); } }

    public Weapon Weapon { get => weapon; }
    public Color Color { get => color; private set => color = value; }
    public bool IsDiving { get => isDiving; private set => isDiving = value; }
    // public float TargetRotation { get => targetRotation; set => targetRotation = value; }

    //Buff由Tag控制唯一性
    private Dictionary<string, Buff> buffs = new System.Collections.Generic.Dictionary<string, Buff>();
    private Node2D parent_weapon, parent_buff;

    //第一次进入场景，获取各个Node，血量调节
    public override void _Ready()
    {
        HP = maxHP;
        Color = TeamUtils.GetColor(team);
        ink = maxInk;

        parent_weapon = GetNode<Node2D>("Weapon");
        parent_buff = GetNode<Node2D>("Buff");
        sprite = GetNode<Sprite>("Sprite");

        Modulate = Color;

        normalTexture = sprite.Texture;

        //For test only
        SetWeapon(debug_weapon);

    }

    public override void _Process(float delta)
    {
        if (IsDiving)
        {
            Ink += inkGainSpeed * delta;
        }

        if (isLanding)
        {
            if (landTimer > 0)
            {
                landTimer -= delta;
            }
            else
            {
                LandImmediately();
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(CurrentSpeed);
        var deltaSpeed = acceleration / 2 * delta * CurrentSpeed.Normalized();
        CurrentSpeed -= deltaSpeed.Length() > CurrentSpeed.Length() ? CurrentSpeed : deltaSpeed;

        if (currentSpeed.Length() == 0) return;
        targetRotation = Vector2.Up.AngleTo(currentSpeed);

        if (!Mathf.IsEqualApprox(GlobalRotation, targetRotation))
        {
            GlobalRotation = Mathf.LerpAngle(GlobalRotation, targetRotation, 0.15f);
        }
        else
        {
            GlobalRotation = targetRotation;
        }
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
            return;
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

    public void Dive()
    {
        if (!HMap.IsOnTeamColor(GlobalPosition, team)) return;

        if (isLanding)
        {
            isDiving = false;
            isLanding = false;
        }

        if (IsDiving) return;

        IsDiving = true;

        ApplyBuff(Buff.Create("Dive", BuffType.Dive, 0.5f, -1));
        sprite.Texture = diveTexture;

        weapon.Visible = false;
    }

    public void Land()
    {
        if (!isDiving) return;
        if (isLanding) return;
        landTimer = landBuffer;
        isLanding = true;
    }

    public void LandImmediately()
    {
        RemoveBuff("Dive");
        sprite.Texture = normalTexture;
        weapon.Visible = true;

        isDiving = false;
        isLanding = false;
    }
}
