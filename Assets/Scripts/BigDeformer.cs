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
	void Start() {
		deformed = false;
		count = 0;
		middlePosition = new Vector3 (500, 0, 500);
		storedVector = new Vector3 (0, 0, 0);
	}

	// Update is called once per frame
	void Update () {
		if(!deformed) {
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
