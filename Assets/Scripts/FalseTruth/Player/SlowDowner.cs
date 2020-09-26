using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class SlowDowner : MonoBehaviour {

	public int slowDirection;		//0 - north, 1 - east, 2 - south, 3 - west

	Vector3 lastPosition;			//The position last Frame
	Vector3 velocity;				//Current velocity

	FalseTruth.FirstPersonControllerExtended fpsController;	//FPS controller script
	float defaultWalkSpeed;				//Default speed
	float slowSpeedMultiplier;				//Speed when slowed

	void Start() {
		lastPosition = Vector3.forward;			//Get Components
		fpsController = GetComponent<FalseTruth.FirstPersonControllerExtended>();
		slowSpeedMultiplier = 1.0f;
	}

	void Update() {
		velocity = transform.position - lastPosition;	//Calculate velocity
		lastPosition = transform.position;				//Updates last position
	}

	public void SetSlowSpeed(float speed) {
		slowSpeedMultiplier = speed;	//Calculates new speed when slowed

		switch(slowDirection) {			//Resets the speed when walking back
			case 0:
				fpsController.RotateBetween(250, 360, true);
				fpsController.RotateBetween(0, 110, false);
				if(velocity.z < 0.0f) {
					fpsController.ResetSpeedMultiplier();
					return;
				}
				break;
			case 1:
				fpsController.RotateBetween(-20, 90, true);
				fpsController.RotateBetween(90, 200, false);
				if(velocity.x < 0.0f) {
					fpsController.ResetSpeedMultiplier();
					return;
				}
				break;
			case 2:
				fpsController.RotateBetween(70, 180, true);
				fpsController.RotateBetween(180, 290, false);
				if(velocity.z > 0.0f) {
					fpsController.ResetSpeedMultiplier();
					return;
				}
				break;
			case 3:
				fpsController.RotateBetween(160, 270, true);
				fpsController.RotateBetween(270, 380, false);
				if(velocity.x > 0.0f) {
					fpsController.ResetSpeedMultiplier();
					return;
				}
				break;
		}

		fpsController.SetSpeedMultiplier(slowSpeedMultiplier);	//Updates speed
	}
}
}