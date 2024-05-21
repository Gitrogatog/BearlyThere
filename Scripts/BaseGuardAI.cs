using Godot;
using System;

public abstract partial class BaseGuardAI : Node2D
{
    public GuardSightAI guardSightAI;
    public bool guardEnabled = true;
    AnimatedSprite2D spriteNode;
    protected float currentVelocity = 0;

    public void Init()
    {
        var globalEvents = GetNode<GlobalEvents>("/root/GlobalEvents");
        globalEvents.StartLevel += OnStartLevel;
        spriteNode = GetNode<AnimatedSprite2D>("Sprite2D");
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
    public void SetAnim(Vector2 rotVector)
    {

        bool isRunning = currentVelocity > 1f;
        if (rotVector.X > 0.5f)
        {
            SetAnimFrame("right", isRunning);
        }
        else if (rotVector.X < -0.5f)
        {
            SetAnimFrame("left", isRunning);
        }
        else
        {
            if (rotVector.Y > 0.5f)
            {
                SetAnimFrame("down", isRunning);
            }
            else if (rotVector.Y < -0.5f)
            {
                SetAnimFrame("up", isRunning);
            }
        }
    }
    void SetAnimFrame(string animName, bool isRunning)
    {
        if (!isRunning)
        {
            spriteNode.Play($"{animName}_idle");
        }
        else
        {
            var current_frame = spriteNode.Frame;
            var current_progress = spriteNode.FrameProgress;
            spriteNode.Play($"{animName}_walk");
            spriteNode.SetFrameAndProgress(current_frame, current_progress);
        }

    }
    float SnapAngle(float rotationToSnap)
    {
        return (float)(Math.PI * 0.25 * Math.Round(rotationToSnap / Math.PI * 0.25f));

    }
}