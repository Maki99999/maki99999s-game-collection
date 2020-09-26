using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FalseTruth {
public class C00EndPart2 : MonoBehaviour {

	public GameObject fadeWhite;	//Object for fading white
	public Animator fadeBlack;		//Animator for fading black

	public Camera camOld;		//Player camera
	public Camera camNew;		//Clock camera

	void Start () {
		StartCoroutine(Cutscene());
	}

	IEnumerator Cutscene() {
		fadeWhite.SetActive(true);			//Activates Object and fading animation
		yield return new WaitForSeconds(1);	//Waits for animation

		camNew.enabled = true;			//Enables new camera
		camOld.enabled = false;			//Disables old camera

		fadeWhite.GetComponent<Animator>().SetTrigger("Clear");	//Starts defading animation
		yield return new WaitForSeconds(4);						//Waits for animation

		fadeBlack.SetTrigger("SceneEnd");		//Starts fading animation
		yield return new WaitForSeconds(3);		//Waits for animation

		SceneManager.LoadScene("01 - Nothing Happened");	//Loads next scene
	}
}
}