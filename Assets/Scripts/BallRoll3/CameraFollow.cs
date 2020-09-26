using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class CameraFollow : MonoBehaviour {

	public Transform lookAtObject;
	public Vector3 offset;
	public float maxTilt;
	[HideInInspector] public Vector2 input;
	[HideInInspector] public bool stopMoving = false;
	public Vector3 forward = Vector3.forward;
	public Vector3 right = Vector3.right;

	float velocityX = 0f;
	float x;
	float velocityZ = 0f;
	float z;

	void FixedUpdate() {
		if(stopMoving) {
			return;
		}
		
		transform.localPosition = lookAtObject.localPosition + offset;
		transform.LookAt(lookAtObject);


		x = Mathf.SmoothDamp(x, input.y, ref velocityX, .3f);
		transform.RotateAround(lookAtObject.position, right, -maxTilt * x);
		
		z = Mathf.SmoothDamp(z, input.x, ref velocityZ, .3f);
		transform.RotateAround(lookAtObject.position, forward, maxTilt * z);
	}

	public IEnumerator JustLookAt(Transform target) {
		while(true) {
			Quaternion neededRotation = Quaternion.LookRotation(target.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * 5f);
			yield return null;
		}
	}
}
}