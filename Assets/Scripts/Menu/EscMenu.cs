using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour {

	public string GameMenuSceneName;
	public Animator fadeAnimator;

	Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			ToggleMenu();
		}
	}

	void OnGUI() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			ToggleMenu();
		}
	}

	public void ToggleMenu() {
		animator.SetBool("Open Menu", !animator.GetBool("Open Menu"));
		Cursor.visible = !Cursor.visible;
		Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
	}

	public void ToMainMenu() {
		StartCoroutine(ToScene("MainMenu"));
	}

	public void ToGameMenu() {
		StartCoroutine(ToScene(GameMenuSceneName));
	}

	public IEnumerator ToScene(string sceneName) {
		fadeAnimator.SetTrigger("Out");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(sceneName);
	}
}
