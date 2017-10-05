using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputManager : MonoBehaviour {

	public GameObject controllerMain;
	public GameObject controllerPointer;
	// Use this for initialization
	void Start () {
		ActivateController ();
	}
	
	private void ActivateController() {
		controllerMain.SetActive(true);
		controllerPointer.SetActive(true);

		GvrLaserPointer pointer = controllerPointer.GetComponentInChildren<GvrLaserPointer>(true);
		GvrPointerInputModule.Pointer = pointer;
	}
}
