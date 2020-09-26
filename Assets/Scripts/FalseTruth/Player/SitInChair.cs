using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class SitInChair : FalseTruth.Useable {

	public Transform sitPosition;		//Sit position
	public Transform standPosition;		//Sit position

	public bool isSittingAtStart = false;

	FalseTruth.FirstPersonControllerExtended playerController;		//Player Controller Script

	[HideInInspector]
	public bool isSitting = false;

	void Start () {
		playerController = GameObject.FindWithTag("Player").GetComponent<FalseTruth.FirstPersonControllerExtended>();	//Get Components

		if(isSittingAtStart) {
			isSitting = true;
			playerController.SetMovement(FalseTruth.FirstPersonControllerExtended.HEAD_MOVEMENT);
		}
	}
	
	public override void Use() {
		StartCoroutine(ToggleSitting());
	}

	IEnumerator ToggleSitting() {
		BoxCollider[] collider = GetComponents<BoxCollider>();

		if(!isSitting) {
			collider[0].enabled = false;
			collider[1].enabled = false;
			collider[2].enabled = true;

			StartCoroutine(playerController.toPosition(sitPosition, 2));
			while(playerController.isInScriptedMovement) yield return null;
			playerController.SetMovement(FalseTruth.FirstPersonControllerExtended.HEAD_MOVEMENT);
		} else {
			collider[0].enabled = true;
			collider[1].enabled = true;
			collider[2].enabled = false;

			StartCoroutine(playerController.toPosition(standPosition, 3));
			while(playerController.isInScriptedMovement)  yield return null;
			playerController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
		}
		isSitting = !isSitting;
	}

	
}
}