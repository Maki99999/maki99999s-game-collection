using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class FirstPersonController : MonoBehaviour {

	public float sensitivityX = 2f;
	public float sensitivityY = 2f;
	
	public readonly float walkSpeed = 5f;
	public readonly float sneakSpeed = 2f;

	public float height = 1.6f;
	public float sneakHeight = 1.2f;

	public float gravity = 0.2f;

	public bool canMove = false;
	public bool isSneaking = false;

	public bool clampY = false;
	public float clampYMin = 0;
	public float clampYMax = 0;

	Transform cam;
	
	Quaternion characterTargetRot;
	Quaternion cameraTargetRot;
	
	CharacterController charController;

	float speed;
	float speedMultiplier = 1f;
	
	void Start () {
		cam = transform.GetChild(0);
		cam.localPosition = new Vector3(0f, height / 3.2f, 0f);

		charController = GetComponent<CharacterController>();

		speed = walkSpeed;
	}

	void Update () {
		Move();

		characterTargetRot = transform.localRotation;
		cameraTargetRot = cam.localRotation;

		float xRot = Input.GetAxis("Mouse Y") * sensitivityY;
		float yRot = Input.GetAxis("Mouse X") * sensitivityX;

		characterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		cameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);
		if(clampY)
			characterTargetRot = ClampRotationAroundYAxis(characterTargetRot);

		transform.localRotation = characterTargetRot;
		cam.localRotation = cameraTargetRot;
	}

	void Move() {
		if(canMove) {
			if(Input.GetAxis("Sneak") > 0) {
				if(!isSneaking) {
					StopAllCoroutines();
					StartCoroutine(SneakOn());
				}
			} else {
				if(isSneaking) {
					StopAllCoroutines();
					StartCoroutine(SneakOff());
				}
			}

			if(isSneaking) {
				speed = sneakSpeed * speedMultiplier;
			} else {
				speed = walkSpeed * speedMultiplier;
			}

			Vector2 input = UserSettings.usingMouse ? new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) : new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		
			input.Normalize();
			input *= speed * Time.deltaTime * 40;
		
			Vector3 movement = transform.forward * input.y + transform.right * input.x;

			if(!charController.isGrounded) {
				movement -= transform.up * gravity;
			}
			charController.Move(movement * 0.015f);
		}
	}

	IEnumerator SneakOn() {
		isSneaking = true;
		speed = sneakSpeed;

		Vector3 oldCamPos = cam.localPosition;

		for(float i = 0; i < 1.0f; i = i + 0.2f) {
			cam.localPosition = Vector3.Lerp(oldCamPos, new Vector3(0f, sneakHeight / 3.2f, 0f), i);
			yield return new WaitForSeconds(1f / 60f);
		}
		
		cam.localPosition = new Vector3(0f, sneakHeight / 3.2f, 0f);
	}

	IEnumerator SneakOff() {
		isSneaking = false;

		Vector3 oldCamPos = cam.localPosition;

		for(float i = 0; i < 1; i = i + 0.2f) {
			cam.localPosition = Vector3.Lerp(oldCamPos, new Vector3(0f, height / 3.2f, 0f), i);
			yield return new WaitForSeconds(1f / 60f);
		}

		cam.localPosition = new Vector3(0f, height / 3.2f, 0f);

		speed = walkSpeed;
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q) {
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, -90, 90);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

	Quaternion ClampRotationAroundYAxis(Quaternion q) {
		Vector3 eulerAngles = q.eulerAngles;

		float yAngle = eulerAngles.y;
		
		if(yAngle < clampYMin && yAngle >= 90) {
			yAngle = 160f;
		}
		if(yAngle > clampYMax && yAngle < 90) {
			yAngle = 20;
		}
		eulerAngles.y = yAngle;

		return Quaternion.Euler(eulerAngles);
	}

	public void SetSpeedMultiplier(float speed) {
		speedMultiplier = speed;
	}
}
}