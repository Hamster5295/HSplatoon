using Godot;
using System;

public class TipText : Label
{
    private Tween tween;
    private Unit u;
    private Control parent;

    private bool isShowing = false;
    private float currentY;

    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");
        parent = GetParent<Control>();
        CallDeferred(nameof(InitUnit));
    }

    private void InitUnit()
    {
        u = Controller.instance.Host;

        u.Connect(nameof(Unit.OnWeaponStateChanged), this, nameof(OnWeaponStateChanged));
        u.Connect(nameof(Unit.OnDead), this, nameof(OnDead));
        u.Connect(nameof(Unit.OnRevived), this, nameof(OnRevived));

        currentY = parent.RectPosition.y;
        parent.RectPosition += Vector2.Down * parent.RectSize.y;
    }

    private void OnWeaponStateChanged(Weapon w)
    {
        switch (w.State)
        {
            case WeaponState.Primary:
                Disappear();
                break;

            case WeaponState.Secondary:
                Text = "按下鼠标左键使用副武器";
                Appear();
                break;

            case WeaponState.Special:
                Text = "按下鼠标左键使用大招";
                Appear();
                break;
        }
    }

    private void OnDead()
    {
        CallDeferred(nameof(OnDeadInterval));
    }

    private void OnRevived()
    {
        Disappear();
    }

    private void OnDeadInterval()
    {
        Text = "等待复活..";
        Appear();
    }

    private void Appear()
    {
        if (isShowing) return;

        isShowing = true;

        tween.StopAll();
        tween.InterpolateProperty(parent, "rect_position:y", currentY + parent.RectSize.y, currentY, 0.2f);
        tween.Start();
    }

    private void Disappear()
    {
        if (!isShowing) return;

        isShowing = false;

        tween.StopAll();
        tween.InterpolateProperty(parent, "rect_position:y", currentY, currentY + parent.RectSize.y, 0.2f);
        tween.Start();
    }
}
