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
	void Start() {
		deformed = false;
		count = 0;
		middlePosition = new Vector3 (500, 0, 500);
		storedVector = new Vector3 (0, 0, 0);
		maxX = 1000f;
		maxZ = 1000f;
	}

	// Update is called once per frame
	void Update () {
		if(!deformed) {
			WorlyDeform ();
			deformed = true;
		}
	}

	void WorlyDeform() {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		Vector3[] baseVerticies = mesh.vertices;
		Vector3[] vertices = new Vector3[baseVerticies.Length];

		//Pick random points
		float numRandoms = 10;
		List<Vector3> randomPoints = new List<Vector3> ();
		for (int i = 0; i < numRandoms; i++) {
			randomPoints.Add (new Vector3 (Random.Range (0, maxX), 0, Random.Range (0, maxZ)));
		}

		//Iterate over each vertex
		for (int i = 0; i < baseVerticies.Length; i++) {
			//Get min distance
			Vector3 vertex = baseVerticies[i];
			//float scale = GetScale (vertex);
			//Set height to distance
			float noise = WorlyNoise (vertex, randomPoints);
			vertex.y = noise * maxHeight * 3f;
			vertices[i] = vertex;
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	private float WorlyNoise(Vector3 point, List<Vector3> points) {
		return MinDistance (point, points) / maxDistance;
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
