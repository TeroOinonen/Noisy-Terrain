using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [Header("Dimensions")]

    [Tooltip("Size of the plane along the X axis.")]
    public float xSize;

	[Tooltip("Size of the plane along the Z axis.")]
	public float zSize;

	[Tooltip("Number of subdivisions along the X axis.")]
	public float xSubdivisions;

	[Tooltip("Number of subdivisions along the Z axis.")]
	public float zSubdivisions;

	[Header("Material")]
	[SerializeField]
	private Material material;

	private MeshRenderer meshRenderer;
	private MeshFilter meshFilter;
	private MeshCollider meshCollider;

	private void Awake()
	{
		GenerateNewMesh();
	}

	public void GenerateNewMesh()
	{
		// Add the required components
		this.meshRenderer = gameObject.GetComponent<MeshRenderer>();
		this.meshRenderer.material = material;

		this.meshFilter = gameObject.GetComponent<MeshFilter>();
		this.meshCollider = gameObject.GetComponent<MeshCollider>();

		// Create a new Mesh
		Mesh mesh = new Mesh();
		this.meshFilter.mesh = mesh;

		// Create vertices and uv array
		Vector3[] vertices = new Vector3[(int)((xSubdivisions + 1) * (zSubdivisions + 1))];
		Vector2[] uv = new Vector2[vertices.Length];

		// calculate the length of each edge
		float xSubLength = xSize / (float)xSubdivisions;
		float zSubLength = zSize / (float)zSubdivisions;

		// create vertices and uv
		for (int i = 0, z = 0; z <= zSubdivisions; z++)
		{
			for (int x = 0; x <= zSubdivisions; x++, i++)
			{
				vertices[i] = new Vector3(x * xSubLength, 0, z * zSubLength);
				uv[i] = new Vector2((float)x / xSubdivisions, (float)z / zSubdivisions);
			}
		}

		// set the vertices and uv
		mesh.vertices = vertices;
		mesh.uv = uv;

		// create the triangles
		int[] tris = new int[(int)(xSubdivisions * zSubdivisions * 6)];

		for (int ti = 0, vi = 0, y = 0; y < zSubdivisions; y++, vi++)
		{
			for (int x = 0; x < xSubdivisions; x++, ti += 6, vi++)
			{
				tris[ti] = vi;
				tris[ti + 3] = tris[ti + 2] = vi + 1;
				tris[ti + 4] = tris[ti + 1] = (int)(vi + xSubdivisions + 1);
				tris[ti + 5] = (int)(vi + xSubdivisions + 2);
			}
		}

		// set the triangles and recalculate the normals
		mesh.triangles = tris;
		mesh.RecalculateNormals();
	}
}
