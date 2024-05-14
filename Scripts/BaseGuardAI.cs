using Godot;
using System;

public abstract partial class BaseGuardAI : Node2D
{
    public GuardSightAI guardSightAI;
    public bool guardEnabled = true;

    public void Init()
    {
        var globalEvents = GetNode<GlobalEvents>("/root/GlobalEvents");
        globalEvents.StartLevel += OnStartLevel;
        guardSightAI = GetNode<GuardSightAI>("GuardSightAI");
        guardSightAI.MaxDetection += DisableGuard;
    }
    public override void _Process(double delta)
    {
        if (guardEnabled)
        {
            MoveGuard((float)delta);
        }
    }
    public virtual void OnStartLevel()
    {
        EnableGuard();
    }

    void EnableGuard()
    {
        guardEnabled = true;
        guardSightAI.SetProcess(true);
        guardSightAI.Visible = true;
        guardSightAI.currentDetectionTime = 0;
    }

    void DisableGuard()
    {
        guardEnabled = false;
        guardSightAI.SetProcess(false);
        guardSightAI.Visible = false;
    }

    abstract public void MoveGuard(float delta);
}