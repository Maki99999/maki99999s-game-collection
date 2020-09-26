using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class ParkourGhost : MonoBehaviour {
    
    public List<Transform> path;
    public Transform pathInit;
    public float moveSpeed;
    public float rotateSpeed;

    GameControllerParkour gameControllerParkour;

    void Start() {
        gameControllerParkour = GetComponentInParent<GameControllerParkour>();
    }

    public void StartWalk() {
        StopAllCoroutines();
        transform.position = pathInit.position;
        transform.rotation = pathInit.rotation;
        StartCoroutine(Walk());
    }

    public void StopWalk() {
        StopAllCoroutines();
        StartCoroutine(MainController.MoveObject(transform, transform.position - 5 * Vector3.up, transform.rotation, 60));
    }

    IEnumerator Walk() {
        while(true) {
            for(int i = 0; i < path.Count; i++) {
                while(transform.position != path[(i + 1) % path.Count].position) {
                    transform.position = Vector3.MoveTowards(transform.position, path[(i + 1) % path.Count].position, moveSpeed * Time.deltaTime);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, path[(i + 1) % path.Count].rotation, rotateSpeed * Time.deltaTime);
                    yield return new WaitUntil(() => !GameControllerParkour.paused.Value);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            gameControllerParkour.Loose();
        }
    }
}
}