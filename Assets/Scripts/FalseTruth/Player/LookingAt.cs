using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class LookingAt : MonoBehaviour {

	public Transform target;

	void Update() {
		transform.LookAt(target);
	}
}
}