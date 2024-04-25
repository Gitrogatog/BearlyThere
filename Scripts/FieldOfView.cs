using System;
using System.Collections.Generic;
using Godot;

public partial class FieldOfView : Node2D
{
    [Export] float viewRadius;
    [Export] float viewAngle;
    [Export] Area2D targetArea;
    [Export] uint obstacleMask;
    float viewDiff;
    List<Node2D> visibleTargets = new List<Node2D>();
    public override void _EnterTree()
    {
        viewDiff = Mathf.DegToRad(viewAngle * 0.5f);
        CollisionShape2D shape = targetArea.GetNode<CollisionShape2D>("CollisionShape2D");
        CircleShape2D circleShape2D = new()
        {
            Radius = viewRadius
        };
        shape.Shape = circleShape2D;
        // if (shape.Shape is CircleShape2D circle)
        // {
        //     circle.Radius = viewRadius;
        // }
    }
    public override void _Process(double delta)
    {
        FindVisibleTargets();
    }
    public override void _Draw()
    {
        DrawCircle(Vector2.Zero, viewRadius, Colors.White);
        // void draw_arc(Vector2 center, float radius, float start_angle, float end_angle, int point_count, Color color, float width = -1.0, bool antialiased = false);
        DrawArc(Vector2.Zero, viewRadius, GlobalRotation - viewDiff, GlobalRotation + viewDiff, 20, Colors.Green, 20);

    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Vector2 mousePos = GetGlobalMousePosition();
        if (!targetArea.HasOverlappingAreas())
        {
            return;
        }
        foreach (Area2D area in targetArea.GetOverlappingAreas())
        {
            if (TargetIsVisible(area.GlobalPosition))
            {
                GD.Print("Can see this target!");
                if (!visibleTargets.Contains(area))
                {
                    visibleTargets.Add(area);
                }
                if (area is VisibleTarget visibleTarget)
                {
                    visibleTarget.MarkVisible();
                }
            }
            // else{
            //     if(visibleTargets.Contains(area)){

            //     }
            // }
        }
        // if (!TargetIsVisible(mousePos))
        // {
        //     GD.Print("Cannot see!");
        // }
        // GD.Print();
    }
    bool TargetIsVisible(Vector2 targetPos)
    {
        Vector2 dirToTarget = (targetPos - GlobalPosition).Normalized();
        float angleToTarget = GlobalTransform.BasisXform(Vector2.Right).AngleTo(dirToTarget);
        // dirToTarget.AngleTo(GlobalTransform.BasisXform(Vector2.Right))
        // GD.Print(angleToTarget);

        if (angleToTarget < viewDiff && angleToTarget > -viewDiff)
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, targetPos, obstacleMask);
            var result = spaceState.IntersectRay(query);
            if (result.Count > 0)
            {
                // we hit something, we failed
                return false;
            }
            return true;

            // return true;
        }
        return false;
    }
    public Vector2 DirFromAngle(float angleInDegrees)
    {
        float angleInRadians = Mathf.DegToRad(angleInDegrees);
        return new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));
    }
}
