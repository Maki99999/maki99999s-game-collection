using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class BossRing : MonoBehaviour {
	
	Vector3 vel;

	public Transform[] circles;

	void Start () {
		vel = Vector3.zero;
	}

	void Update () {
		circles[0].Rotate(vel * Time.deltaTime * 10f);
		circles[1].Rotate(-1 * vel * Time.deltaTime * 10f);
	}

	public IEnumerator ResetRotation() {
		StopAllCoroutines();
		for(float f = 0; f <= 1f; f += 0.01f) {
			circles[0].rotation = Quaternion.Lerp(circles[0].rotation, Quaternion.identity, f);
			circles[1].rotation = Quaternion.Lerp(circles[1].rotation, Quaternion.identity, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}

	public void StartRotating() {
		StartCoroutine(ChangeVel(Random.onUnitSphere * 0.6f));
	}

	public void ChangeSpeed(float speed) {
		StartCoroutine(ChangeVel(vel * speed));
	}

	public void StopRotating() {
		StartCoroutine(ChangeVel(Vector3.zero));
	}

	IEnumerator ChangeVel(Vector3 newVel) {
		for(float f = 0; f <= 1f; f += 0.01f) {
			vel = Vector3.Lerp(vel, newVel, f);
			yield return new WaitForSeconds(1f / 60f);
		}
	}
}
}