using Godot;

public class Deployer : Component<Bullet>
{
    [Export] public PackedScene deployable;

    public override void _Ready()
    {
        base._Ready();
        CallDeferred(nameof(SetUp));
    }

    private void SetUp()
    {
        Host.Connect(nameof(Bullet.OnDestory), this, nameof(Destroy));
    }

    public void Destroy()
    {
        var d = deployable.Instance<IDeployable>();
        d.Deploy(GlobalPosition, Host.UnitOwner.team);
        Host.UnitOwner.GetParent().AddChild(d.GetNode());
    }
}