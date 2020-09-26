using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class CollectibleExtended : MonoBehaviour {
	
	public Vector3 rotation;

	Collectible collectible;

	void Start () {
		collectible = GetComponent<Collectible>();

		StartCoroutine(LateStart());
	}

	IEnumerator LateStart() {
		yield return new WaitForEndOfFrame();
		collectible.Sync(rotation);
	}
}
}