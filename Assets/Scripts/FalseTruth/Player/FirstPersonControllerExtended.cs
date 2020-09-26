using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class FirstPersonControllerExtended : MonoBehaviour {

	public const int NO_MOVEMENT = 0;
	public const int HEAD_MOVEMENT = 1;
	public const int FULL_MOVEMENT = 2;
	const int HeadAfterNoMovement = 3;

	[HideInInspector]
	public bool isInScriptedMovement = false;

	Transform player;			//Player and camera
	Transform playerCamera;

	FalseTruth.FirstPersonController fpsController;		//PlayerController Scripts
	CharacterController charController;
	FalseTruth.UseController useController;

	Stack<int> movements = new Stack<int>();

	void Start () {
		player = transform;

		fpsController = GetComponent<FalseTruth.FirstPersonController>();
		charController = GetComponent<CharacterController>();
		useController = GetComponent<FalseTruth.UseController>();

		playerCamera = transform.GetChild(0);
	}
	
	public void SetMovement(int state) {
		switch(state) {
			case NO_MOVEMENT:
				movements.Push(NO_MOVEMENT);
				fpsController.enabled = false;
				charController.enabled = false;
				useController.Hide();
				useController.enabled = false;
				break;

			case HEAD_MOVEMENT:
				if(movements.Count > 0 && movements.Peek() == NO_MOVEMENT) {
					movements.Push(NO_MOVEMENT);
				} else {
					movements.Push(HEAD_MOVEMENT);
				}

				fpsController.enabled = true;
				charController.enabled = false;
				useController.enabled = true;

				fpsController.canMove = false;
				break;

			case FULL_MOVEMENT:
				if(movements.Count > 0) movements.Pop();

				if(movements.Count == 0) {
					fpsController.enabled = true;
					charController.enabled = true;
					useController.enabled = true;
					
					fpsController.canMove = true;
				} else {
					SetMovement(movements.Pop());
				}
				break;
		}
		//Debug.Log(string.Join(" / ", new List<int>(movements).ConvertAll(i => i.ToString()).ToArray()));
	}

	public IEnumerator toPosition(Transform position, float speedMultiplier) {
		isInScriptedMovement = true;
		SetMovement(NO_MOVEMENT);	//Deactivate Movement

		Vector3 pos = player.position;			//Positions and Rotations before lerping
		Quaternion rot1 = player.rotation;
		Quaternion rot2 = playerCamera.rotation;

		for(float i = 0; i < 1; i = i + 0.01f * speedMultiplier) {		//Lerps to the start position
			player.position = Vector3.Lerp(pos, position.position, i);
			player.rotation = Quaternion.Lerp(rot1, position.rotation, i);
			playerCamera.rotation = Quaternion.Lerp(rot2, Quaternion.LookRotation(position.forward, position.up), i);
			yield return new WaitForSeconds(1f / 60f);
		}
			
		player.position = position.position;
		player.rotation = position.rotation;
		playerCamera.rotation = Quaternion.LookRotation(position.forward, position.up);

		isInScriptedMovement = false;
		SetMovement(FULL_MOVEMENT);
	}

	public void RotateBetween(float left, float right, bool toLeft) {
		float playerRotationY = player.rotation.eulerAngles.y;

		if(left < 0f) {
			right -= left;
			playerRotationY -= left;
			left = 0;
		}

		if(right > 360) {
			left -= right - 360;
			playerRotationY -= right - 360;
			right = 360;
		}

		if(playerRotationY > left && playerRotationY < right) {

			float multiplier = -(playerRotationY - left) / (right - left);

			if(!toLeft) {
				multiplier *= -1;
				multiplier = 1 - multiplier;
			}
			player.rotation *= Quaternion.Euler(player.up * multiplier);
		}
	}

	public void RotateToFast(Vector3 toRotation, float speedMultiplier) {
		StartCoroutine(CameraRotateToFast(toRotation.x, speedMultiplier));
		StartCoroutine(PlayerRotateToFast(toRotation.y, speedMultiplier));
	}

	public void RotateTo(Vector3 toRotation, float speedMultiplier) {
		StartCoroutine(RotateTo(toRotation.x, toRotation.y, speedMultiplier));
	}

	IEnumerator RotateTo(float XValue, float YValue, float speedMultiplier) {
		isInScriptedMovement = true;

		Vector3 oldRotation = player.localEulerAngles;
		Vector3 newRotation = Vector3.up * YValue;
		Vector3 oldRotationCam = playerCamera.localEulerAngles;
		Vector3 newRotationCam = Vector3.right * XValue;

		for(float i = 0; i < 1f; i += speedMultiplier) {
			player.localRotation = Quaternion.Euler(Vector3.Lerp(oldRotation, newRotation, i));
			playerCamera.localRotation = Quaternion.Euler(Vector3.Lerp(oldRotationCam, newRotationCam, i));
			yield return new WaitForSeconds(1f / 60f);
		}

		player.localRotation = Quaternion.Euler(newRotation);
		playerCamera.localRotation = Quaternion.Euler(newRotationCam);

		isInScriptedMovement = false;
	}

	IEnumerator CameraRotateToFast(float XValue, float speedMultiplier) {
		Quaternion oldRotation = playerCamera.localRotation;
		Quaternion newRotation = Quaternion.Euler(Vector3.right * XValue);
		for(float i = 100; i > 1f; i /= 2 - speedMultiplier) {
			playerCamera.localRotation = Quaternion.Lerp(oldRotation, newRotation, (100 - i) / 100);
			yield return new WaitForSeconds(1f / 60f);
		}
		playerCamera.localRotation = newRotation;
	}

	IEnumerator PlayerRotateToFast(float YValue, float speedMultiplier) {
		Quaternion oldRotation = player.localRotation;
		Quaternion newRotation = Quaternion.Euler(Vector3.up * YValue);
		for(float i = 100; i > 1f; i /= 2 - speedMultiplier) {
			player.localRotation = Quaternion.Lerp(oldRotation, newRotation, (100 - i) / 100);
			yield return new WaitForSeconds(1f / 60f);
		}
		player.localRotation = newRotation;
	}

	public void ResetSpeedMultiplier() {
		fpsController.SetSpeedMultiplier(1f);
	}

	public void SetSpeedMultiplier(float multiplier) {
		fpsController.SetSpeedMultiplier(multiplier);
	}

	public void ClampY(bool clamp) {
		fpsController.clampY = clamp;
	}

	public void ClampY(float min, float max) {
		fpsController.clampY = true;
		fpsController.clampYMin = min;
		fpsController.clampYMax = max;
	}

	public Transform GetCamera() {
		return playerCamera;
	}
}
}