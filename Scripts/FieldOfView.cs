using System;
using System.Collections.Generic;
using Godot;

public partial class FieldOfView : Node2D
{
    [Export] float viewRadius;
    [Export] float viewAngle;
    [Export] Area2D targetArea;
    [Export] uint obstacleMask;
    [Export] float meshResolution;
    [Export] MeshInstance2D meshInstanceNode;

    float viewDiff;
    public List<Node2D> visibleTargets = new List<Node2D>();
    List<Vector3> viewPoints = new List<Vector3>();

    public override void _EnterTree()
    {
        viewDiff = Mathf.DegToRad(viewAngle * 0.5f);
        CollisionShape2D shape = targetArea.GetNode<CollisionShape2D>("CollisionShape2D");
        CircleShape2D circleShape2D = new()
        {
            Radius = viewRadius
        };
        shape.Shape = circleShape2D;
    }
    public override void _Process(double delta)
    {
        FindVisibleTargets();
        UpdateViewpoints();
        // QueueRedraw();
        DrawFOVMesh();
    }
    public override void _Draw()
    {
        // DrawViewpoints();
        // DrawCircle(Vector2.Zero, viewRadius, Colors.White);
        // DrawArc(Vector2.Zero, viewRadius, GlobalRotation - viewDiff, GlobalRotation + viewDiff, 20, Colors.Green, 20);
        // for (int i = 0; i < viewPoints.Count; i++)
        // {
        //     Vector3 vp = viewPoints[i];
        //     Color color = new Color(i * 0.1f, i * 0.1f, i * 0.1f);
        //     DrawLine(Position, new Vector2(vp.X, vp.Y), color);
        // }

    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        if (!targetArea.HasOverlappingAreas())
        {
            return;
        }
        foreach (Area2D area in targetArea.GetOverlappingAreas())
        {
            if (TargetIsVisible(area.GlobalPosition))
            {
                // GD.Print("Can see this target!");
                if (!visibleTargets.Contains(area))
                {
                    visibleTargets.Add(area);
                }
                if (area is VisibleTarget visibleTarget)
                {
                    visibleTarget.MarkVisible();
                }
            }
        }
    }
    bool TargetIsVisible(Vector2 targetPos)
    {
        Vector2 dirToTarget = (targetPos - GlobalPosition).Normalized();
        float angleToTarget = GlobalTransform.BasisXform(Vector2.Right).AngleTo(dirToTarget);

        if (angleToTarget < viewDiff && angleToTarget > -viewDiff)
        {
            // fire a raycast to the target checking if there are any obstacles in the way
            // if the raycast hits nothing, the path to the target is clear!
            var spaceState = GetWorld2D().DirectSpaceState;
            var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, targetPos, obstacleMask);
            var result = spaceState.IntersectRay(query);
            if (result.Count > 0)
            {
                // we hit something, we failed
                return false;
            }
            return true;
        }
        return false;
    }
    void UpdateViewpoints()
    {
        int stepCount = Mathf.RoundToInt(meshResolution * viewAngle);
        float stepAngleSize = viewAngle / stepCount;
        viewPoints.Clear();
        for (int i = 0; i <= stepCount; i++)
        {
            float localAngle = -viewAngle / 2 + stepAngleSize * i;
            float globalAngle = GlobalRotationDegrees + localAngle;
            ViewCastInfo newViewCast = ViewCast(globalAngle);
            // this line adds the global position point
            viewPoints.Add(new Vector3(newViewCast.point.X, newViewCast.point.Y, 0));

            // this line adds the local position point
            // Vector2 angleVector = DirFromAngle(localAngle);
            // viewPoints.Add(new Vector3(angleVector.X * newViewCast.distance, angleVector.Y * newViewCast.distance, 0));
        }
    }
    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector2 aimVector = DirFromAngle(globalAngle);
        Vector2 endPos = GlobalPosition + aimVector * viewRadius;
        var spaceState = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, endPos, obstacleMask);
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            Vector2 rayHitPos = (Vector2)result["position"];

            // we hit something, we failed
            return new ViewCastInfo(true, rayHitPos, GlobalPosition.DistanceTo(rayHitPos), globalAngle, aimVector);

        }
        return new ViewCastInfo(false, endPos, viewRadius, globalAngle, aimVector);

    }
    void DrawFOVMesh()
    {
        int vertexCount = viewPoints.Count + 1;
        // GD.Print(vertexCount);
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        // vertices[0] = new Vector3(GlobalPosition.X, GlobalPosition.Y, 0);
        vertices[0] = Vector3.Zero;
        for (int i = 0; i < vertexCount - 2; i++)
        {
            // Vector2 localVector = ToLocal(new Vector2(viewPoints[i].X, viewPoints[i].Y));
            // vertices[i + 1] = new Vector3(localVector.X, localVector.Y, 0);
            vertices[i + 1] = GlobalToLocalPoint(viewPoints[i]);
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        vertices[vertexCount - 1] = GlobalToLocalPoint(viewPoints[vertexCount - 2]);

        var surfaceArray = new Godot.Collections.Array();
        surfaceArray.Resize((int)Mesh.ArrayType.Max);

        surfaceArray[(int)Mesh.ArrayType.Vertex] = vertices;
        surfaceArray[(int)Mesh.ArrayType.Index] = triangles;

        var arrMesh = meshInstanceNode.Mesh as ArrayMesh;
        if (arrMesh != null)
        {
            arrMesh.ClearSurfaces();
            arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
        }
    }
    public Vector2 DirFromAngle(float angleInDegrees)
    {
        float angleInRadians = Mathf.DegToRad(angleInDegrees);
        return Vector2.FromAngle(angleInRadians);
        // return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
    }
    Vector3 GlobalToLocalPoint(Vector3 globalPoint)
    {
        Vector2 localVector = ToLocal(new Vector2(globalPoint.X, globalPoint.Y));
        return new Vector3(localVector.X, localVector.Y, 0);
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float distance;
        public float angle;
        public Vector2 aimVector;
        public ViewCastInfo(bool _hit, Vector2 _point, float _distance, float _angle, Vector2 _aimVector)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
            aimVector = _aimVector;
        }
    }
}
