using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BallRoll3 {
public class BallRoll3Menu : MonoBehaviour {

	const int levelCount = 30;

	[HideInInspector] public int levelCompleted;
	public Button continueStartButton;
	public Animator fadeBlack;

	public Animator LevelSelector;
	public GridLayoutGroup gridLayout;
	public Transform levelButtonParent;
	public GameObject levelButtonPrefab;

	Text continueStartButtonText;

	void Start () {
		continueStartButtonText = continueStartButton.GetComponentInChildren<Text>();
		levelCompleted = PlayerPrefs.GetInt("BallRollProgress", 0);
		if(levelCompleted < 1) {
			PlayerPrefs.SetInt("BallRollProgress", 0);
			continueStartButtonText.text = "Start";
		}

		MakeLevelButtons();
	}
	
	public void ContinueStart() {
		StartCoroutine(LoadScene("BRLevel" + (levelCompleted + 1)));
	}

	public void BackToMainMenu() {
		StartCoroutine(LoadScene("MainMenu"));
	}

	public void OpenLevelSelector() {
		LevelSelector.SetBool("Open", true);
	}

	public void CloseLevelSelector() {
		LevelSelector.SetBool("Open", false);
	}

	public void StartLevel(int level) {
		StartCoroutine(LoadScene("BRLevel" + level));
	}

	IEnumerator LoadScene(string name) {
		fadeBlack.SetTrigger("Out");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}

	void MakeLevelButtons() {
		Rect rect = levelButtonParent.GetComponent<RectTransform>().rect;
		float availableWidth = rect.width - 10 * 10;

		int rows = Mathf.CeilToInt(levelCount / 10);
		float availableHeight = rect.height - rows * 10;

		gridLayout.cellSize = new Vector2(availableWidth / 10, Mathf.Clamp(availableHeight / rows, 10, 50));

		for(int i = 1; i <= levelCount; i++) {
			GameObject newButton = Instantiate(levelButtonPrefab, Vector3.zero, Quaternion.identity, levelButtonParent);
			newButton.transform.localRotation = Quaternion.identity;
			newButton.GetComponent<LevelButton>().SetLevel(i);
			if(i > levelCompleted + 1) {
				newButton.GetComponent<Button>().interactable = false;
			}
			if(i == levelCount) {
				newButton.GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f);
			}
		}
	}
}
}