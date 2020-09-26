using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth {
public class DoorOpenerFromSubAndStartCutscene : FalseTruth.Useable {

	public Animator girlAnimator;		//Animator for grill

	Animator anim;		//Open animation
	Animator cutscene;	//Cutscene animation

	Transform player;			//Player and camera
	Transform playerCamera;

	FalseTruth.FirstPersonControllerExtended playerController;	//Player Controller Script

	FalseTruth.DialogueManager dialogueManager;

	Vector3 pos;
	Quaternion rot1;
	Quaternion rot2;
	
	public Transform cutsceneStartPosition;		//Start position of the cutscene

	void Start () {
		anim = transform.parent.GetComponent<Animator>();				//Get Components
		cutscene = transform.parent.parent.GetComponent<Animator>();

		StartCoroutine(LateStart());
	}

	IEnumerator LateStart() {
		yield return null;

		player = GameObject.FindWithTag("Player").transform;
		playerController = player.GetComponent<FalseTruth.FirstPersonControllerExtended>();
		
		playerCamera = playerController.GetCamera();
		
		dialogueManager = FindObjectOfType<FalseTruth.DialogueManager>();

	}
	
	public override void Use() {
		anim.SetBool("Open", true);		//Opens the door
		StartCoroutine(StartCutscene());
	}

	IEnumerator StartCutscene() {
		playerController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);	//Deactivates Movement

		pos = player.position;		//Positions and Rotations before lerping
		rot1 = player.rotation;
		rot2 = playerCamera.rotation;

		for(float i = 0; i < 1; i = i + 0.01f) {		//Lerps to the start position
			player.position = Vector3.Lerp(pos, cutsceneStartPosition.position, i);
			player.rotation = Quaternion.Lerp(rot1, cutsceneStartPosition.rotation, i);
			playerCamera.rotation = Quaternion.Lerp(rot2, Quaternion.LookRotation(cutsceneStartPosition.forward, cutsceneStartPosition.up), i);
			yield return new WaitForSeconds(1f / 60f);
		}
		cutscene.enabled = true;
		cutscene.SetTrigger("Start");	//Starts the actual cutscene

		yield return new WaitForSeconds(1f);
		StartCoroutine(dialogueManager.StartDialogue(FalseTruth.LocalizationScript.Get("You"), FalseTruth.LocalizationScript.Get("C00Start02"), 3));
		
		yield return new WaitForSeconds(4f);
		StartCoroutine(dialogueManager.StartDialogue(FalseTruth.LocalizationScript.Get("You"), FalseTruth.LocalizationScript.Get("Girl") + "?", 1));
		girlAnimator.SetTrigger("Falling");
		
		yield return new WaitForSeconds(1f);
		StartCoroutine(dialogueManager.StartDialogue(FalseTruth.LocalizationScript.Get("You"), FalseTruth.LocalizationScript.Get("Girl") + "!", 1));
	}
}
}