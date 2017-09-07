using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureTerrainGenerator : MonoBehaviour {


	GameObject baseFutureTerrain;
	List<GameObject> terrainMarkers;
	List<GameObject> futureTerrains;
	void Start () {
		InitTerrainMarkers ();
		baseFutureTerrain = GameObject.Find ("FutureTerrain");
		futureTerrains = new List<GameObject> ();
		Generate ();
	}
	private void InitTerrainMarkers() {
		GameObject[] terrainMarkersArray = GameObject.FindGameObjectsWithTag ("TerrainMarker");
		terrainMarkers = new List<GameObject> (terrainMarkersArray);
		terrainMarkers.Sort(delegate(GameObject x, GameObject y)
			{
				return x.name.CompareTo(y.name);
			});
	}

	private void Generate() {
		for (int i = 1; i < terrainMarkers.Count; i++) {
			GenerateGrid(terrainMarkers [i - 1].transform.position, terrainMarkers [i].transform.position, i);
		}
	}

	private void GenerateGrid(Vector3 aPos, Vector3 bPos, int count) {
		GameObject ft = Instantiate (baseFutureTerrain);
		FutureTerrain futureTerrain = ft.GetComponent<FutureTerrain> ();
		futureTerrain.Init ();

		futureTerrain.GenerateGridForPair (aPos, bPos, count);
		futureTerrains.Add (ft);
	}

}
