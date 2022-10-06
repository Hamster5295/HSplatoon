using Godot;
using System;

public class EndUI : Control
{
    [Export] public float speed;

    private Tween tween;
    private int playerC, enemyC;

    private TextureRect finish, cat1, cat2, flag, flagFace;
    private ProgressBar playerBar, enemyBar;
    private Label playerText, playerCounter, enemyText, enemyCounter, result;
    private Position2D playerPos, enemyPos;

    private AnimationPlayer ani;

    private Color playerColor, enemyColor;

    public override void _Ready()
    {
        tween = GetNode<Tween>("Tween");

        finish = GetNode<TextureRect>("Finish");
        cat1 = GetNode<TextureRect>("Cat1");
        cat2 = GetNode<TextureRect>("Cat2");
        flag = GetNode<TextureRect>("Flag");
        flagFace = GetNode<TextureRect>("Flag/F");
        playerBar = GetNode<ProgressBar>("PGB_Player");
        enemyBar = GetNode<ProgressBar>("PGB_Enemy");
        playerText = GetNode<Label>("Label_Player");
        enemyText = GetNode<Label>("Label_Enemy");
        playerCounter = GetNode<Label>("Count_Player");
        enemyCounter = GetNode<Label>("Count_Enemy");
        result = GetNode<Label>("Result");
        ani = GetNode<AnimationPlayer>("Ani");

        playerPos = GetNode<Position2D>("PlayerFlagPos");
        enemyPos = GetNode<Position2D>("EnemyFlagPos");

        ani.Connect("animation_finished", this, nameof(OnAnimationFinished));

        playerColor = TeamUtils.GetColor(Game.instance.PlayerTeam);
        enemyColor = TeamUtils.GetColor(Game.instance.EnemyTeam);

        playerBar.Modulate = playerColor;
        enemyBar.Modulate = enemyColor;
        playerText.Modulate = playerColor;
        enemyText.Modulate = enemyColor;

        cat1.Visible = false;
        cat2.Visible = false;
    }

    public void Start(int playerCount, int enemyCount)
    {
        Visible = true;
        playerC = playerCount;
        enemyC = enemyCount;
        ani.Play("End");
    }

    private void OnAnimationFinished(String s)
    {
        TextureRect winner, loser;
        Vector2 pos = Vector2.Zero;

        float baseNumber = HMap.GetMapSize();
        float playerPercent = (playerC / baseNumber), enemyPercent = (enemyC / baseNumber);
        float occupiedBaseNumber = playerC + enemyC;
        if (occupiedBaseNumber == 0)
        {
            playerC++;
            enemyC++;
            occupiedBaseNumber = 2;
        }

        if (playerC > enemyC)
        {
            winner = cat1;
            loser = cat2;
            pos = playerPos.GlobalPosition;
            flagFace.Modulate = playerColor;
            result.Text = "我方获胜";
            result.Modulate = playerColor;
        }
        else if (playerC < enemyC)
        {
            winner = cat2;
            loser = cat1;
            pos = enemyPos.GlobalPosition;
            flagFace.Modulate = enemyColor;
            result.Text = "敌方获胜";
            result.Modulate = enemyColor;
        }
        else
        {
            result.Text = "平局";
            Tie();
            return;
        }

        flag.RectPosition = pos;
        flag.Visible = true;

        playerCounter.Text = Mathf.Round(playerPercent * 100) + "%";
        enemyCounter.Text = Mathf.Round(enemyPercent * 100) + "%";

        tween.InterpolateProperty(playerBar, "value", null, playerBar.MaxValue * (playerC / occupiedBaseNumber), 0.3f);
        tween.InterpolateProperty(enemyBar, "value", null, enemyBar.MaxValue * (enemyC / occupiedBaseNumber), 0.3f);

        // playerBar.Value = playerBar.MaxValue * playerPercent;
        // enemyBar.Value = enemyBar.MaxValue * enemyPercent;

        tween.InterpolateProperty(winner, "rect_scale", null, Vector2.One * 1.2f, 0.3f);
        tween.InterpolateProperty(loser, "rect_scale:y", null, 0.7f, 0.3f);
        // winner.RectScale *= 1.2f;
        // loser.RectScale += Vector2.Up * 0.4f;
        tween.Start();
    }

    private void Tie()
    {
        tween.InterpolateProperty(playerBar, "value", null, playerBar.MaxValue / 2, 0.3f);
        tween.InterpolateProperty(enemyBar, "value", null, enemyBar.MaxValue / 2, 0.3f);
        tween.Start();

        playerCounter.Text = "50%";
        enemyCounter.Text = "50%";
    }
}
