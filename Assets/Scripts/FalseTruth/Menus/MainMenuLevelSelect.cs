using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class MainMenuLevelSelect : MonoBehaviour {

	public FalseTruth.MainMenuButtons mmButtons;

	public void selectLevel(string level) {
		string[] levelA = level.Split('.');
		if(levelA.Length == 2) {
			int scene;
			int checkp;
			int.TryParse(levelA[0], out scene);
			int.TryParse(levelA[1], out checkp);
			PlayerPrefs.SetInt("Scene", scene);
			PlayerPrefs.SetInt("Checkpoint", checkp);
			mmButtons.LoadGame();
		}
	}
}
}