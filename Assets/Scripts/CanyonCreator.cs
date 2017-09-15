using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lol this code is so ugly...really should fix to make it more reusable etc whatever
public class CanyonCreator : MonoBehaviour {

	// Use this for initialization
	private List<GameObject> terrainMarkers;
	private float maxX;
	void Start () {
		InitVariables ();
		InitTerrainMarkers ();
		Generate ();
	}

	private void InitVariables() {
		maxX = 100f;
	}

	private void InitTerrainMarkers() {
		GameObject[] tunnelMarkersArray = GameObject.FindGameObjectsWithTag ("TerrainMarker");
		terrainMarkers = new List<GameObject> (tunnelMarkersArray);
		terrainMarkers.Sort(delegate(GameObject x, GameObject y)
			{
				return x.name.CompareTo(y.name);
			});
	}

	private void Generate() {
		for (int i = 0; i < terrainMarkers.Count - 1; i++) {
			GenerateForPair (terrainMarkers [i], terrainMarkers [i + 1]);
		}
	}

	//Assume that we have an grid already of the right size
	private void GenerateForPair(GameObject a, GameObject b) {
		MeshFilter meshFilter = gameObject.GetComponent<MeshFilter> ();
		Mesh mesh = meshFilter.mesh;
		Vector3[] baseVerticies = mesh.vertices;

		Vector3 forward;
		Vector3 width;
		Vector3 up;
		float radius = 10f;
		forward = (b.transform.position - a.transform.position).normalized;
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
