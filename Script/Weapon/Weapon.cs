using Godot;
using System.Collections.Generic;

public class Weapon : Component<Unit>
{
    [Signal] public delegate void OnUseBegin();
    [Signal] public delegate void OnUseStay(float delta);
    [Signal] public delegate void OnUseEnd();

    [Signal] public delegate void OnUseSecondary();
    [Signal] public delegate void OnActivateSecondary();
    [Signal] public delegate void OnActivateSpecial();
    [Signal] public delegate void OnUseSpecial();

    [Export] public string weaponName;
    [Export] public float recoil;

    public SecondaryWeapon secondaryWeapon;
    public SpecialWeapon specialWeapon;


    private Node parent_bullet;
    private Sprite sprite;
    private Tween tween;
    private WeaponState state = WeaponState.Primary;
    private int stateCounter = 0;
    private int headIndex = 0;
    private bool isRotationLocked = false;

    private List<Position2D> heads = new List<Position2D>();

    public List<Position2D> Heads { get => heads; private set => heads = value; }
    public bool IsRotationLocked { get => isRotationLocked; set => isRotationLocked = value; }
    public WeaponState State { get => state; }

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<Sprite>("Sprite");
        tween = GetNode<Tween>("Tween");
        parent_bullet = Host.GetNode<Node>("..");

        foreach (var item in GetNode<Node2D>("Heads").GetChildren())
        {
            if (item is Position2D p) Heads.Add(p);
        }

        Host.Connect(nameof(Unit.OnDive), this, nameof(HandleEnd));
    }

    public void Fire(Bullet bullet)
    {
        // if (Host.Ink < inkCost) return;
        if (Host.IsDiving) return;
        // Host.Ink -= inkCost;

        headIndex++;
        if (headIndex >= heads.Count) headIndex = 0;

        bullet.Position = GetHead();
        bullet.Rotation = GlobalRotation;

        bullet.ApplyDeltaAngle();

        parent_bullet.AddChild(bullet);

        tween.StopAll();
        tween.InterpolateProperty(this, "position", recoil * Vector2.Down.Rotated(Rotation), Vector2.Zero, 0.2f);
        tween.Start();
    }

    public Vector2 GetHead()
    {
        return heads[headIndex].GlobalPosition;
    }

    public Vector2 GetDirection()
    {
        return Vector2.Up.Rotated(GlobalRotation);
    }

    public void HandleBegin()
    {
        if (Host.State != UnitState.Normal) return;
        if (Host.IsDiving) return;

        switch (State)
        {
            case WeaponState.Primary:
                EmitSignal(nameof(OnUseBegin));
                break;

            case WeaponState.Secondary:
                EmitSignal(nameof(OnUseSecondary));
                break;

            case WeaponState.Special:
                EmitSignal(nameof(OnUseSpecial));
                break;
        }
    }

    public void HandleStay(float delta)
    {
        if (Host.State != UnitState.Normal) return;
        if (Host.IsDiving) return;

        if (State == WeaponState.Primary)
            EmitSignal(nameof(OnUseStay), delta);
    }

    public void HandleEnd()
    {
        if (Host.State == UnitState.Freeze) return;

        if (State == WeaponState.Primary)
            EmitSignal(nameof(OnUseEnd));
        else
        {
            if (stateCounter == -1) return;
            stateCounter--;
            if (stateCounter == 0)
            {
                state = WeaponState.Primary;
                Host.EmitSignal(nameof(Unit.OnWeaponStateChanged), this);
            }
        }
    }

    public void HandleSecondary()
    {
        if (Host.State != UnitState.Normal) return;

        EmitSignal(nameof(OnActivateSecondary));
    }

    public void HandleSpecial()
    {
        if (Host.State != UnitState.Normal) return;

        EmitSignal(nameof(OnActivateSpecial));
    }

    public void SetState(WeaponState state, int count)
    {
        // if (count <= 0) return;

        this.state = state;
        stateCounter = count;

        Host.EmitSignal(nameof(Unit.OnWeaponStateChanged), this);
    }
}

public enum WeaponState
{
    Primary, Secondary, Special
}
