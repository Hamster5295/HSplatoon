using Godot;
using System.Collections.Generic;

public class Unit : KinematicBody2D
{
    public static float DAMAGE_PER_SECOND_ON_ENEMY_COLOR = 10,
    SPEED_DECREASE_WHEN_HIT = -0.3f,
    ENERGY_NATURAL_GAIN = 2,
    ENERGY_KILL_GAIN = 10;

    [Export] public float maxHP, speed, acceleration = 50f, rotateSpeed, maxInk = 100, inkGainSpeed, landBuffer;
    [Export] public Team team;
    [Export(PropertyHint.ResourceType, "Texture")] public Texture diveTexture;
    [Export] public PackedScene debug_weapon;

    private Sprite sprite;
    private Weapon weapon;
    private Color color;
    private Texture normalTexture;

    private float hp, ink, targetRotation, energy;
    private Vector2 currentSpeed, accel = Vector2.Zero;
    private bool isDiving = false;
    private UnitState state = UnitState.Normal;

    public float HP
    {
        get => hp;
        private set
        {
            hp = value;
            if (hp < 0)
            {
                //死亡逻辑
                OnDead();
            }
        }
    }

    public float Ink { get => ink; set => ink = Mathf.Clamp(value, 0, maxInk); }

    public Vector2 CurrentSpeed
    {
        get => currentSpeed;
        set
        {
            var origin = currentSpeed;
            if (origin.Length() < speed)
            {
                if (value.Length() > speed)
                {
                    currentSpeed = value.Normalized() * speed;
                }
                else currentSpeed = value;
            }
            else
            {
                if (origin.Length() > value.Length())
                {
                    currentSpeed = value;
                }
            }
        }
    }

    public Weapon Weapon { get => weapon; }
    public Color Color { get => color; private set => color = value; }
    public bool IsDiving { get => isDiving; private set => isDiving = value; }
    public float Energy { get => energy; set => energy = Mathf.Clamp(value, 0, 100); }
    public UnitState State { get => state; set => state = value; }

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
        if (debug_weapon != null)
            SetWeapon(debug_weapon);

    }

    public override void _Process(float delta)
    {
        if (state != UnitState.Normal) return;

        if (IsDiving)
        {
            if (HMap.IsOnTeamColor(GlobalPosition, team))
            {
                Ink += inkGainSpeed * delta;

                RemoveBuff("Dive_Land");
                ApplyBuff("Dive_Ink", BuffType.Dive, 1, -1);
            }
            else
            {
                if (HMap.IsOnEmptyCell(GlobalPosition))
                {
                    RemoveBuff("Dive_Ink");
                    ApplyBuff("Dive_Land", BuffType.Dive, -0.5f, -1);
                }
                else
                {
                    Land();
                }
            }
        }

        if (HMap.IsOnEnemyTeamColor(GlobalPosition, team))
        {
            TakeDamage(DAMAGE_PER_SECOND_ON_ENEMY_COLOR * delta);
        }

        Energy += ENERGY_NATURAL_GAIN * delta;
    }

    public override void _PhysicsProcess(float delta)
    {
        if (state != UnitState.Normal) return;

        MoveAndSlide(CurrentSpeed);
        var deltaSpeed = acceleration / 2 * delta * CurrentSpeed.Normalized();
        CurrentSpeed -= deltaSpeed.Length() > CurrentSpeed.Length() ? CurrentSpeed : deltaSpeed;

        CurrentSpeed += accel;
        accel = Vector2.Zero;

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
        if (state != UnitState.Normal) return;
        accel += acceleration * delta * direction;
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

    public bool TakeDamage(float damage)
    {
        HP -= damage;

        ApplyBuff("DamageSD", BuffType.Speed, SPEED_DECREASE_WHEN_HIT, 0.5f);

        return HP <= 0;
    }

    public void SetWeapon(PackedScene w)
    {
        weapon = w.Instance<Weapon>();
        if (parent_weapon == null) parent_weapon = GetNode<Node2D>("Weapon");

        if (parent_weapon.GetChildCount() > 0)
            foreach (var item in parent_weapon.GetChildren())
            {
                ((Node2D)item).QueueFree();
            }
        parent_weapon.AddChild(Weapon);
    }

    public void ApplyBuff(string tag, BuffType type, float intensity, float duration)
    {
        if (HasBuff(tag)) return;

        ApplyBuffInterval(Buff.Create(tag, type, intensity, duration));
    }

    private void ApplyBuffInterval(Buff buff)
    {
        parent_buff.AddChild(buff);
        buff.OnBuffAdded(this);

        if (buffs.ContainsKey(buff.tag)) { buffs.Remove(buff.tag); }

        buffs.Add(buff.tag, buff);
    }

    public void RemoveBuff(string tag)
    {
        if (!HasBuff(tag)) return;

        buffs[tag].OnBuffRemoved();
        buffs[tag].QueueFree();
        buffs.Remove(tag);
    }

    public bool HasBuff(string tag)
    {
        return buffs.ContainsKey(tag);
    }

    public void Dive()
    {
        if (IsDiving) return;
        IsDiving = true;

        sprite.Texture = diveTexture;
        weapon.Visible = false;
    }

    public void Land()
    {
        sprite.Texture = normalTexture;
        weapon.Visible = true;

        isDiving = false;

        RemoveBuff("Dive_Land");
        RemoveBuff("Dive_Ink");
    }

    public void OnKillEnemy()
    {
        Energy += ENERGY_KILL_GAIN;
    }

    public void OnDead()
    {
        Visible = false;
        state = UnitState.Dead;
        Game.instance.RegisterRevive(this);
    }

    public void OnRevive()
    {
        hp = maxHP;
        Visible = true;
        state = UnitState.Normal;
    }
}

public enum UnitState
{
    Normal, Dead, Freeze
}
