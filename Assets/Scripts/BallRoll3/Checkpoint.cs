using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class Checkpoint : MonoBehaviour {

	public Vector3 offset;
	public float newDeathHeight;

	bool activated = false;
	
	void OnTriggerEnter (Collider other) {
		if(!activated && other.CompareTag("Player")) {
			activated = true;
			BallMovement ball = other.GetComponent<BallMovement>();
			ball.SetCheckpoint(transform.position + offset, newDeathHeight);
		}
	}
}
}