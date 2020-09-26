using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class FirstPersonController : MonoBehaviour {

	public float sensitivityX = 2f;
	public float sensitivityY = 2f;

	public float sprintSpeed = 10f;		//Readonly
	public float walkSpeed = 5f;		//Readonly
	public float sneakSpeed = 2f;		//Readonly

	public float height = 1.8f;			//Readonly
	public float sneakHeight = 1.3f;	//Readonly

	public float jumpMultiplier = 1f;

	public float gravity = 9.81f;

	public bool canMove = true;
	public bool isSneaking = false;
	public bool isSprinting = false;


	Transform cam;
	Camera camComponent;
	
	Quaternion characterTargetRot;
	Quaternion cameraTargetRot;
	
	CharacterController charController;

	float speed;
	float currentGravity = 0f;
	float speedMultiplier = 1f;
	float currentJumpVelocity = 0f;
	
	void Start () {
		cam = transform.GetChild(0);
		camComponent = cam.GetComponent<Camera>();
		cam.localPosition = new Vector3(0f, height / 3.2f, 0f);

		charController = GetComponent<CharacterController>();

		speed = walkSpeed;
	}

	void Update () {
		characterTargetRot = transform.localRotation;
		cameraTargetRot = cam.localRotation;

		float xRot = Input.GetAxis("Mouse Y") * sensitivityY;
		float yRot = Input.GetAxis("Mouse X") * sensitivityX;

		characterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		cameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

		transform.localRotation = characterTargetRot;
		cam.localRotation = cameraTargetRot;
	}

	void FixedUpdate() {
		Move();
	}

	void Move() {
		if(canMove) {
			MoveSneak();

			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			input.Normalize();
			
			if(input.y > 0 && Input.GetAxisRaw("Sprint") > 0) {
				if(!isSprinting) {
					StartCoroutine(ChangeFOV(70));
				}
				isSprinting = true;
			} else {
				if(isSprinting) {
					StartCoroutine(ChangeFOV(60));
				}
				isSprinting = false;
			}
		
			Vector3 movement = transform.forward * input.y + transform.right * input.x;

			speed = isSneaking ? sneakSpeed : (isSprinting ? sprintSpeed : walkSpeed);
			movement *= speed * speedMultiplier * Time.deltaTime;

			currentJumpVelocity /= 2;
			if(Input.GetAxisRaw("Jump") > 0 && charController.isGrounded) {
				currentJumpVelocity = gravity * jumpMultiplier;
			}

			if(charController.isGrounded) {
				currentGravity = 0f;
			} else {
				currentGravity += gravity - currentJumpVelocity;
				movement -= transform.up * currentGravity * Time.deltaTime;
			}

			charController.Move(movement);
		}
	}

	void MoveSneak() {
		if(Input.GetAxisRaw("Sneak") > 0) {
			if(!isSneaking) {
				StartCoroutine(MoveHead(sneakHeight));
			}
			isSneaking = true;
		} else {
			if(isSneaking) {
				StartCoroutine(MoveHead(height));
			}
			isSneaking = false;
		}
	}

	IEnumerator MoveHead(float newHeight) {
		Vector3 oldCamPos = cam.localPosition;
		Vector3 newCamPos = new Vector3(0f, newHeight / 3.2f, 0f);

		for(float i = 0; i < 1.0f; i = i + 0.2f) {
			cam.localPosition = Vector3.Lerp(oldCamPos, newCamPos , i);
			yield return new WaitForSeconds(1f / 60f);
		}
		
		cam.localPosition = newCamPos;
	}

	IEnumerator ChangeFOV(float newFOV) {
		float oldFOV = camComponent.fieldOfView;

		for(float i = 0; i < 1.0f; i = i + 0.2f) {
			camComponent.fieldOfView = Mathf.Lerp(camComponent.fieldOfView, newFOV , i);
			yield return new WaitForSeconds(1f / 60f);
		}
		
		camComponent.fieldOfView = newFOV;
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

	public void SetSpeedMultiplier(float speed) {
		speedMultiplier = speed;
	}

	public void ResetSpeedMultiplier() {
		SetSpeedMultiplier(1f);
	}

	public IEnumerator toPosition(Transform position, float speedMultiplier) {
		canMove = false;	//Deactivate Movement

		Vector3 pos = transform.position;			//Positions and Rotations before lerping
		Quaternion rot1 = transform.rotation;
		Quaternion rot2 = cam.rotation;

		for(float i = 0; i < 1; i = i + 0.01f * speedMultiplier) {		//Lerps to the start position
			transform.position = Vector3.Lerp(pos, position.position, i);
			transform.rotation = Quaternion.Lerp(rot1, position.rotation, i);
			cam.rotation = Quaternion.Lerp(rot2, Quaternion.LookRotation(position.forward, position.up), i);
			yield return new WaitForSeconds(1f / 60f);
		}
			
		transform.position = position.position;
		transform.rotation = position.rotation;
		cam.rotation = Quaternion.LookRotation(position.forward, position.up);

		canMove = true;
	}

	public void RotateToFast(Vector3 toRotation, float speedMultiplier) {
		StartCoroutine(CameraRotateToFast(toRotation.x, speedMultiplier));
		StartCoroutine(transformRotateToFast(toRotation.y, speedMultiplier));
	}

	public void RotateTo(Vector3 toRotation, float speedMultiplier) {
		StartCoroutine(RotateTo(toRotation.x, toRotation.y, speedMultiplier));
	}

	IEnumerator RotateTo(float XValue, float YValue, float speedMultiplier) {
		canMove = false;

		Vector3 oldRotation = transform.localEulerAngles;
		Vector3 newRotation = Vector3.up * YValue;
		Vector3 oldRotationCam = cam.localEulerAngles;
		Vector3 newRotationCam = Vector3.right * XValue;

		for(float i = 0; i < 1f; i += speedMultiplier) {
			transform.localRotation = Quaternion.Euler(Vector3.Lerp(oldRotation, newRotation, i));
			cam.localRotation = Quaternion.Euler(Vector3.Lerp(oldRotationCam, newRotationCam, i));
			yield return new WaitForSeconds(1f / 60f);
		}

		transform.localRotation = Quaternion.Euler(newRotation);
		cam.localRotation = Quaternion.Euler(newRotationCam);

		canMove = true;
	}

	IEnumerator CameraRotateToFast(float XValue, float speedMultiplier) {
		Vector3 oldRotation = cam.localEulerAngles;
		Vector3 newRotation = Vector3.right * XValue;
		for(float i = 100; i > 1f; i /= 2 - speedMultiplier) {
			cam.localRotation = Quaternion.Euler(Vector3.Lerp(oldRotation, newRotation, (100 - i) / 100));
			yield return new WaitForSeconds(1f / 60f);
		}
		cam.localRotation = Quaternion.Euler(newRotation);
	}

	IEnumerator transformRotateToFast(float YValue, float speedMultiplier) {
		Vector3 oldRotation = transform.localEulerAngles;
		Vector3 newRotation = Vector3.up * YValue;
		for(float i = 100; i > 1f; i /= 2 - speedMultiplier) {
			transform.localRotation = Quaternion.Euler(Vector3.Lerp(oldRotation, newRotation, (100 - i) / 100));
			yield return new WaitForSeconds(1f / 60f);
		}
		transform.localRotation = Quaternion.Euler(newRotation);
	}
}
}