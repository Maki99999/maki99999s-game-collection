using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRoll3 {
public class End : MonoBehaviour {

	GameController gameController;

	bool activated = false;

	void Start() {
		gameController = FindObjectOfType<GameController>();
	}

	void OnTriggerEnter(Collider other) {
		if(!activated && gameController.hasEnoughPieces && other.CompareTag("Player")) {
			activated = true;
			gameController.NextLevel();
		}
	}
}
}