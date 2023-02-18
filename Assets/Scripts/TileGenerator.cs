using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
	[Header("Parameters")]
	public int noiseSampleSize;
	public float scale;
	public int textureResolution = 1;
	public float maxHeight = 1.0f;

	[Header("Terrain Types")]
	public TerrainType[] heightTerrainTypes;

	private MeshRenderer tileMeshRenderer;
	private MeshFilter tileMeshFilter;
	private MeshCollider tileMeshCollider;


	private void Start()
	{
		tileMeshRenderer = GetComponent<MeshRenderer>();
		tileMeshFilter = GetComponent<MeshFilter>();
		tileMeshCollider = GetComponent<MeshCollider>();

		GenerateTile();
	}

	private void GenerateTile()
	{
		float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);

		float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, textureResolution);

		Vector3[] verts = tileMeshFilter.mesh.vertices;

		for (int x = 0; x < noiseSampleSize; x++)
		{
			for (int z = 0; z < noiseSampleSize; z++)
			{
				int index = (x * noiseSampleSize) + z;
				
				verts[index].y = heightMap[x, z] * maxHeight;
			}
		}

		tileMeshFilter.mesh.vertices = verts;
		tileMeshFilter.mesh.RecalculateBounds();
		tileMeshFilter.mesh.RecalculateNormals();

		tileMeshCollider.sharedMesh = tileMeshFilter.mesh;

		Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

		tileMeshRenderer.material.mainTexture = heightMapTexture;
	}
}
