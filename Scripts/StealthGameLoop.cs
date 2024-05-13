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
    GlobalEvents globalEvents;

    public override void _Ready()
    {
        globalEvents = GetNode<GlobalEvents>("/root/GlobalEvents");
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

        // global.GotoScene("res://Scene2.tscn");
        globalEvents.EmitSignal(GlobalEvents.SignalName.ResetLevel);
        StartLevel();
    }

    void StartLevel()
    {
        globalEvents.EmitSignal(GlobalEvents.SignalName.StartLevel);
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
