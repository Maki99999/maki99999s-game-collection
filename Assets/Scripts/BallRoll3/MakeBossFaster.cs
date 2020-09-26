using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class MakeBossFaster : MonoBehaviour {

	public Boss boss;
	public float newSpeed;

	void OnTriggerEnter (Collider other) {
		if(other.CompareTag("Player")) {
			boss.moveTowardsPlayerSpeed = newSpeed;
		}
	}
}
}