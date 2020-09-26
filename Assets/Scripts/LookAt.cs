using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

	public Transform lookAt;

	void Start() {
		transform.LookAt(lookAt);
	}

	void Update () {
		transform.LookAt(lookAt);
	}
}
