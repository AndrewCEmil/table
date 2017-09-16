using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CanyonCreator : MonoBehaviour {

	// Use this for initialization
	private List<GameObject> terrainMarkers;
	private Vector3[] verticies;
	private float maxX;
	private float width;
	void Start () {
	}

	public void Init() {
		InitVariables ();
	}

	private void InitVariables() {
		maxX = 100f;
		width = maxX;
	}

	public void Generate(Vector3 first, Vector3 second, int gridNum) {
		GenerateGridForPair (first, second, gridNum);
		GenerateForPair (first, second);
	}

	private void GenerateGridForPair(Vector3 first, Vector3 second, int gridNum) {
		Mesh mesh = new Mesh ();
		mesh.name = "Canyon" + gridNum;
		GetComponent<MeshFilter> ().mesh = mesh;

		int length = (int) Mathf.Floor (Vector3.Distance (first, second));
		verticies = new Vector3[((int)width + 1) * (length + 1)];
		Vector2[] uv = new Vector2[verticies.Length];
		for (int i = 0, y = 0; y <= length; y++) {
			for (int x = 0; x <= width; x++, i++) {
				verticies [i] = new Vector3 (x, 0, y);
				uv [i] = new Vector2 ((float)x / width, (float)y / length);
			}
		}

		mesh.vertices = verticies;
		mesh.uv = uv;

		int[] triangles = new int[(int)width * length * 6];
		for (int ti = 0, vi = 0, y = 0; y < length; y++, vi++) {
			for (int x = 0; x < width; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + (int)width + 1;
				triangles[ti + 5] = vi + (int)width + 2;
			}
		}


		mesh.triangles = triangles;

		mesh.RecalculateNormals();
		mesh.MarkDynamic ();
		mesh.RecalculateBounds ();
	}


	//Assume that we have an grid already of the right size
	private void GenerateForPair(Vector3 aPos, Vector3 bPos) {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		Vector3[] baseVerticies = mesh.vertices;

		Vector3 forward;
		Vector3 width;
		Vector3 up;
		float radius = 10f;
		forward = (bPos - aPos).normalized;
		up = (forward + new Vector3 (0, 1, 0)).normalized;
		width = Vector3.Cross (forward, up).normalized;
		up = Vector3.Cross (forward, width).normalized;


		for (int i = 0; i < baseVerticies.Length; i++) {	
			float startX = baseVerticies [i].x;
			float startY = baseVerticies [i].z;
			float theta = Mathf.PI * startX / maxX;
			float newX = Mathf.Cos (theta) * radius;
			float newY = Mathf.Sin (theta) * radius;

			baseVerticies[i] = newX * width + newY * up + startY * forward;
		}

		mesh.vertices = baseVerticies;
		mesh.RecalculateNormals();
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
