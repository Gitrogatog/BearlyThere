using Godot;
using System;
using System.Collections.Generic;

public partial class TestModifyMesh : Node2D
{
	[Export] MeshInstance2D meshInstanceNode;
	public override void _Ready()
	{
		// var mesh = new ArrayMesh();
		var surfaceArray = new Godot.Collections.Array();
		surfaceArray.Resize((int)Mesh.ArrayType.Max);
		var verts = new Vector3[] {
			new Vector3(50, 50, 0), new Vector3(200, 0, 0), new Vector3(100, 200, 0)
		};
		// var verts2 = new Godot.Collections.Array();
		var uvs = new Vector2[] {
			Vector2.Zero, Vector2.Zero, Vector2.Zero
		};
		var normals = new Vector3[] {
			Vector3.Up, Vector3.Up, Vector3.Up
		};
		var indices = new int[]{
			0, 1, 2
		};



		surfaceArray[(int)Mesh.ArrayType.Vertex] = verts;
		// surfaceArray[(int)Mesh.ArrayType.TexUV] = uvs;
		// surfaceArray[(int)Mesh.ArrayType.Normal] = normals;
		surfaceArray[(int)Mesh.ArrayType.Index] = indices;

		var arrMesh = meshInstanceNode.Mesh as ArrayMesh;
		if (arrMesh != null)
		{
			// No blendshapes, lods, or compression used.
			arrMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
		}
	}
}
