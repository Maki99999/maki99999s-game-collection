using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class MasterClock : MonoBehaviour {

	public float secondsPerSecond;	//Determines how many seconds on the in-game clock should pass per real-time second
	public Transform big;			//Big hand
	public Transform small;			//Small hand

	public float startHour = 0;
	public float startMinute = 0;


	float currentMinute;			//The current minute (0 = 0:00, 719 = 11:59)
	
	void Update () {
		if(FalseTruth.GameController.paused) return;		//Doesn't do anything when the game is marked as paused

		currentMinute = (currentMinute + secondsPerSecond * Time.deltaTime / 60) % 720;	//Adds 1 time unit to the clock
		addTime(secondsPerSecond / 60 * Time.deltaTime);								//
	}

	public void addTime(float minutes) {		//Adds minutes minutes to the clock
		small.Rotate(Vector3.left * minutes / 2); 	// 360 / 60 / 12 = 1 / 2
		big.Rotate(Vector3.left * minutes * 6);		// 360 / 60 = 6
	}

	public void SetTime(float hour, float minute) {		//Sets the time to hour:minute
		float nextTime = (hour % 12) * 60 + (minute % 60);	//Converts hours & minutes to minutes only
		float diffTime = nextTime - currentMinute;			//The difference between new and old time
		currentMinute = nextTime;							//Updates the current minute
		
		if(Mathf.Sign(diffTime) == -1) {		//Clock shouldn't go backwards, so
			diffTime = 720 + diffTime;			//	it goes forward
		}

		addTime(diffTime);		//Adds the difference
	}
}
}