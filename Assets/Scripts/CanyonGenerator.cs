using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanyonGenerator : MonoBehaviour {

	// Use this for initialization
	private GameObject baseCanyon;
	private List<GameObject> terrainMarkers;
	private List<GameObject> canyons;
	void Start () {
		canyons = new List<GameObject> ();
		baseCanyon = GameObject.Find ("BaseCanyon");
		InitTerrainMarkers ();
		Generate ();
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
			GenerateGrid (terrainMarkers [i].transform.position, terrainMarkers [i + 1].transform.position, i);
		}
	}

	private void GenerateGrid(Vector3 aPos, Vector3 bPos, int count) {
		GameObject canyon = Instantiate (baseCanyon);
		CanyonCreator canyonCreator = canyon.GetComponent<CanyonCreator> ();
		canyonCreator.Init ();
		canyonCreator.Generate (aPos, bPos, count);
		canyons.Add (canyon);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
