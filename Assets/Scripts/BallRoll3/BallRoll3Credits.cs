using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BallRoll3 {
public class BallRoll3Credits : MonoBehaviour {

	public Animator fadeBlack;
	public RectTransform creditText;
	public Animator backAnimator;

	bool stopAnvoiding;

	void Start () {
		StartCoroutine(CreditsScroll());
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
	public void Back() {
		StartCoroutine(LoadScene("BallRoll3Menu"));
	}

	public void AvoidMouse() {
		if(stopAnvoiding) return;
		backAnimator.SetTrigger("Move");
	}

	IEnumerator CreditsScroll() {
		for(float f = -1f; f <= 2f; f += 0.001f) {
			creditText.anchorMin = new Vector2(0, f);
			creditText.anchorMax = new Vector2(1, f + 1);
			yield return new WaitForSeconds(1f / 60f);
		}
		stopAnvoiding = true;
	}

	IEnumerator LoadScene(string name) {
		fadeBlack.SetTrigger("Out");
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(name);
	}
}
}