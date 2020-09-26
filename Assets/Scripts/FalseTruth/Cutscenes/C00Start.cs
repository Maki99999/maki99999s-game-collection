using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class C00Start : MonoBehaviour {

	void Start () {
		StartCoroutine(Cutscene());
	}
	
	IEnumerator Cutscene() {
		yield return null;
		CharacterController charController = FindObjectOfType<CharacterController>();
		FalseTruth.FirstPersonControllerExtended fpsController =  FindObjectOfType<FalseTruth.FirstPersonControllerExtended>();
		FalseTruth.DialogueManager dialogueManager = FindObjectOfType<FalseTruth.DialogueManager>();

		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);
		charController.enabled = true;

		yield return new WaitForSeconds(0.5f);

		float timer = Time.time;
		while(Time.time - timer < 1.6f) {
			charController.Move(charController.gameObject.transform.forward * Time.deltaTime * 2.5f);
			yield return null;
		}
		charController.enabled = false;

		FalseTruth.Dialogue dialogue = new FalseTruth.Dialogue();
		dialogue.Add(FalseTruth.LocalizationScript.Get("C00Start00"), true);
		dialogue.Add(FalseTruth.LocalizationScript.Get("C00Start01"), true);

		dialogueManager.StartDialogue(dialogue);

		while(!dialogueManager.isFinished()) {
			yield return null;
		}
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
	}
}
}