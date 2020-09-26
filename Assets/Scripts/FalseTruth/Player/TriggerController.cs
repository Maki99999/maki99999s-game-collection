using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class TriggerController : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("NPC")) {
			other.gameObject.GetComponent<FalseTruth.Avoider>().isAvoiding = true;
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.CompareTag("NPC")) {
			other.gameObject.GetComponent<FalseTruth.Avoider>().isAvoiding = false;
		}
	}
}
}