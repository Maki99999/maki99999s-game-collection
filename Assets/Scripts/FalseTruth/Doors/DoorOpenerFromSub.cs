using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FalseTruth {
public class DoorOpenerFromSub : FalseTruth.Useable {

	Animator anim;

	void Start () {
		anim = transform.parent.GetComponent<Animator>();
	}
	
	public override void Use() {
		anim.SetBool("Open", !anim.GetBool("Open"));	//Toggles between opened and closed
	}
}
}