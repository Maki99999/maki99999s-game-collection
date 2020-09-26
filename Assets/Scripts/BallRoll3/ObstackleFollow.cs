using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class ObstackleFollow : MonoBehaviour {

	public Transform ball;
	public Transform obstackle;
	public float fromX;
	public float toX;

	MoveContinuously moveContinuously;

	bool activated = false;

	void Start() {
		moveContinuously = obstackle.GetComponent<MoveContinuously>();
	}
	
	void OnTriggerEnter (Collider other) {
		if(!activated && other.CompareTag("Player")) {
			activated = true;
			moveContinuously.StopAllCoroutines();
			StartCoroutine(Follow());
		}
	}

	void OnTriggerExit (Collider other) {
		if(activated && other.CompareTag("Player")) {
			activated = false;
		}
	}

	IEnumerator Follow() {
		float percent = 0f;
		for(float f = 0; f <= 0.9f; f += 0.01f) {
			percent = ((ball.position.x - fromX) / (toX - fromX));
			obstackle.position = Vector3.Lerp(obstackle.position, new Vector3(ball.position.x + 1.4f + 2f * percent, obstackle.position.y, obstackle.position.z), f);
			yield return new WaitForSeconds(1f / 60f);
		}
		while(enabled) {
			percent = ((ball.position.x - fromX) / (toX - fromX));
			obstackle.position = new Vector3(ball.position.x + 1.4f + 2f * percent, obstackle.position.y, obstackle.position.z);
			yield return null;
		}
	}
}
}