using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutMesh : MonoBehaviour
{
	private MeshFilter mf;

	[Range(3, 255)]
	public int NumberOfPoints = 8;

	public float InnerRadius = 1.0f;

	public float OuterRadius = 3.0f;

	private const float tau = 2 * Mathf.PI;

	private void Awake()
	{
		mf = GetComponent<MeshFilter>();
	}

	// Start is called before the first frame update
	void Start()
	{
		GenerateMesh();
	}

	private void OnValidate()
	{
		GenerateMesh();
	}

	private void GenerateMesh()
	{
		Mesh mesh = new Mesh();

		List<Vector3> verts = new List<Vector3>();

		verts.Add(Vector3.up * InnerRadius);
		verts.Add(Vector3.up * OuterRadius);

		for (int i = 1; i < NumberOfPoints; i++)
		{
			float theta = tau * i / NumberOfPoints; // Angle of current iteration (in radians)
			Debug.Log("Angle radians:" + theta + " Angle degrees:" + 360f * theta / tau);

			Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);

			verts.Add(v * InnerRadius);
			verts.Add(v * OuterRadius);
		}

		List<int> tri_indices = new List<int>();

		for (int i = 0; i < NumberOfPoints; i++)
		{
			tri_indices.Add(i * 2);
			tri_indices.Add(i + 1);
			tri_indices.Add(i + 2);

			tri_indices.Add(i * 2);
			tri_indices.Add(i + 2);
			tri_indices.Add(i + 3);
		}


		mesh.SetVertices(verts);
		mesh.SetTriangles(tri_indices, 0);

		mesh.RecalculateNormals();

		if (mf != null)
		{
			mf.sharedMesh = mesh;
		}
	}
}
