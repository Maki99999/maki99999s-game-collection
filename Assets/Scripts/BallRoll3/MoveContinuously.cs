using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class MoveContinuously : MonoBehaviour {

	public Vector3 position0;
	public Vector3 position1;
	public float moveSpeedMultiplier = 1f;

	public Vector3 rotation0;
	public Vector3 rotation1;
	public float rotationSpeedMultiplier = 1f;

	public bool circle;
	public Vector3 circleDirection;

	Vector3 defaultPosition;
	Vector3 defaultRotation;

	public void Reset() {
		StopAllCoroutines();
		transform.position = defaultPosition;
		transform.eulerAngles = defaultRotation;
		Start();
	}
	
	public void ResetSync() {
		StopAllCoroutines();
		transform.position = defaultPosition;
		transform.eulerAngles = defaultRotation;
	}

	public void StartSync() {
		Start();
	}

	void Awake() {
		defaultPosition = transform.position;
		defaultRotation = transform.eulerAngles;
	}

	void Start () {
		defaultPosition = transform.position;
		defaultRotation = transform.eulerAngles;
		
		if((position0.magnitude != 0 || position1.magnitude != 0) && moveSpeedMultiplier != 0) {
			StartCoroutine(Move());
		}

		if(circle) {
			StartCoroutine(Circle());
		} else {
			if((rotation0.magnitude != 0 || rotation1.magnitude != 0) && rotationSpeedMultiplier != 0) {
				StartCoroutine(Rotate());
			}
		}
	}

	IEnumerator Rotate() {
		while(true) {
			for(float i = 0; i <= 1; i += 0.01f * rotationSpeedMultiplier) {
				transform.rotation = Quaternion.Euler(Vector3.Lerp(defaultRotation + rotation0, defaultRotation + rotation1, Mathf.SmoothStep(0f, 1f, i)));
				yield return new WaitForSeconds(1f / 60f);
			}
			
			for(float i = 1; i >= 0; i -= 0.01f * rotationSpeedMultiplier) {
				transform.rotation = Quaternion.Euler(Vector3.Lerp(defaultRotation + rotation0, defaultRotation + rotation1, Mathf.SmoothStep(0f, 1f, i)));
				yield return new WaitForSeconds(1f / 60f);
			}
		}
	}

	IEnumerator Circle() {
		while(true) {
			transform.Rotate(circleDirection);
			yield return new WaitForSeconds(1f / 60f);
		}
	}
	
	IEnumerator Move() {
		while(true) {
			for(float i = 0; i <= 1; i += 0.01f * moveSpeedMultiplier) {
				transform.position = Vector3.Lerp(defaultPosition + position0, defaultPosition + position1, Mathf.SmoothStep(0f, 1f, i));
				yield return new WaitForSeconds(1f / 60f);
			}
			
			for(float i = 1; i >= 0; i -= 0.01f * moveSpeedMultiplier) {
				transform.position = Vector3.Lerp(defaultPosition + position0, defaultPosition + position1, Mathf.SmoothStep(0f, 1f, i));
				yield return new WaitForSeconds(1f / 60f);
			}
		}
	}
}
}