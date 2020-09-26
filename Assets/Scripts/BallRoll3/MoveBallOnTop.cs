using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class MoveBallOnTop : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			other.transform.parent = transform;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			other.transform.parent = null;
		}
	}
}
}