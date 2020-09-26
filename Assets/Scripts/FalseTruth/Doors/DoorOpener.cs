using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth {
public class DoorOpener : FalseTruth.Useable {

	Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	public override void Use() {
		anim.SetBool("Open", !anim.GetBool("Open"));	//Toggles between opened and closed
		StopAllCoroutines();
		StartCoroutine(CloseAfterTime());				//Forces closed door after 5 seconds
	}

	IEnumerator CloseAfterTime() {
		yield return new WaitForSeconds(5);		//Waits 5 seconds
		anim.SetBool("Open", false);			//Closes the door
	}
}
}