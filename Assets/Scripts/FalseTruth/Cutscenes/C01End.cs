using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class C01End : MonoBehaviour {

	bool alreadyUsed = false;

	public Transform player;
	public Transform playerLookingAt;
	public Transform girl;
	public Transform girlLookingAt;
	public Transform girlMoveTo;

	public Animator doorAnimator;
	public Animator fadeWhiteAnimator;

	public Transform playerEndPosition;
	public Transform girlEndPosition;

	Animator girlAnimator;
	Animator playerAnimator;
	FalseTruth.FirstPersonControllerExtended fpsController;
	FalseTruth.DialogueManager dialogueManager;

	public void Start () {
		StartCoroutine(LateStart());

		playerAnimator = player.GetComponent<Animator>();
		girlAnimator = girl.GetComponent<Animator>();
		fpsController = player.GetComponent<FalseTruth.FirstPersonControllerExtended>();
		dialogueManager = GameObject.FindObjectOfType<FalseTruth.DialogueManager>();
	}

	IEnumerator LateStart() {
		yield return new WaitForEndOfFrame();

		yield return null;
		girlAnimator.SetBool("Walk", true);
		yield return null;
		girlAnimator.SetBool("Walk", false);
	}

	void OnTriggerEnter(Collider other) {
		if(alreadyUsed) return;

		if(other.CompareTag("Player")) {
			alreadyUsed = true;
			
			fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);

			playerLookingAt.position = player.GetChild(0).position;
			playerLookingAt.LookAt(girlLookingAt);
			girlLookingAt.LookAt(playerLookingAt);
			girlLookingAt.rotation = Quaternion.Euler(new Vector3(0f, girlLookingAt.eulerAngles.y, 0f));

			StartCoroutine(FalseTruth.GameController.RotateTo(girl, girlLookingAt.rotation, 40));
			fpsController.RotateTo(playerLookingAt.eulerAngles, 1f / 40f);

			if(PlayerPrefs.GetFloat("OccultLevel") < 0) {
				StartCoroutine(CutsceneOccult());
			} else {
				StartCoroutine(CutsceneNormal());
			}
		}
	}
	
	IEnumerator CutsceneNormal() {
		FalseTruth.Dialogue dialogue0 = new FalseTruth.Dialogue();
		dialogue0.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndNormalGirl00"));
		dialogueManager.StartDialogue(dialogue0);

		while(!dialogueManager.isFinished())
			yield return null;
		
		doorAnimator.SetBool("Open", true);
		fadeWhiteAnimator.SetTrigger("White");
		while(!fadeWhiteAnimator.GetCurrentAnimatorStateInfo(0).IsName("isWhite"))
			yield return null;
		
		player.position = playerEndPosition.position;
		player.rotation = playerEndPosition.rotation;
		girl.position = girlEndPosition.position;
		girl.rotation = girlEndPosition.rotation;

		girlAnimator.SetBool("Walk", true);
		playerAnimator.enabled = true;
		playerAnimator.SetTrigger("Walk");
		
		FalseTruth.Dialogue dialogue1 = new FalseTruth.Dialogue();
		for(int i = 0; i <= 2; i++) {
			dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndNormalGirl0" + (i + 1)));
			dialogue1.Add(FalseTruth.LocalizationScript.Get("C01EndNormal0" + i));
		}
		for(int i = 4; i <= 7; i++) {
			dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndNormalGirl0" + i));
		}
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01EndNormal03"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndNormalGirl08"));
		dialogue1.Add(FalseTruth.LocalizationScript.Get("C01EndNormal04"), true);
		yield return null;

		fadeWhiteAnimator.SetTrigger("White");

		yield return new WaitForSeconds(1f);
		dialogueManager.StartDialogue(dialogue1);

		while(!dialogueManager.isFinished())
			yield return null;

		GameObject.FindObjectOfType<FalseTruth.GameController>().EndScene("99 - Credits");	//TODO: scene
	}

	IEnumerator CutsceneOccult() {
		FalseTruth.Dialogue dialogue0 = new FalseTruth.Dialogue();
		dialogue0.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndOccultGirl00"));
		dialogueManager.StartDialogue(dialogue0);

		while(!dialogueManager.isFinished())
			yield return null;
		
		doorAnimator.SetBool("Open", true);
		fadeWhiteAnimator.SetTrigger("White");
		while(!fadeWhiteAnimator.GetCurrentAnimatorStateInfo(0).IsName("isWhite"))
			yield return null;
		
		player.position = playerEndPosition.position;
		player.rotation = playerEndPosition.rotation;
		girl.position = girlEndPosition.position;
		girl.rotation = girlEndPosition.rotation;

		girlAnimator.SetBool("Walk", true);
		playerAnimator.enabled = true;
		playerAnimator.SetTrigger("Walk");
		
		FalseTruth.Dialogue dialogue1a = new FalseTruth.Dialogue();
		dialogue1a.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndOccultGirl01"));
		dialogue1a.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndOccultGirl02"));
		dialogue1a.Add(FalseTruth.LocalizationScript.Get("C01EndOccult00"));

		FalseTruth.Dialogue dialogue1b = new FalseTruth.Dialogue();
		for(int i = 1; i <= 3; i++) {
			dialogue1b.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndOccultGirl0" + (i + 2)));
			dialogue1b.Add(FalseTruth.LocalizationScript.Get("C01EndOccult0" + i));
		}
		dialogue1b.Add(FalseTruth.LocalizationScript.Get("C01EndOccult04"));
		dialogue1b.Add(FalseTruth.LocalizationScript.Get("Girl"), FalseTruth.LocalizationScript.Get("C01EndOccultGirl06") + "@Out7");
		dialogue1b.Add(FalseTruth.LocalizationScript.Get("C01EndOccult05"), true);

		yield return null;

		fadeWhiteAnimator.SetTrigger("White");
		yield return new WaitForSeconds(1f);

		dialogueManager.StartDialogue(dialogue1a);

		while(!dialogueManager.isFinished())
			yield return null;

		dialogueManager.StartDialogue(dialogue1b);
		
		while(dialogueManager.dialogueOutput != 7)
			yield return null;

		girlAnimator.SetTrigger("Run");
		StartCoroutine(FalseTruth.GameController.GoTo(girl, girlMoveTo.position, 100));
		yield return new WaitForSeconds(1.6f);
		girlAnimator.SetTrigger("Run");
		
		while(!dialogueManager.isFinished())
			yield return null;

		GameObject.FindObjectOfType<FalseTruth.GameController>().EndScene("99 - Credits");	//TODO: scene
	}
}
}