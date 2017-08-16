using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {

	public GameObject anchor;
	public GameObject payload;
	public GameObject baseElement;
	private bool constructed;
	private float elementSize;
	private List<GameObject> elements;
	// Use this for initialization
	void Start () {
		constructed = false;
		elementSize = 2f;
		elements = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!constructed) {
			Construct ();
			constructed = true;
		}
	}

	private void Construct() {
		//1 get distance
		float numEles = NumberOfElements();
		Vector3 directionStep = DirectionStep ();
		GameObject previous = payload;
		for (int i = 0; i < numEles; i++) {
			GameObject element = Instantiate (baseElement);
			element.transform.position = directionStep * (i + 1) + payload.transform.position;
			element.transform.LookAt (anchor.transform.position);

			HingeJoint hinge = previous.GetComponent<HingeJoint> ();
			hinge.connectedBody = element.GetComponent<Rigidbody> ();

			elements.Add (element);
			previous = element;
		}
		HingeJoint finalHinge = previous.GetComponent<HingeJoint> ();
		finalHinge.connectedBody = anchor.GetComponent<Rigidbody> ();
	}

	private float NumberOfElements() {
		return Mathf.Floor(Vector3.Distance (anchor.transform.position, payload.transform.position) / elementSize);
	}

	private Vector3 DirectionStep() {
		float numEles = NumberOfElements ();
		Vector3 direction = anchor.transform.position - payload.transform.position;
		direction.x = direction.x / numEles;
		direction.y = direction.y / numEles;
		direction.z = direction.z / numEles;
		return direction;
	}
}
