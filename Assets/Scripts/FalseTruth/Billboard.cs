using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class Billboard : MonoBehaviour {

	void Update () {
		Transform camera = Camera.main.transform;	//Get the main camera
		Vector3 targetPostition = new Vector3(camera.position.x, this.transform.position.y, camera.position.z);		//Get the camera position
		transform.LookAt(targetPostition);		//Forces the object to look at the camera
	}
}
}