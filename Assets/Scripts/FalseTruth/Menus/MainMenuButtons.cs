using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FalseTruth {
public class MainMenuButtons : MonoBehaviour {

	public FalseTruth.OptionsMenuController optionsMenu;	//The options menu

	public GameObject loadButton;

	Animator canvasAnimator;		//The GUI animator

	void Start() {
		canvasAnimator = transform.root.GetComponent<Animator>();	//Get the components

		if(PlayerPrefs.GetInt("Scene", 0) > 0) {
			loadButton.SetActive(true);
		}
	}

	public void NewGame() {
		StartCoroutine(NewGameIEnumerator());
	}

	IEnumerator NewGameIEnumerator() {
		canvasAnimator.SetTrigger("NewGame");	//Start fade animation
		yield return new WaitForSeconds(3);		//Wait for the animation

		PlayerPrefs.SetInt("Scene", -1);
		PlayerPrefs.SetInt("OccultLevel", 0);
		PlayerPrefs.SetInt("Checkpoint", -1);
		SceneManager.LoadScene("00 - Beginning");	//Load the first scene
	}

	public void LoadGame() {
		int savedScene = PlayerPrefs.GetInt("Scene", -1);
		string sceneName = "FalseTruthMainMenu";

		switch(savedScene) {
			case -1: 
				goto case 0;
			case 0: 
				NewGame();
				break;
			case 1:
				sceneName = "01 - Nothing Happened";
				break;
		}
		StartCoroutine(LoadGameIEnumerator(sceneName));
	}

	IEnumerator LoadGameIEnumerator(string sceneName) {
		canvasAnimator.SetTrigger("NewGame");	//Start fade animation
		yield return new WaitForSeconds(3f);		//Wait for the animation

		SceneManager.LoadScene(sceneName);	//Load the scene
	}

	public void Options() {
		optionsMenu.Open();
	}

	public void Exit() {
		StartCoroutine(LoadGameIEnumerator("MainMenu"));
	}
}
}