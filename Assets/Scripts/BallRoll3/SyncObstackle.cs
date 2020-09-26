using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class SyncObstackle : MonoBehaviour {
	
	public MoveContinuously[] obstackles;

	public float timeBetweenObstacklesActivation = 0.6f;

	bool isSyncing = false;

	void Start() {
		foreach(MoveContinuously obstackle in obstackles) {
			obstackle.enabled = false;
		}
	}

	void OnTriggerEnter (Collider other) {
		if(!isSyncing && other.CompareTag("Player")) {
			isSyncing = true;
			StartCoroutine(Sync());
		}
	}

	IEnumerator Sync() {
		foreach(MoveContinuously obstackle in obstackles) {
			obstackle.StartSync();
			yield return new WaitForSeconds(timeBetweenObstacklesActivation);
		}
	}

	public void Reset() {
		isSyncing = false;
		foreach(MoveContinuously obstackle in obstackles) {
			obstackle.ResetSync();
			obstackle.enabled = false;
		}
	}
}
}