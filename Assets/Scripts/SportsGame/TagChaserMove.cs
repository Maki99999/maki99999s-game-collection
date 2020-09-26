using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SportsGame {
public class TagChaserMove : TagNPCMove {

	public float maxTurnDegree = 30;
	public float turnSpeed = 1;

	Transform player;

	void Start () {
		rig = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		gameController = GetComponentInParent<GameControllerTag>();
		if(gameController == null) {
			isFake = true;
			fakeController = GetComponentInParent<FakeController>();
		}
		
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject == null ? null : playerObject.transform;

		StartCoroutine(WalkIn());
		
		InitArea();
	}

	void FixedUpdate() {
		if(GameControllerTag.paused.Value) return;
		if(walkIn) {
			rig.MovePosition(transform.position + transform.forward * walkSpeed);
			return;
		}

		if(player == null || isFake || !gameController.area.Contains(player.position)) {
			Walk();
			return;
		}

		Chase();
	}

	void Chase() {
		float angleToPlayer = Vector3.Angle(transform.forward, player.position - transform.position);
		float direction = Vector3.Cross(transform.forward, (player.position - transform.position).normalized).y;
		if(angleToPlayer > 170) {
			direction++;
		}

		if(angleToPlayer < maxTurnDegree) {
			transform.Rotate(new Vector3(0, direction * angleToPlayer, 0));
		} else {
			transform.Rotate(new Vector3(0, direction * maxTurnDegree, 0));
		}

		Vector3 newPos = transform.position + transform.forward * walkSpeed;
		rig.MovePosition(newPos);
	}

	IEnumerator WalkIn() {
		yield return new WaitUntil(() => area.Contains(transform.position));
		walkIn = false;
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag.Equals("Player")) {
			gameController.Loose();
		}
	}
}
}