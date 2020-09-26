using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class SlowDownPlayer : MonoBehaviour {

	public int direction;		//0 - north, 1 - east, 2 - south, 3 - west

	FalseTruth.SlowDowner playerSlowDowner;	//The SlowDown script from the player

	void Start() {
		playerSlowDowner = GameObject.FindWithTag("Player").GetComponent<FalseTruth.SlowDowner>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			playerSlowDowner.slowDirection = direction;		//Updates the direction the player slows down
		}
	}

	void OnTriggerStay(Collider other) {
		if(other.CompareTag("Player")) {
			Vector3 pointPlayer = other.transform.position;		//Position of the Player
			Vector3 pointBegin = transform.position;			//Position of one side of the SlowDownCube
			Vector3 pointEnd = transform.position;				//Position of the opposite of the SlowDownCube

			pointPlayer.y = 0;		//Ignore heights
			pointBegin.y = 0;
			pointEnd.y = 0;

			switch(direction) {		//Ignore another axis and set the actual position of pointBegin and pointEnd
				case 0:
					pointPlayer.x = 0;
					pointBegin.x = 0;
					pointEnd.x = 0;

					pointBegin.z -= transform.localScale.z / 2 - 0.1f;
					pointEnd.z += transform.localScale.z / 2 + 0.1f;
					
					break;
				case 1:
					pointPlayer.z = 0;
					pointBegin.z = 0;
					pointEnd.z = 0;
					
					pointBegin.x -= transform.localScale.x / 2;
					pointEnd.x += transform.localScale.x / 2;

					break;
				case 2:
					pointPlayer.x = 0;
					pointBegin.x = 0;
					pointEnd.x = 0;
					
					pointBegin.z += transform.localScale.z / 2;
					pointEnd.z -= transform.localScale.z / 2;

					break;
				case 3:
					pointPlayer.z = 0;
					pointBegin.z = 0;
					pointEnd.z = 0;

					pointBegin.x += transform.localScale.x / 2;
					pointEnd.x -= transform.localScale.x / 2;

					break;
			}
			float distanceOverall = Vector3.Distance(pointBegin, pointEnd);			//The Distance form the beginning to the end of the SlowDownCube
			float distanceFromPlayer = Vector3.Distance(pointPlayer, pointEnd);		//The Distance form the player to the end of the SlowDownCube


			playerSlowDowner.SetSlowSpeed((distanceFromPlayer / distanceOverall) * (distanceFromPlayer / distanceOverall));		//Slows down the player
		}
	}
/*
	void OnTriggerExit(Collider other) {
		if(other.CompareTag("Player")) {
			playerSlowDowner.SetSlowSpeed(1f);
		}
	}*/
}
}