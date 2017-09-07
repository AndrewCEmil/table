using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelCreator : MonoBehaviour {

	private List<GameObject> tunnelMarkers;
	private Vector3[] verticies;
	private int[] triangles;
	private int radialGranularity;
	private float width;
	void Start () {
		radialGranularity = 4;
		width = 5;
		InitTunnelMarkers ();
		Generate ();
	}
	private void InitTunnelMarkers() {
		GameObject[] tunnelMarkersArray = GameObject.FindGameObjectsWithTag ("TunnelMarker");
		tunnelMarkers = new List<GameObject> (tunnelMarkersArray);
		tunnelMarkers.Sort(delegate(GameObject x, GameObject y)
			{
				return x.name.CompareTo(y.name);
			});
	}

	private void Generate() {
		FillVerticies ();
		FillTriangles ();

		Mesh mesh = new Mesh ();
		mesh.name = "Tunnel";
		GetComponent<MeshFilter> ().mesh = mesh;
		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	}

	private void FillVerticies() {
		verticies = new Vector3[radialGranularity * tunnelMarkers.Count];
		for(int i = 0; i < tunnelMarkers.Count; i++) {
			FillVerticiesForMarker (tunnelMarkers [i], i);
		}
	}

	//TODO split between previous + next
	private void FillVerticiesForMarker(GameObject tunnelMarker, int i) {
		Vector3 forward;
		Vector3 orthog0;
		Vector3 orthog1;
		Vector3 markerPosition = tunnelMarker.transform.position;
		if (i == tunnelMarkers.Count - 1) {
			forward = new Vector3 (0, 0, 1);
		} else {
			forward = (tunnelMarkers [i + 1].transform.position - tunnelMarker.transform.position).normalized;
		}
		//TODO I dont think this is actually right...
		orthog0 = (forward + new Vector3 (0, 1, 0)).normalized;
		orthog1 = Vector3.Cross (forward, orthog0).normalized;
		orthog0 = Vector3.Cross (forward, orthog1).normalized;

		//TODO generate circle points...for now hard coded
		verticies[i * radialGranularity] = markerPosition + (orthog0 + orthog1) * width;
		verticies[i * radialGranularity + 1] = markerPosition + (orthog0 - orthog1) * width;
		verticies [i * radialGranularity + 2] = markerPosition - (orthog0 + orthog1) * width;
		verticies[i * radialGranularity + 3] = markerPosition + (orthog1 - orthog0) * width;
	}

	private void FillTriangles() {
		triangles = new int[radialGranularity * 6 * (tunnelMarkers.Count - 1)];
		for (int i = 0; i < tunnelMarkers.Count - 1; i++) {
			FillTrianglesForSegment (i);
		}
	}

	private void FillTrianglesForSegment(int i) {
		int numTrianglesPerSegment = radialGranularity * 2;
		int startIndex = i * numTrianglesPerSegment;
		//for each vertex on segment build 2 triangles:
			//point, up, right
			//up, up + right, right
		for (int v = 0; v < radialGranularity; v++) {
			int current = i * radialGranularity + v;
			int forward = (i + 1) * radialGranularity + v;
			int right = i  * radialGranularity + v + 1;
			int forwardRight = (i + 1) * radialGranularity + v + 1;
			if (v == radialGranularity - 1) {
				right = (i  * radialGranularity);
				forwardRight = (i + 1) * radialGranularity;
			}

			triangles [startIndex + v * 6] = current;
			triangles [startIndex + v * 6 + 1] = forward;
			triangles [startIndex + v * 6 + 2] = right;

			triangles [startIndex + v * 6 + 3] = forward;
			triangles [startIndex + v * 6 + 4] = forwardRight;
			triangles [startIndex + v * 6 + 5] = right;
		}
	}
}
