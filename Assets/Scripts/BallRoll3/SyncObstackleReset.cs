using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class SyncObstackleReset : MonoBehaviour {
	
	SyncObstackle syncObstackle;

	bool isResetting = false;

	private void Start() {
		syncObstackle = GetComponentInParent<SyncObstackle>();
	}

	void OnTriggerEnter (Collider other) {
		if(!isResetting && other.CompareTag("Player")) {
			isResetting = true;
			syncObstackle.Reset();
		}
	}

	void OnTriggerExit (Collider other) {
		if(isResetting && other.CompareTag("Player")) {
			isResetting = false;
		}
	}
}
}