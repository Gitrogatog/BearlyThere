using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class StealthGameLoop : Node
{
    [Export] PlayerCharacter playerCharacter;
    List<GuardSightAI> guards;
    [Export] Control deathUI;
    [Export] Control detectBar;
    [Export] Control foodBar;
    [Export] Node2D playerStartPos;

    public override void _Ready()
    {
        StartLevel();
    }
    public override void _Process(double delta)
    {
        if (Detection.detectionMeter >= 1)
        {
            PlayerCaught();
        }
    }

    void ResetLevel()
    {
        StartLevel();
    }

    void StartLevel()
    {
        Detection.detectionMeter = 0;
        deathUI.Visible = false;
        playerCharacter.IsDead = false;
        playerCharacter.Position = playerStartPos.Position;
        // CreateTween().TweenMethod(Callable.From<int, int>((x, y) => x = 1), 1, 2, 1f);
        //tween.TweenMethod(Callable.From<float>(x => Node.RotationDegrees = x), 360f, 1f, 1f);
    }

    void PlayerCaught()
    {
        playerCharacter.IsDead = true;
        deathUI.Visible = true;
        deathUI.Modulate = new Color(1, 1, 1, 1);
    }
}
