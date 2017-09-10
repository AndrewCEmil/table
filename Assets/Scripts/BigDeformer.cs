using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDeformer : MonoBehaviour {

	public float maxDistance;
	public float maxHeight;
	private Vector3 middlePosition;
	private Vector3 storedVector;
	bool deformed;
	int count;
	private float maxX;
	private float maxZ;
	private List<Vector3> worlyPoints;
	void Start() {
		deformed = false;
		count = 0;
		middlePosition = new Vector3 (500, 0, 500);
		storedVector = new Vector3 (0, 0, 0);
		maxX = 1000f;
		maxZ = 1000f;
		InitWorlyPoints ();
	}

	private void InitWorlyPoints() {
		float numRandoms = 10;
		worlyPoints = new List<Vector3> ();
		for (int i = 0; i < numRandoms; i++) {
			worlyPoints.Add (new Vector3 (Random.Range (0, maxX), 0, Random.Range (0, maxZ)));
		}
	}

	// Update is called once per frame
	void Update () {
		if(!deformed) {
			WorlyDeform ();
		}
	}

	void WorlyDeform() {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		Vector3[] baseVerticies = mesh.vertices;
		Vector3[] vertices = new Vector3[baseVerticies.Length];

		UpdateWorlyPoints();

		for (int i = 0; i < baseVerticies.Length; i++) {
			Vector3 vertex = baseVerticies[i];
			float noise = WorlyNoise (vertex);
			vertex.y = noise * GetScale (vertex) - 10f;
			vertices[i] = vertex;
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	private void UpdateWorlyPoints() {
		Vector3 curPoint;
		Vector3 up = new Vector3 (0, 1, 0);
		for (int i = 0; i < worlyPoints.Count; i++) {
			curPoint = worlyPoints [i] - middlePosition;
			worlyPoints [i] = Quaternion.AngleAxis (Time.deltaTime * 10f, up) * curPoint + middlePosition;
		}
	}

	private float WorlyNoise (Vector3 point) {
		return MinDistance (point, worlyPoints) / maxDistance;
	}

	private float MinDistance(Vector3 point, List<Vector3> points) {
		point.y = 0f;
		float minDist = float.MaxValue;
		foreach (Vector3 p in points) {
			minDist = Mathf.Min (minDist, Vector3.Distance (point, p));
		}
		return minDist;
	}

	void PerlinDeform() {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		Vector3[] baseVerticies = mesh.vertices;
		Vector3[] vertices = new Vector3[baseVerticies.Length];

		float timez = (Time.time + 3f) / 5f;
		for (var i=0; i < baseVerticies.Length; i++) {
			Vector3 vertex = baseVerticies[i];
			float scale = GetScale (vertex);
			float noise = Mathf.PerlinNoise (timez + vertex.x, timez + vertex.z);
			vertex.y = noise * scale;
			vertices[i] = vertex;
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	float GetScale(Vector3 position) {
		return maxHeight * ScaleFunction (position);
	}

	float ScaleFunction(Vector3 position) {
		return (GetRadius (position) / maxDistance);
	}

	float GetRadius(Vector3 position) {
		storedVector.x = position.x;
		storedVector.z = position.z;
		return Vector3.Distance (storedVector, middlePosition);
	}

}
