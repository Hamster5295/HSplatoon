using Godot;

public class Unit : Node2D
{
    [Export] public float maxHP, speed;

    private float hp;
    private float currentSpeed;

    public float HP
    {
        get
        {
            return hp;
        }
        private set
        {
            hp = value;
            if (hp < 0)
            {
                //死亡逻辑
            }
        }
    }

    public float CurrentSpeed { get => currentSpeed; private set => currentSpeed = value; }


    //第一次进入场景，获取各个Node，血量调节
    public override void _Ready()
    {
        HP = maxHP;
    }

    public void TakeDamage(float damage, float speedDecrease)
    {
        HP -= damage;
    }
}
