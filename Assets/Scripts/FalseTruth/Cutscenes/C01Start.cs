using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class C01Start : MonoBehaviour {

	public Animator playerAnimator;
	public Animator girlAnimator;
	public Animator girlChairAnimator;
	public Animator paperAnimator;

	public AnimationClip girlAngry;
	public AnimationClip girlHappy;

	public GameObject ghost;

	public Transform deskNChair;

	public FalseTruth.FirstPersonControllerExtended fpsController;

	public AudioClip gong;

	AudioSource audioSource;
	FalseTruth.DialogueManager dialogueManager;

	void Start () {
		StartCoroutine(Cutscene());
		audioSource = GetComponent<AudioSource>();
	}

	IEnumerator Cutscene() {
		yield return null;	//Waits 1 Frame, so everything is initialized

		FalseTruth.Dialogue dialogue0 = new FalseTruth.Dialogue();
		dialogue0.Add(FalseTruth.LocalizationScript.Get("C01Start00"), true);
		dialogue0.Add("@Out0" + FalseTruth.LocalizationScript.Get("C01Start01"), true);
		dialogue0.Add(FalseTruth.LocalizationScript.Get("C01Start02"), true);
		dialogue0.Add(FalseTruth.LocalizationScript.Get("C01Start03"), true);
		dialogue0.Add("@Out1" + FalseTruth.LocalizationScript.Get("C01Start04"), true);

		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);

		playerAnimator.SetTrigger("Start");

		yield return new WaitForSeconds(2f);
		dialogueManager = FindObjectOfType<FalseTruth.DialogueManager>();
		dialogueManager.StartDialogue(dialogue0, true);

		while(dialogueManager.dialogueOutput != 0)
			yield return null;
		playerAnimator.SetTrigger("Paper");
		paperAnimator.SetBool("Hold", true);
		yield return new WaitForSeconds(1f);
		paperAnimator.gameObject.transform.LookAt(playerAnimator.gameObject.transform);

		while(dialogueManager.dialogueOutput != 1)
			yield return null;
		playerAnimator.SetTrigger("Paper");
		paperAnimator.SetBool("Hold", false);

		float timer = Time.time;
		while(!dialogueManager.isFinished() || Time.time - timer < 0.75f) {
			yield return null;
		}
		playerAnimator.enabled = false;
		fpsController.ClampY(160, 20);
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);

		Transform player = playerAnimator.gameObject.transform;

		timer = Time.time;
		float playerRotY;
		while(Time.time - timer < 10f) {
			playerRotY = player.eulerAngles.y;
			if(playerRotY > 5 && playerRotY < 90) {
				fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);
				fpsController.ClampY(false);

				fpsController.RotateToFast(new Vector3(0f, 45f, 0f), .5f);
				StartCoroutine(dialogueManager.StartDialogue(FalseTruth.LocalizationScript.Get("You"), FalseTruth.LocalizationScript.Get("C01Start09"), 1.5f));
				yield return new WaitForSeconds(0.2f);
				ghost.SetActive(false);
				yield return new WaitForSeconds(1.0f);

				fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
				break;
			}
			yield return null;
		}
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);
		if(player.localEulerAngles.y > 90) {
			fpsController.RotateTo(new Vector3(24.681f, 270f, 0f), .02f);
		} else {
			fpsController.RotateTo(new Vector3(24.681f, -90f, 0f), .02f);
		}
		fpsController.ClampY(false);

		ghost.SetActive(false);

		while(fpsController.isInScriptedMovement) {
			yield return null;
		}
		playerAnimator.enabled = true;
		playerAnimator.Play("C01StartPlayerIdle00", 0);

		if(gong != null) audioSource.PlayOneShot(gong, .8f);
		yield return new WaitForSeconds(2f);
		//TODO: GroupOfPeopleAnimator.SetTrigger("GoAway");
		//TODO: Start babble (murmeln) sound effect

		Transform girl = girlAnimator.gameObject.transform;

		girlAnimator.SetTrigger("StandUp");
		girlChairAnimator.SetTrigger("Move");

		Vector3 oldPosition = girl.position;
		for(float i = 3.02f; i <= 3.17; i += .005f) {
			girl.position = new Vector3(oldPosition.x, i, oldPosition.z);
			yield return new WaitForSeconds(1f / 60f);
		}

		girlAnimator.SetBool("Walk", true);
		while(!girlAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
			yield return null;
		StartCoroutine(FalseTruth.GameController.RotateTo(girl, Quaternion.Euler(new Vector3(0f, 0f, 0f)), 10));
		yield return FalseTruth.GameController.GoTo(girl, new Vector3(-7.67f, 3.17f, -4.55f), 40);
		playerAnimator.SetTrigger("Start");
		StartCoroutine(FalseTruth.GameController.RotateTo(girl, Quaternion.Euler(new Vector3(0f, 35f, 0f)), 10));
		yield return FalseTruth.GameController.GoTo(girl, new Vector3(-7.333f, 3.17f, -4.07f), 40);
		girlAnimator.SetBool("Walk", false);
		StartCoroutine(FalseTruth.GameController.RotateTo(girl, Quaternion.Euler(new Vector3(0f, 80f, 0f)), 10));

		FalseTruth.Dialogue dialogue1 = new FalseTruth.Dialogue();
		dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), "@Out2" + FalseTruth.LocalizationScript.Get("C01StartGirl00"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01Start10"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01StartGirl01"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01Start11"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01StartGirl02") + "@Out3");
		dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01StartGirl03"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01Start12"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01StartGirl04"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01Start13") + "@Out4");
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01Start14"), true);
		
		dialogueManager.StartDialogue(dialogue1, true);

		while(dialogueManager.dialogueOutput != 2)
			yield return null;
		girlAnimator.SetTrigger("faceAngry");

		while(dialogueManager.dialogueOutput != 3)
			yield return null;
		girlAnimator.SetTrigger("faceHappy");
		
		while(dialogueManager.dialogueOutput != 4)
			yield return null;
		girlAnimator.SetTrigger("faceDefault");
		playerAnimator.SetTrigger("Start");
		girlAnimator.SetBool("Walk", true);

		StartCoroutine(FalseTruth.GameController.RotateTo(girl, Quaternion.Euler(new Vector3(0f, 160f, 0f)), 10));
		yield return FalseTruth.GameController.GoTo(girl, new Vector3(-7.2f, 3.17f, -4.55f), 20);

		StartCoroutine(FalseTruth.GameController.RotateTo(girl, Quaternion.Euler(new Vector3(0f, 90f, 0f)), 10));
		yield return FalseTruth.GameController.GoTo(girl, new Vector3(-5.2f, 3.17f, -4.55f), 50);

		while(!dialogueManager.isFinished())
			yield return null;
		girl.gameObject.SetActive(false);

		while(!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("C01StartPlayerIdle00"))
			yield return null;
		
		playerAnimator.enabled = false;
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
		deskNChair.GetComponent<FalseTruth.Useable>().Use();
		FalseTruth.Checkpoints.SetCheckpoint(0f);
	}
}
}