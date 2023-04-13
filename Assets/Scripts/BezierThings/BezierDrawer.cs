using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class BezierDrawer : MonoBehaviour
{
	/// <summary>
	/// Starting Control point, defines starting direction for the Bezier
	/// </summary>
	[SerializeField]
	private List<BezierPoint> Beziers;

	[SerializeField]
	private bool IsLooping = false;

	[SerializeField]
	[Range(0f, 1f)]
	private float t = 0f; // Placement position on the bezier track

	[SerializeField]
	private Mesh2D road2D;

	private MeshRenderer meshRenderer;
	private MeshFilter meshFilter;

	[SerializeField]
	private int divider = 5;

	private void Awake()
	{
		this.meshRenderer = gameObject.GetComponent<MeshRenderer>();
		this.meshFilter = gameObject.GetComponent<MeshFilter>();
	}

	private void Start()
	{
		GenerateMesh();
	}

	private void Update()
	{
		GenerateMesh();
	}

	private void OnDrawGizmos()
	{

		for (int i = 0; i < Beziers.Count; i++)
		{
			BezierPoint source = Beziers[i];
			BezierPoint target;
			if (Beziers.Count > i + 1)
			{
				target = Beziers[i + 1];
			}
			else
			{
				if (IsLooping)
				{
					target = Beziers[0];
				}
				else
				{
					break;
				}

			}

			Handles.DrawBezier(source.Anchor.position, target.Anchor.position, source.Control1.position, target.Control0.position, Color.yellow, default, 5f);

			Vector3 tPos = GetBezierPositionWhenT(t, source, target); // Calculate position when t is x
			Vector3 tDir = GetBezierDirectionWhenT(t, source, target);

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(tPos, 0.2f);
			
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(tPos, tPos + tDir * 3);

			Quaternion rot = Quaternion.LookRotation(tDir);

			Handles.PositionHandle(tPos, rot);

			float tIncrement = 1f / divider;

			for (int iz = 0; iz < divider; iz++)
			{
				float calcT = (float)iz * tIncrement;

				Vector3 tCalcPos = GetBezierPositionWhenT(calcT, source, target); // Calculate position when t is x
				Vector3 tCalcDir = GetBezierDirectionWhenT(calcT, source, target);
				Quaternion Calcrot = Quaternion.LookRotation(tCalcDir);

				for (int ix = 0; ix < road2D.vertices.Length; ix++)
				{
					Vector3 roadPoint = road2D.vertices[ix].point;
					Gizmos.DrawSphere(tCalcPos + (Calcrot * roadPoint), 0.1f);
				}
			}
		}
	}

	private Vector3 GetBezierPositionWhenT(float tValue, BezierPoint source, BezierPoint target)
	{
		// First Lerp
		Vector3 PtX = (1 - tValue) * source.Anchor.position + tValue * source.Control1.position;
		Vector3 PtY = (1 - tValue) * source.Control1.position + tValue * target.Control0.position;
		Vector3 PtZ = (1 - tValue) * target.Control0.position + tValue * target.Anchor.position;

		// Second Lerp
		Vector3 PtR = (1 - tValue) * PtX + tValue * PtY;
		Vector3 PtS = (1 - tValue) * PtY + tValue * PtZ;

		// Third Lerp
		Vector3 PtO = (1 - tValue) * PtR + tValue * PtS;

		return PtO;
	}

	private Vector3 GetBezierDirectionWhenT(float tValue, BezierPoint source, BezierPoint target)
	{
		// First Lerp
		Vector3 PtX = (1 - tValue) * source.Anchor.position + tValue * source.Control1.position;
		Vector3 PtY = (1 - tValue) * source.Control1.position + tValue * target.Control0.position;
		Vector3 PtZ = (1 - tValue) * target.Control0.position + tValue * target.Anchor.position;

		// Second Lerp
		Vector3 PtR = (1 - tValue) * PtX + tValue * PtY;
		Vector3 PtS = (1 - tValue) * PtY + tValue * PtZ;

		// Direction forward is the vector between R and S points
		return (PtS - PtR).normalized;
	}

	private void GenerateMesh()
	{
		// Create a new Mesh
		Mesh mesh = new Mesh();
		this.meshFilter.mesh = mesh;

		// Vertices
		List<Vector3> vertices = new List<Vector3>();

		// UVs
		List<Vector3> uvs = new List<Vector3>();

		GenerateVertices(vertices, uvs);

		List<int> triangles = new List<int>();

		int offset = road2D.vertices.Length;

		int triangleSets = Beziers.Count * (divider - 1);

		if (IsLooping)
		{
			triangleSets += divider;
		}

		Debug.Log("Trianglesets:" + triangleSets);

		// create the triangles
		for (int tri = 0; tri < triangleSets; tri++)
		{
			int startOffset = offset * tri;
			for (int i = 0; i < road2D.vertices.Length; i += 2)
			{
				triangles.Add((startOffset + i) % vertices.Count);
				triangles.Add((startOffset + i + offset) % vertices.Count);
				triangles.Add((startOffset + i + 1) % vertices.Count);

				triangles.Add((startOffset + i + 1) % vertices.Count);
				triangles.Add((startOffset + i + offset) % vertices.Count);
				triangles.Add((startOffset + i + offset + 1) % vertices.Count);
			}
		}

		mesh.vertices = vertices.ToArray();
		mesh.SetUVs(0, uvs);
		// set the triangles and recalculate the normals
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateNormals();
	}

	private void GenerateVertices(List<Vector3> vertices, List<Vector3> uvs)
	{
		int endLoop = Beziers.Count;
		bool isLastOne = false;

		if (IsLooping)
		{
			endLoop++; // Make trail between the end of last bezier and start of first bezier
		}

		for (int i = 0; i < endLoop; i++)
		{
			BezierPoint source = Beziers[i];
			BezierPoint target;
			if (Beziers.Count > i + 1)
			{
				target = Beziers[i + 1];
			}
			else
			{
				if (IsLooping)
				{
					target = Beziers[0];
					isLastOne = true; // Make sure this is the last time we loop
				}
				else
				{
					break;
				}
			}

			float tIncrement = 1f / divider;

			int sliceCount = divider;

			if (!IsLooping && isLastOne)
			{
				sliceCount++;
			}

			// If we want 5 divisions, we will draw 4 slices in looped or extra slice in last one when unlooped
			for (int iz = 0; iz < divider; iz++)
			{
				float calcT = (float)iz * tIncrement;

				Vector3 tCalcPos = GetBezierPositionWhenT(calcT, source, target); // Calculate position when t is x
				Vector3 tCalcDir = GetBezierDirectionWhenT(calcT, source, target);
				Quaternion Calcrot = Quaternion.LookRotation(tCalcDir);

				for (int ix = 0; ix < road2D.vertices.Length; ix++)
				{
					Vector3 roadPoint = road2D.vertices[ix].point;
					Vector3 vertexPoint = tCalcPos + (Calcrot * roadPoint);
					vertices.Add(vertexPoint);

					uvs.Add(new Vector2(roadPoint.x / 10.0f + 0.5f, calcT));
				}
			}

			if (isLastOne)
			{
				break;
			}
		}
	}
}
