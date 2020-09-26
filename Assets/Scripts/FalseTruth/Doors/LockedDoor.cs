using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class LockedDoor : FalseTruth.Useable {

	Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	}
	
	public override void Use() {
		anim.SetTrigger("Open");	//Play Animation
	}
}
}