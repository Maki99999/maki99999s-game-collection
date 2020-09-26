using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth {
public class C01OccultClub : FalseTruth.Cutscene {

	const string RIGHT_PASSWORD = "TheGameIsDead";

	public Transform secretStart;

	[HideInInspector]
	public bool gotInput = false;
	public GameObject inputUI;

	InputField input;

	[HideInInspector]
	public bool alreadyUsed = false;
	FalseTruth.DialogueManager dialogueManager;
	FalseTruth.FirstPersonControllerExtended fpsController;

	void Start() {
		dialogueManager = FindObjectOfType<FalseTruth.DialogueManager>();
		input = inputUI.GetComponentInChildren<InputField>();
		fpsController = GameObject.FindWithTag("Player").GetComponent<FalseTruth.FirstPersonControllerExtended>();
	}
	
	public override IEnumerator TheCutscene() {
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);
		FalseTruth.Dialogue dialogue = new FalseTruth.Dialogue();
		if(alreadyUsed) {
			dialogue = new FalseTruth.Dialogue(FalseTruth.LocalizationScript.Get("C01OccultDoorYouRepeat"));
			dialogueManager.StartDialogue(dialogue, true);
		} else {
			alreadyUsed = true;
			GetComponent<Animator>().SetTrigger("Open");	//Play Animation

			dialogue = new FalseTruth.Dialogue();
			dialogue.Add(FalseTruth.LocalizationScript.Get("C01OccultDoorYou00"));
			dialogue.Add(FalseTruth.LocalizationScript.Get("OccultGirl0"), FalseTruth.LocalizationScript.Get("C01OccultDoorGirl00"));

			FalseTruth.Dialogue dialogueInput = new FalseTruth.Dialogue("@Out1");

			FalseTruth.Dialogue dialogueWhatPassword = new FalseTruth.Dialogue(FalseTruth.LocalizationScript.Get("C01OccultDoorYou03") + "@Out0");
			for(int i = 3; i < 6; i++) {
				dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("OccultGirl0"), FalseTruth.LocalizationScript.Get("C01OccultDoorGirl0" + (i - 1)));
				dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("C01OccultDoorYou0" + (i + 1)));
			}
			dialogueWhatPassword.Add("", "...");
			dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("C01OccultDoorYou08"));
			dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("OccultGirl0"), FalseTruth.LocalizationScript.Get("C01OccultDoorGirl06"));
			dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("C01OccultDoorYou09"));
			dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("OccultGirl0"), FalseTruth.LocalizationScript.Get("C01OccultDoorGirl07"));
			dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("C01OccultDoorYou10"));
			dialogueWhatPassword.Add(FalseTruth.LocalizationScript.Get("C01OccultDoorYou11"));
			dialogueWhatPassword.lastText.italic = true;
			
			dialogue.AddChoices(new List<FalseTruth.Dialogue> {dialogueInput, dialogueWhatPassword}, 
					new List<string> {FalseTruth.LocalizationScript.Get("C01OccultDoorYou01") + "...", FalseTruth.LocalizationScript.Get("C01OccultDoorYou03")});
			dialogueManager.StartDialogue(dialogue, true);

			bool firstLoop = true;
			while(dialogueManager.dialogueOutput != 0) {
				while(dialogueManager.dialogueOutput == -1) {
					yield return null;
				}

				if(dialogueManager.dialogueOutput == 0) break;

				inputUI.SetActive(true);
				input.ActivateInputField();

				while(!gotInput) {
					yield return null;
				}
				
				inputUI.SetActive(false);
				gotInput = false;

				if(input.text == RIGHT_PASSWORD) {
					dialogueManager.dialogueOutput = 0;
					StartCoroutine(SuperSecretSecret1());
				} else {
					if(firstLoop) {
						firstLoop = false;
						FalseTruth.Dialogue dialogueWrong = new FalseTruth.Dialogue(FalseTruth.LocalizationScript.Get("C01OccultDoorYou01") + " " + input.text + "?");
						dialogueWrong.Add(FalseTruth.LocalizationScript.Get("OccultGirl0"), FalseTruth.LocalizationScript.Get("C01OccultDoorGirl01"));
						dialogueWrong.AddChoices(new List<FalseTruth.Dialogue> {dialogueInput, dialogueWhatPassword}, 
								new List<string> {FalseTruth.LocalizationScript.Get("C01OccultDoorYou02") + "...", FalseTruth.LocalizationScript.Get("C01OccultDoorYou03")});

						dialogueManager.StartDialogue(dialogueWrong, true);
					} else {
						FalseTruth.Dialogue dialogueWrong = new FalseTruth.Dialogue(FalseTruth.LocalizationScript.Get("C01OccultDoorYou02") + " " + input.text + "?");
						dialogueWrong.Add(FalseTruth.LocalizationScript.Get("OccultGirl0"), FalseTruth.LocalizationScript.Get("C01OccultDoorGirl01"));
						dialogueWrong.AddChoices(new List<FalseTruth.Dialogue> {dialogueInput, dialogueWhatPassword}, 
								new List<string> {FalseTruth.LocalizationScript.Get("C01OccultDoorYou02") + "...", FalseTruth.LocalizationScript.Get("C01OccultDoorYou03")});
						
						dialogueManager.StartDialogue(dialogueWrong, true);
					}
				}
			}
		}
		
		while(!dialogueManager.isFinished())
					yield return null;
		fpsController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);
		FalseTruth.Checkpoints.SetCheckpoint(-1f);
	}

	IEnumerator SuperSecretSecret1() {
		dialogueManager.EndDialogue();

		fpsController.toPosition(secretStart, 1f);
		while(fpsController.isInScriptedMovement)
			yield return null;
		Animator playerAnim = fpsController.gameObject.GetComponent<Animator>();
		playerAnim.enabled = true;
		playerAnim.Play("C01Secret", 0);
		GetComponent<Animator>().SetTrigger("Secret");

		yield return new WaitForSeconds(1f);
		StartCoroutine(dialogueManager.StartDialogue(FalseTruth.LocalizationScript.Get("You"), FalseTruth.LocalizationScript.Get("C01OccultDoorYouSecret"), 2f));
		yield return new WaitForSeconds(2.2f);

		StartCoroutine(dialogueManager.StartDialogue("", FalseTruth.LocalizationScript.Get("C01OccultDoorSecret"), 2f));
		yield return new WaitForSeconds(2f);
		
		PlayerPrefs.SetInt("Checkpoint", -1);
		PlayerPrefs.SetInt("Scene", -1);
		FindObjectOfType<FalseTruth.GameController>().EndScene("FalseTruthMainMenu");
	}

	public void setGotInput() {
		gotInput = true;
	}
}
}