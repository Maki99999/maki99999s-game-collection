using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSelect : MonoBehaviour {

	public AudioSource audioSource;
	public Button selectLevelButton;
	public Animator fadeBlack;
	public ToggleGroup toggleGroup;

	int activeToggle = -1;

	public void Cancel() {
		audioSource.Play();
		gameObject.SetActive(false);
	}

	public void SelectLevel() {
		audioSource.Play();
		string levelName = "";
		string discordName = "";
		switch(activeToggle) {
			case -1:
				selectLevelButton.interactable = false;
				return;
			case 0:
				levelName = "BallRoll3Menu";
				discordName = BallRoll3.TitleRandomizer.GetRandomName() + ": The Second Return";
				break;
			case 1:
				levelName = "FalseTruthMainMenu";
				discordName = "False Truth";
				break;
			case 2:
				levelName = "TicTacToe";
				discordName = "Tic Tac Toe";
				break;
			case 3:
				levelName = "SportsGame";
				discordName = "Sports Game";
				break;
			case 4:
				levelName = "ArtPark";
				discordName = "Art Park";
				break;
			case 5:
				levelName = "Proj3ctMenu";
				discordName = "Proj3ct";
				break;
			case 6:
				levelName = "MakiSupermarketMainMenu";
				discordName = "Maki Supermarket";
				break;
			case 7:
				levelName = "FriendsInDreamsDummy";
				discordName = "Friends In Dreams";
				break;
			default:
				return;
		}
		StartCoroutine(LoadScene(levelName));
		DiscordIntegration.UpdateActivity(discordName);
	}

	IEnumerator LoadScene(string name) {
		fadeBlack.SetTrigger("Out");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}

	public void SetActiveToggle(int id, bool setActive) {
		toggleGroup.allowSwitchOff = false;
		if(setActive) {
			activeToggle = id;
			selectLevelButton.interactable = true;
		} else {
			if(activeToggle == id) {
				selectLevelButton.interactable = false;
				activeToggle = -1;
			}
		}
	}
}
