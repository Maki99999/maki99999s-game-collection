using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class DodgePlayer : MonoBehaviour {

    GameControllerDodge gameControllerDodge;

    void Start() {
        gameControllerDodge = GetComponentInParent<GameControllerDodge>();
    }

    void OnCollisionEnter(Collision other) {
        if(other.collider.CompareTag("Projectile")) {
            gameControllerDodge.Loose();
        }
    }
}
}