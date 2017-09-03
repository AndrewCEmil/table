using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FutureTerrain : MonoBehaviour {

	public int xSize;
	public int ySize;
	private Vector3[] verticies;
	List<GameObject> terrainMarkers;
	// Use this for initialization
	void Start () {
		GameObject[] terrainMarkersArray = GameObject.FindGameObjectsWithTag ("TerrainMarker");
		terrainMarkers = new List<GameObject> (terrainMarkersArray);

		GenerateGrid ();
		Crumple ();
	}

	private void Crumple() {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		Vector3[] baseVerticies = mesh.vertices;
		Vector3[] vertices = new Vector3[baseVerticies.Length];

		float seed = Random.Range (1f, 5f);
		for (var i=0; i < baseVerticies.Length; i++) {
			var vertex = baseVerticies[i];
			float noise = Mathf.PerlinNoise(seed + vertex.x, seed + vertex.y);
			vertex.z = noise;
			vertices[i] = vertex;
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	private void GenerateGrid() {
		Mesh mesh = new Mesh ();
		mesh.name = "FutureTerrain";
		GetComponent<MeshFilter> ().mesh = mesh;
		verticies = new Vector3[(xSize + 1) * (ySize + 1)];
		Vector2[] uv = new Vector2[verticies.Length];
		for (int i = 0, y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++, i++) {
				verticies [i] = new Vector3 (x, y);
				uv [i] = new Vector2 ((float)x / xSize, (float)y / ySize);
			}
		}

		mesh.vertices = verticies;
		mesh.uv = uv;

		int[] triangles = new int[xSize * ySize * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
			for (int x = 0; x < xSize; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
				triangles[ti + 5] = vi + xSize + 2;
			}
		}
		mesh.triangles = triangles;

		mesh.RecalculateNormals();
		mesh.MarkDynamic ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
