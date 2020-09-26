using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class ParkourCheckpoint : MonoBehaviour {

    GameControllerParkour gameControllerParkour;

    public bool activated = false;
    public bool ghostActivated = false;
    public bool isFinish = false;
    
    void Start() {
        gameControllerParkour = GetComponentInParent<GameControllerParkour>();
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            if(isFinish) {
                gameControllerParkour.hasWon();
            }
            if(!activated) {
                activated = true;
                gameControllerParkour.AddCp();
            }
        } else if(other.CompareTag("Ghost")) {
            Ghost();
        }
    }

    public void Ghost() {
        if(!ghostActivated) {
            ghostActivated = true;
            gameControllerParkour.AddGhostCp();
        }
    }
}
}