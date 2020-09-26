using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class LockedOccultDoor : FalseTruth.Useable {

	FalseTruth.Cutscene cutscene;

	void Start () {
		cutscene = GetComponent<FalseTruth.Cutscene>();
	}
	
	public override void Use() {
		cutscene.StartCutscene();
	}
}
}