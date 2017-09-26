using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCreator : MonoBehaviour {


	GameObject basePillar;
	GameObject baseWall;
	List<GameObject> terrainMarkers;
	List<Vector3> leftPillars;
	List<Vector3> rightPillars;
	float width;
	float height;
	void Start () {
		InitTerrainMarkers ();
		basePillar = GameObject.Find ("BasePillar");
		baseWall = GameObject.Find ("BaseWall");
		leftPillars = new List<Vector3> ();
		rightPillars = new List<Vector3> ();
		width = 10f;
		height = 10f;
		Generate ();
		basePillar.SetActive (false);
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
		CreatePillars ();
		CreateWalls ();
	}

	private void CreateWalls() {
		CreateLeftWalls ();
		CreateRightWalls ();
	}

	private void CreateLeftWalls() {
		for (int i = 0; i < leftPillars.Count - 1; i++) {
			CreateWall (leftPillars [i], leftPillars [i + 1], i);
			CreatePillarTemp (leftPillars [i]);
		}
		CreatePillarTemp (leftPillars [leftPillars.Count - 1]);
	}

	private void CreatePillarTemp(Vector3 pos) {
		GameObject ft = Instantiate (basePillar);
		ft.transform.position = pos;
	}

	private void CreateRightWalls() {
		for (int i = 0; i < rightPillars.Count - 1; i++) {
			CreateWall (rightPillars [i], rightPillars [i + 1], i);
			CreatePillarTemp (rightPillars [i]);
		}
		CreatePillarTemp (rightPillars [rightPillars.Count - 1]);
	}

	private void CreateWall(Vector3 start, Vector3 end, int count) {
		Vector3 forward = (end - start).normalized;
		GameObject wall = Instantiate(baseWall);
		Mesh mesh = new Mesh ();
		mesh.name = "Wall" + count;
		wall.GetComponent<MeshFilter> ().mesh = mesh;


		int length = (int) Mathf.Floor (Vector3.Distance (start, end));
		Vector3[] verticies = new Vector3[((int)height + 1) * (length + 1)];
		Vector2[] uv = new Vector2[verticies.Length];
		for (int i = 0, y = 0; y <= height; y++) {
			for (float x = 0; x <= length; x++, i++) {
				verticies [i] = (x / length) * forward * length + new Vector3 (0, y, 0) + start;
				uv [i] = new Vector2 (x / length, (float)y / height);
			}
		}

		mesh.vertices = verticies;
		mesh.uv = uv;

		int[] triangles = new int[(int)height * length * 6];
		for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++) {
			for (int x = 0; x < length; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 4] = triangles[ti + 1] = vi + (int)length + 1;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 5] = vi + (int)length + 2;
			}
		}


		mesh.triangles = triangles;

		mesh.RecalculateNormals();
		mesh.MarkDynamic ();
		mesh.RecalculateBounds ();
	}

	private void CreatePillars() {
		CreateFirstPillars();

		for (int i = 1; i < terrainMarkers.Count - 1; i++) {
			CreatePillarPair (terrainMarkers [i - 1].transform.position, terrainMarkers [i].transform.position, terrainMarkers [i + 1].transform.position, i);
		}

		CreateEndPillars ();
	}

	/*
	 * Create two sets of walls, one on each side
	 * 1: Go through and create "pillars" one on each side
		a: Have 3 points
		b: Get forward vector from pt 0 to pt 1
		c: Get angle theta to from forward to point 2
		d: Create two pillars on either side of "forward" from pt
		e: Rotate pillars theta
		f: Save pillars
	 * 2: Go through and create walls between each pillar
	 * 3: Add noise to all walls
	 */
	public void CreatePillarPair(Vector3 previous, Vector3 current, Vector3 next, int count) {
		Vector3 forward;
		Vector3 nextForward;
		Vector3 side;
		Vector3 up;
		forward = (current - previous).normalized;
		nextForward = (next - current).normalized;
		up = (forward + new Vector3 (0, 1, 0)).normalized;
		side = Vector3.Cross (forward, up).normalized;
		up = Vector3.Cross (forward, side).normalized;
		float theta = (Vector3.Angle (forward, nextForward)) / -2f;
		Quaternion rot = Quaternion.AngleAxis (theta, up);
		Vector3 leftPillar = -1f * side * width;
		Vector3 rightPillar = side * width;
		leftPillar = rot * leftPillar;
		rightPillar = rot * rightPillar;
		leftPillar = leftPillar + current;
		rightPillar = rightPillar + current;
		leftPillars.Add (leftPillar);
		rightPillars.Add (rightPillar);
	}

	private void CreateFirstPillars() {
		Vector3 current = terrainMarkers [0].transform.position;
		Vector3 next = terrainMarkers [1].transform.position;
		Vector3 forward;
		Vector3 side;
		Vector3 up;
		forward = (next - current).normalized;
		up = (forward + new Vector3 (0, 1, 0)).normalized;
		side = Vector3.Cross (forward, up).normalized;
		up = Vector3.Cross (forward, side).normalized;
		Vector3 leftPillar = -1f * side * width + current;
		Vector3 rightPillar = side * width + current;
		leftPillars.Add (leftPillar);
		rightPillars.Add (rightPillar);
	}

	private void CreateEndPillars() {
		Vector3 current = terrainMarkers [terrainMarkers.Count - 1].transform.position;
		Vector3 next = terrainMarkers [terrainMarkers.Count - 2].transform.position;
		Vector3 forward;
		Vector3 side;
		Vector3 up;
		forward = (next - current).normalized;
		up = (forward + new Vector3 (0, 1, 0)).normalized;
		side = Vector3.Cross (forward, up).normalized;
		up = Vector3.Cross (forward, side).normalized;
		//These are reversed from above because current/next are reversed
		Vector3 leftPillar = side * width + current;
		Vector3 rightPillar = -1f * side * width + current;
		leftPillars.Add (leftPillar);
		rightPillars.Add (rightPillar);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
