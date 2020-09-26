using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public abstract class Cutscene : MonoBehaviour{
	public abstract IEnumerator TheCutscene();
	
	public void StartCutscene() {
		StartCoroutine(TheCutscene());
	}
}
}