using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMesh : MonoBehaviour
{
	private MeshFilter mf;

	[Range(3, 255)]
	public int NumberOfPoints = 8;

	public float Radius = 1.0f;

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

		verts.Add(Vector3.zero);

		for (int i = 0; i < NumberOfPoints; i++)
		{
			float theta = tau * i / NumberOfPoints; // Angle of current iteration (in radians)
			//Debug.Log("Angle radians:" + theta + " Angle degrees:" + 360f * theta / tau);

			Vector3 v = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);

			verts.Add(v * Radius);
		}

		List<int> tri_indices = new List<int>();

		for (int i = 1; i < NumberOfPoints; i++)
		{
			tri_indices.Add(0);
			tri_indices.Add(i);
			tri_indices.Add(i + 1);
		}

		tri_indices.Add(0);
		tri_indices.Add(NumberOfPoints);
		tri_indices.Add(1);


		mesh.SetVertices(verts);
		mesh.SetTriangles(tri_indices, 0);

		mesh.RecalculateNormals();

		if (mf != null)
		{
			mf.sharedMesh = mesh;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}

