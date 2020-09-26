using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FalseTruth {
public class GameController : MonoBehaviour {

	public static bool paused;		//Marks the game as paused or unpaused
	
	static AudioSource audioSource;	//Background music

	static int unlockCount = 0;		//Number of unlocks

	public Animator fadeBlack;

	void Awake () {
		paused = false;
	}

	void Start() {
		audioSource = GetComponent<AudioSource>();	//Starts the background music
		if(audioSource != null) audioSource.Play();	//

		Cursor.lockState = CursorLockMode.Locked;	//Lock the mouse
		Cursor.visible = false;						//Hide the cursor
	}

	public void EndScene(string newScene) {
		StartCoroutine(SceneEnd(newScene));
	}

	IEnumerator SceneEnd(string newScene) {
		fadeBlack.SetTrigger("SceneEnd");
		yield return new WaitForSeconds(3f);

		PlayerPrefs.SetInt("Checkpoint", -1);
		int newSceneNumber;
		if(int.TryParse(newScene.Substring(0, 2), out newSceneNumber))
			PlayerPrefs.SetInt("Scene", newSceneNumber);

		SceneManager.LoadScene(newScene);
	}

	public static void LockMouse() {
		if(--unlockCount <= 0) {
			Cursor.lockState = CursorLockMode.Locked;	//Lock the mouse
			Cursor.visible = false;						//Hide the cursor
		}
	}

	public static void UnlockMouse() {
		if(unlockCount++ >= 0) {
			Cursor.lockState = CursorLockMode.None;		//Unlock the mouse
			Cursor.visible = true;						//Show the cursor
		}
	}

	public static IEnumerator GoTo(Transform obj, Vector3 newPosition, int frameCount) {
		Vector3 oldPosition = obj.position;
		for(float i = 0f; i <= 1; i += 1f / frameCount) {
			obj.position = Vector3.Lerp(oldPosition, newPosition, i);
			yield return new WaitForSeconds(1f / 60f);
		}
		obj.position = newPosition;
	}

	public static IEnumerator RotateTo(Transform obj, Quaternion newRotation, int frameCount) {
		Quaternion oldRotation = obj.rotation;
		for(float i = 0f; i <= 1; i += 1f / frameCount) {
			obj.rotation = Quaternion.Lerp(oldRotation, newRotation, i);
			yield return new WaitForSeconds(1f / 60f);
		}
		obj.rotation = newRotation;
	}
}
}