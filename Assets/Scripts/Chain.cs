using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

	// Use this for initialization
	public GameObject baseLink;
	public GameObject anchor;
	private Vector3 connectedAnchorPosition;
	private bool attached;
	void Start () {
		connectedAnchorPosition = new Vector3 (0, 0, -1f);
		attached = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!attached) {
			AttachFirstChain ();
			attached = true;
		}
	}

	void AttachFirstChain() {
		baseLink.AddComponent<HingeJoint> ();
		HingeJoint hinge = baseLink.GetComponent<HingeJoint> ();
		hinge.autoConfigureConnectedAnchor = false;
		hinge.connectedAnchor = connectedAnchorPosition;
		hinge.connectedBody = anchor.GetComponent<Rigidbody> ();
		hinge.enableCollision = true;
	}
}
