using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMesh : MonoBehaviour
{
    private MeshFilter mf;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] verts = {
            new Vector3(-1f, 1f, 0f),
			new Vector3( 1f, 1f, 0f),
		    new Vector3(-1f,-1f, 0f),
		    new Vector3( 1f,-1f, 0f) };

        int[] tri_indices = { 
            0, 1, 2, 
            1, 3, 2 };

        mesh.vertices = verts;
        mesh.SetTriangles(tri_indices, 0);

        mesh.RecalculateNormals();

        mf.sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
