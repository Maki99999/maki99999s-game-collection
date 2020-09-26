using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class NPCMove : FalseTruth.Avoider {

	public float timeBetweenWalks = 5;		//The time between walks
	public float walkSpeed = 1;				//The walk speed

	float defaultWalkSpeed;	//The default walk speed
	Rigidbody rig;			//The rigidbody
	Vector3 newPosition;	//The desired position 
	Vector3 avoidPosition;	//The desired position when avoiding
	bool isWalking;			//Marks the object as walking / not walking

	void Start () {
		rig = GetComponent<Rigidbody>();		//Get the components
		defaultWalkSpeed = walkSpeed;			//Sets the default walk speed

		StartCoroutine(Walk());					//Starts the walk method
	}

	void FixedUpdate() {
		if(FalseTruth.GameController.paused) return;	//Do nothing when paused
		if(isAvoiding) {					//If the object is avoiding the player
			isWalking = true;										//Lets the object walk
			newPosition = transform.position - transform.forward;	//New position is backwards
			walkSpeed = defaultWalkSpeed * 2;											//Doubles the speed
		} else {
			walkSpeed = defaultWalkSpeed;							//Resets the speed
		}
		if(isWalking) {									//If the object is walking
			newPosition.y = transform.position.y;		//Set the y coordinate to the current y coordinate, so it doesn't change
			rig.MovePosition(Vector3.Lerp(transform.position, newPosition, Time.deltaTime * walkSpeed));	//Move the object
		}
	}
	
	IEnumerator Walk() {
		yield return new WaitForSeconds(1);	//Wait for the object to hit the floor
		while(true) {				//Forever do
			if(!isAvoiding) {			//If the object is not avoiding the player
				isWalking = !isWalking;		//Toggle walking
				newPosition = transform.position + Random.insideUnitSphere;		//Random new Position
			}
			yield return new WaitForSeconds(Random.Range(timeBetweenWalks * 3 / 5, timeBetweenWalks * 7 / 5));	//Wait
		}
	}
}
}