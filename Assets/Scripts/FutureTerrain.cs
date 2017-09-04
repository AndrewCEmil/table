using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FutureTerrain : MonoBehaviour {

	private int width;
	private Vector3[] verticies;
	List<GameObject> terrainMarkers;
	// Use this for initialization
	void Start () {
		width = 10;

		InitTerrainMarkers ();
		GenerateGrid ();
		//Crumple ();
	}

	private void InitTerrainMarkers() {
		GameObject[] terrainMarkersArray = GameObject.FindGameObjectsWithTag ("TerrainMarker");
		terrainMarkers = new List<GameObject> (terrainMarkersArray);
		terrainMarkers.Sort(delegate(GameObject x, GameObject y)
			{
				return x.name.CompareTo(y.name);
			});
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
		for (int i = 1; i < terrainMarkers.Count; i++) {
			GenerateGridForPair (terrainMarkers [i - 1].transform.position, terrainMarkers [i].transform.position, i);
		}
	}

	//TODO make sure y vs z is right here
	private void GenerateGridForPair(Vector3 first, Vector3 second, int gridNum) {
		Mesh mesh = new Mesh ();
		mesh.name = "FutureTerrain" + gridNum;
		GetComponent<MeshFilter> ().mesh = mesh;

		Vector3 yAxis = new Vector3 (0, (second.y + first.y) / 2f + 1, 0).normalized;
		Vector3 lineAxis = (second - first).normalized;
		Vector3 widthAxis = Vector3.Cross (lineAxis, yAxis).normalized;
		Vector3 topLeft = second + (widthAxis * width / 2f);
		Vector3 bottomRight = first - (widthAxis * width / 2f);
		Vector3 origin = bottomRight;
		int length = (int) Mathf.Floor (Vector3.Distance (first, second));

		verticies = new Vector3[(width + 1) * (length + 1)];
		Vector2[] uv = new Vector2[verticies.Length];
		for (int i = 0, y = 0; y <= length; y++) {
			for (int x = 0; x <= width; x++, i++) {
				verticies [i] = GetPoint (x, y, lineAxis, widthAxis, origin);
				uv [i] = new Vector2 ((float)x / width, (float)y / length);
			}
		}

		mesh.vertices = verticies;
		mesh.uv = uv;

		int[] triangles = new int[width * length * 6];
		for (int ti = 0, vi = 0, y = 0; y < length; y++, vi++) {
			for (int x = 0; x < width; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 4] = triangles[ti + 1] = vi + 1;
				triangles[ti + 3] = triangles[ti + 2] = vi + width + 1;
				triangles[ti + 5] = vi + width + 2;
			}
		}


		mesh.triangles = triangles;

		mesh.RecalculateNormals();
		Vector3[] normals = mesh.normals;
		for(int i = 0; i < normals.Length; i++) {
			normals [i] = yAxis;
		}
		mesh.normals = normals;
		mesh.MarkDynamic ();
	}


	private Vector3 GetPoint(int x, int y, Vector3 lineAxis, Vector3 widthAxis, Vector3 origin) {
		return (lineAxis * y) + (widthAxis * x) + origin;
	}
	
	// Update is called once per frame
	void Update () {
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] normals = mesh.normals;
		Debug.Log ("test");
	}
}
