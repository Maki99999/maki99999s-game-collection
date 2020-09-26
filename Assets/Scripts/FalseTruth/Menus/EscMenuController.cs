using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class EscMenuController : MonoBehaviour {

	public FalseTruth.OptionsMenuController optionsMenu;	//The options menu

	Animator anim;								//Animator for the animation
	FalseTruth.FirstPersonControllerExtended playerController;	// FPC-Script
	
	void Start () {
		anim = GetComponent<Animator>();
		GameObject player = GameObject.FindWithTag("Player");				//Get all the Components
		playerController = player.GetComponent<FalseTruth.FirstPersonControllerExtended>();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {	//Toggles the menu by pressing Escape
			ToggleMenu();
		}
	}

	public void ToggleMenu() {		//Toogles the menu
		if(anim.GetBool("Open")) {					//When the menu is open
			anim.SetBool("Open", false);				//Close the menu

			optionsMenu.Close();						//Close the options menu

			playerController.SetMovement(FalseTruth.FirstPersonControllerExtended.FULL_MOVEMENT);			//Activate both player scripts
			FalseTruth.GameController.paused = false;				//Mark the game as unpaused

			FalseTruth.GameController.LockMouse();					//Locks the mouse
		} else {									//When the menu is closed
			anim.SetBool("Open", true);					//Open the menu

			playerController.SetMovement(FalseTruth.FirstPersonControllerExtended.NO_MOVEMENT);			//Deactivate both player scripts
			FalseTruth.GameController.paused = true;				//Mark the game as paused

			FalseTruth.GameController.UnlockMouse();				//Unlocks the mouse
		}
	}

	void Save() {		//Saves the game
		//GameController.Save();
	}

	public void Quit() {		//Quits the game
		Save();
		GameObject.FindObjectOfType<FalseTruth.GameController>().EndScene("MainMenu");
	}

	public void ToMenu() {		//Returns to the main menu
		Save();
		GameObject.FindObjectOfType<FalseTruth.GameController>().EndScene("FalseTruthMainMenu");
	}

	public void OpenOptions() {		//Opens the options menu
		optionsMenu.Open();
	}
}
}