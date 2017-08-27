using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour {

	// Use this for initialization
	private bool pushed;
	private Rigidbody rb;
	void Start () {
		pushed = false;
		rb = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!pushed) {
			Push ();
			pushed = true;
		}
	}

	void Push() {
		rb.AddForce (new Vector3 (0, 0, 2000), ForceMode.Impulse);
	}
}
