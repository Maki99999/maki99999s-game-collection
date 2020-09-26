using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class Deadly : MonoBehaviour {

    GameControllerParkour gameControllerParkour;

    void Start() {
        gameControllerParkour = GetComponentInParent<GameControllerParkour>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            gameControllerParkour.CorrectIllegalPosition();
        }
    }

    void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player")) {
            gameControllerParkour.CorrectIllegalPosition();
        }
    }
}
}